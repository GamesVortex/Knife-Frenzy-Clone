using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

	public static GameControllerScript instance;

	public GameObject knifePrefab;
	public GameObject knifeSpawnPosition;
	public GameObject targetCircleObjectHolder;
	public GameObject knifeParticles;
	public GameObject currentKnife;
	public Target[] targets;
	public Challenge[] challenges;
	public Knife[] knives;

	private GameObject targetCircle;
	private bool canShoot;
	private bool isPlaying;
	private ParticleSystem knifeParticleSystem;
	private int challengesCompleted;
	private int selectedKnifeId;
	private Knife selectedKnife;

	public bool IsPlaying { set { isPlaying = value; } get { return isPlaying; } }
	public Challenge[] Challenges { get { return challenges; } }
	public Target[] Targets { get { return targets; } }
	public Knife[] Knives { get { return knives; } }
	public int ChallengesCompleted { set { challengesCompleted = value; } get { return challengesCompleted; } }
	public int SelectedKnifeId { 
		set { 
			selectedKnifeId = value; 
			selectedKnife = knives[selectedKnifeId];
			StageControllerScript.instance.SelectedKnife = selectedKnife;
			UIManagerScript.instance.UpdateStartPanelKnifeImage();
		} 
		get { return selectedKnifeId; } 	
	}

	public Knife SelectedKnife { get { return selectedKnife; } }

	public GameObject CenterCircle { 
		set { 
			targetCircle = value;
			knifeParticles = targetCircle.transform.GetChild (1).gameObject;
			targetCircleObjectHolder = targetCircle.transform.GetChild (2).gameObject;
			knifeParticleSystem = knifeParticles.transform.GetChild(0).GetComponent<ParticleSystem> ();
		} 
	}

	public bool CanShoot { set { canShoot = value; } }

	void Awake(){
		instance = this;

		PlayerPrefs.SetInt ("ChallengesCompleted", 10);
		challengesCompleted = PlayerPrefs.GetInt ("ChallengesCompleted");
	}

	void Start(){
		SelectedKnifeId = PlayerPrefs.GetInt("SelectedKnife");
	}

	public void PlayGame(){
		//isPlaying = true;
		StageControllerScript.instance.IsChallenge = false;
		StageControllerScript.instance.SetStage ();
	}

	public void PlayChallenge(int challengeId){
		StageControllerScript.instance.IsChallenge = true;
		StageControllerScript.instance.Challenge = challenges [challengeId - 1];
		StageControllerScript.instance.ChallengeTarget = targets [challengeId];
		StageControllerScript.instance.SetStage ();
	}

	public void SpawnKnife(){
		currentKnife = Instantiate (knifePrefab, knifeSpawnPosition.transform.position, Quaternion.identity) as GameObject;
		currentKnife.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite =
			currentKnife.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite =
			currentKnife.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = selectedKnife.sprite;
		currentKnife.GetComponent<KnifeControllerScript>().KnifeNo = StageControllerScript.instance.KnifeCount;
	}

	public void DeactivateKnife (){
		currentKnife.GetComponent<Animator> ().SetTrigger ("DoDeactivate");
	}

	public void OnInput(){
		if (canShoot) {
			AudioManagerScript.instance.PlayKnifeThrowAudio();
			currentKnife.GetComponent<KnifeControllerScript> ().ShootKnife = true;
			if (StageControllerScript.instance.DoSpawnKnife ()) {
				SpawnKnife ();
			}
		}
	}

	public void MakeChildOfTargetCircle(GameObject knife){
		knife.transform.SetParent (targetCircleObjectHolder.transform, true);
		knifeParticles.transform.rotation = Quaternion.identity;
		//knifeParticleSystem.Clear ();
		knifeParticleSystem.Play ();
	}

	public void GameOver(){
		canShoot = false;
		isPlaying = false;
		ScoreControllerScript.instance.UpdateGameOverMenu ();
		StartCoroutine (WaitBeforeChangeScene());

		int bestScore = PlayerPrefs.GetInt("BestScore");
		int bestStage = PlayerPrefs.GetInt("BestStage");

		if(StageControllerScript.instance.StageNo > bestStage){
			PlayerPrefs.SetInt("BestStage", StageControllerScript.instance.StageNo);
			UIManagerScript.instance.UpdateBestStageText(StageControllerScript.instance.StageNo);
		}

		if(ScoreControllerScript.instance.Score > bestScore){
			PlayerPrefs.SetInt("BestScore", ScoreControllerScript.instance.Score);
			UIManagerScript.instance.UpdateBestScoreText(ScoreControllerScript.instance.Score);			
		}
	}

	public void ShineTarget(){
		targetCircle.GetComponent<Animator> ().SetTrigger ("DoShine");
	}

	public Target GetRandomTarget(){
		return targets [Random.Range (0, challengesCompleted)];
	}
		
	IEnumerator WaitBeforeChangeScene(){
		yield return new WaitForSeconds (1);
		UIManagerScript.instance.OnGameOver ();
	}
}

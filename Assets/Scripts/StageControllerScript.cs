using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageControllerScript : MonoBehaviour {

	public static StageControllerScript instance;

	public int knifeCount;

	public bool doReverse;
	public float reverseAfterTime;
	public float rotationSpeed;
	public float reverseInterpolationInterval;

	public bool symetricPosition;
	public int coinAmount;
	public int preKnifeAmount;

	public GameObject preKnifePrefab;
	public GameObject coinPrefab;
	public GameObject centerCirclePrefab;
	public GameObject centerCircleObjectHolder;

	private float stageStartTime;
	private float startRotationSpeed;
	private float endRotationSpeed;
	private float interpolationTime;

	private Knife selectedKnife;

	private bool doTransition;
	private GameObject inGameObject;
	private GameObject outGameObject;
	public Vector3 centerPosition;
	public Vector3 leftPosition;
	public Vector3 rightPosition;
	public float transitionInterval;

	private bool doWait;
	public float waitTime;
	private float stageCompletionTime;

	private bool startRotation;
	private int stageNo;
	private bool goLeft;

	private bool isChallenge;
	private Challenge challenge;
	private Target challengeTarget;
	private bool isLastChallengeStage;

	public int StageNo{ get { return stageNo; } }
	public bool IsChallenge { set { isChallenge = value; } }
	public Challenge Challenge { set { challenge = value; } }
	public Target ChallengeTarget { set { challengeTarget = value; } }
	public int KnifeCount { set { knifeCount = value; } get { return knifeCount; } }
	public Knife SelectedKnife { set { selectedKnife = value; } }

	void Awake(){
		instance = this;
	}

	void Update(){
		if (GameControllerScript.instance.IsPlaying) {
			if (doReverse && Time.time >= stageStartTime + reverseAfterTime) {
				interpolationTime += reverseInterpolationInterval * Time.deltaTime;
				rotationSpeed = Mathf.Lerp (startRotationSpeed, endRotationSpeed, interpolationTime);
				if (interpolationTime >= 1) {
					SetReverseRotationValues ();
				}
			}

			if (inGameObject != null) {
				inGameObject.transform.Rotate (Vector3.forward * rotationSpeed * Time.deltaTime);
			}

			if (doWait && Time.time >= waitTime + stageCompletionTime) {
				doWait = false;
				doTransition = true;
				goLeft = true;
			}
		}

		if (doTransition) {
			interpolationTime += transitionInterval * Time.deltaTime;
			if (outGameObject != null) {
				outGameObject.transform.position = Vector3.Lerp (centerPosition, goLeft ? leftPosition : rightPosition, interpolationTime);
			}
			if (inGameObject != null) {
				inGameObject.transform.position = Vector3.Lerp (goLeft ? rightPosition : leftPosition, centerPosition, interpolationTime);
			}
			if (interpolationTime >= 1) {
				doTransition = false;
				if (GameControllerScript.instance.IsPlaying && ((isChallenge && !isLastChallengeStage) || !isChallenge)) {
					startRotation = true;
					GameControllerScript.instance.SpawnKnife ();
					GameControllerScript.instance.CanShoot = true;
					UIManagerScript.instance.UpdateStageCountText(stageNo);
				}
				if (outGameObject != null) {
					ClearLastCenterCircle ();
				}
			}
		}
	}
		
	public void SetStage(){
		stageNo++;
		startRotation = false;
		// Clear Stage and UI
		KnifePanelControllerScript.instance.ClearUI ();

		inGameObject = Instantiate (centerCirclePrefab, rightPosition, Quaternion.identity) as GameObject;
		centerCircleObjectHolder = inGameObject.transform.GetChild (2).gameObject;
		GameControllerScript.instance.CenterCircle = inGameObject;

		// Reset to new values
		if (!isChallenge) { // if the stage is to be set to random config
			knifeCount = Random.Range (5, 10);
			preKnifeAmount = Random.Range (0, 5);
			coinAmount = Random.Range (0, 6);
			rotationSpeed = Random.Range (100, 150);
			if (Random.Range (0, 1) == 0) {
				doReverse = true;
			} else {
				doReverse = false;
			}
			if (Random.Range (0, 1) == 0) {
				symetricPosition = true;
			} else {
				symetricPosition = false;
			}
		} else { // if the stage is to be set to challenge config
			Stage stage = challenge.stages[stageNo - 1];

			knifeCount = stage.knifeCount;
			preKnifeAmount = stage.preKnifeAmount;
			coinAmount = stage.coinAmount;
			rotationSpeed = stage.rotationSpeed;

			doReverse = stage.doReverse;
			symetricPosition = stage.symetricPosition;
		}
			
		// Update Stage with the values
		if (preKnifeAmount > 0 || coinAmount > 0) {
			SetPreObjects ();
		}
		if (doReverse) {
			SetReverseRotationValues ();
		}
		KnifePanelControllerScript.instance.CreateKnifes (knifeCount);
		GameControllerScript.instance.IsPlaying = true;
	}

	public bool DoSpawnKnife(){
		knifeCount--;
		if (knifeCount > 0) {
			return true;
		}
		return false;
	}

	public void UpdateKnifeCountUI(int knifeNo){
		KnifePanelControllerScript.instance.DarkenKnife (knifeNo-1);
	}

	void SetPreObjects(){
		float lastSpawnRotation = 0;
		float totalObjects = coinAmount + preKnifeAmount;
		float spawnRotationOffset;
		if (symetricPosition) {
			spawnRotationOffset = 360 / (totalObjects);
		} else {
			spawnRotationOffset = 360 / Random.Range (totalObjects + 2, totalObjects + 5);
		}
		int knifesLeft = preKnifeAmount;
		int coinsLeft = coinAmount; 
		int lastSpawnItem = 0; // 0 = coin, 1 = knife

		for (int i = 0; i < totalObjects; i++) {
			GameObject spawnedObject;
			if (knifesLeft > 0 && (lastSpawnItem == 0 || coinsLeft == 0)) {
				knifesLeft--;
				lastSpawnItem = 1;
				spawnedObject = Instantiate (preKnifePrefab, preKnifePrefab.transform.position, Quaternion.Euler (0, 0, lastSpawnRotation)) as GameObject;
				spawnedObject.transform.GetChild(0).transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = 
					spawnedObject.transform.GetChild(0).transform.GetChild(3).GetComponent<SpriteRenderer>().sprite =
					spawnedObject.transform.GetChild(0).transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = selectedKnife.Sprite;
			} else {
				lastSpawnItem = 0;
				coinsLeft--;
				spawnedObject = Instantiate (coinPrefab, coinPrefab.transform.position, Quaternion.Euler (0, 0, lastSpawnRotation)) as GameObject;
			}
			spawnedObject.transform.SetParent (centerCircleObjectHolder.transform, false);
			lastSpawnRotation += spawnRotationOffset;
		}
	}

	void SetReverseRotationValues (){
		reverseAfterTime = Random.Range (3, 10);
		reverseInterpolationInterval = Random.Range (0.1f, 0.3f);
		stageStartTime = Time.time;
		startRotationSpeed = rotationSpeed;
		endRotationSpeed = -rotationSpeed;
		interpolationTime = 0;
	}

	// if it was the last knife
	public void CheckIfLastKnife(){
		if (knifeCount <= 0) {
			GameControllerScript.instance.CanShoot = false;
			outGameObject = inGameObject;

			doWait = true;
			stageCompletionTime = Time.time;

			if (isChallenge && stageNo == challenge.stages.Length) {
				Debug.Log ("ChallengeCompleted");
				DoTransition (null, inGameObject, true);
				UIManagerScript.instance.OnChallengeCompletion(challengeTarget);

				isLastChallengeStage = true;
			} else {
				SetStage ();
			}
		}
	}

	void ClearLastCenterCircle(){
		centerCircleObjectHolder = outGameObject.transform.GetChild (2).gameObject;
		if (centerCircleObjectHolder != null) {
			int childCount = centerCircleObjectHolder.transform.childCount;
			if (childCount > 0) {
				for (int i = childCount - 1; i >= 0; i--) {
					Destroy (centerCircleObjectHolder.transform.GetChild (i).gameObject);
				}
			}
		}
		Destroy (outGameObject);
	}

	public void TransitionOnPlay(){
		doWait = true;
	}

	public void TransitionOnGameOver(){
		DoTransition (null, inGameObject, true);
		GameControllerScript.instance.DeactivateKnife ();
	}

	public void TransitionOnResume(){
		int childCount = outGameObject.transform.GetChild(2).childCount;
		for (int i = childCount - 1; i >= 0; i--) {
			if (outGameObject.transform.GetChild (2).transform.GetChild (i).CompareTag ("Knife")) {
				Destroy(outGameObject.transform.GetChild (2).transform.GetChild (i).gameObject);
				ScoreControllerScript.instance.AddScore (-1);
				knifeCount++;
			}
		}
		knifeCount++;
		KnifePanelControllerScript.instance.ClearUI ();
		KnifePanelControllerScript.instance.CreateKnifes (knifeCount);
		DoTransition (outGameObject, null, false);
	}

	public void TransitionOnRestart(){
		Reset ();
		if (outGameObject != null) {
			ClearLastCenterCircle ();
		}
		SetStage ();
		DoTransition (inGameObject, null, false);
	}

	public void TransitionOnHome(){
		Reset ();
		if (outGameObject != null) {
			ClearLastCenterCircle ();
		}
	}

	private void DoTransition(GameObject inGameObject, GameObject outGameObject, bool goLeft){
		this.outGameObject = outGameObject;
		this.inGameObject = inGameObject;
		this.goLeft = goLeft;
		doTransition = true;
		interpolationTime = 0;
	}

	void Reset(){
		stageNo = 0;
		ScoreControllerScript.instance.ResetScore ();
		ScoreControllerScript.instance.ResetCoins ();
	}
}

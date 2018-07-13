using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengePanelScript : MonoBehaviour {

	public static ChallengePanelScript instance;

	public GameObject scrollViewContent;
	public GameObject challengeButtonHolderPrefab;

	public Sprite lockSprite;
	public Color lockColor;
	public Color darkLockColor;

	private int id;
	private Target[] targets;
	private Challenge[] challenges;
	private int challengeCount;
	private int challengesCompleted;
	private int nextChallengeID;

	public int NextChallengeID { get { return nextChallengeID; } }

	void Awake(){
		instance = this;
	}

	void Start() { 
		challengesCompleted = GameControllerScript.instance.ChallengesCompleted;

		targets = GameControllerScript.instance.Targets;
		challenges = GameControllerScript.instance.Challenges;

		challengeCount = challenges.Length;
		nextChallengeID = 1;

		for (int i = 0; i < challengeCount; i++) {
			GameObject challengeButtonHolder = Instantiate (challengeButtonHolderPrefab);
			challengeButtonHolder.transform.SetParent (scrollViewContent.transform, false);
			challengeButtonHolder.GetComponent<ChallengeButtonHolderScript> ().ChallengeId = challenges[i].id;

			if (i < challengesCompleted) {
				challengeButtonHolder.transform.GetChild (0).GetComponent<Image>().sprite = targets[challenges[i].targetId].Sprite;

				challengeButtonHolder.transform.GetChild (1).transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = 
					challengeButtonHolder.transform.GetChild (1).transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = targets [challenges [i].targetId].Name;

				challengeButtonHolder.transform.GetChild (1).gameObject.SetActive (true);
				challengeButtonHolder.transform.GetChild (2).gameObject.SetActive (false);
				nextChallengeID++;
			} else {
				challengeButtonHolder.transform.GetChild (0).GetComponent<Image> ().sprite = lockSprite;

				challengeButtonHolder.transform.GetChild (0).GetComponent<Image> ().color = darkLockColor;

				challengeButtonHolder.transform.GetChild (1).gameObject.SetActive (false);
				challengeButtonHolder.transform.GetChild (2).gameObject.SetActive (false);

				// if it is the next challenge
				if(i == nextChallengeID - 1){
					challengeButtonHolder.transform.GetChild (2).gameObject.SetActive (true);

					challengeButtonHolder.transform.GetChild (2).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
						challengeButtonHolder.transform.GetChild (2).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = challenges[i].coinCost.ToString();
						challengeButtonHolder.transform.GetChild (0).GetComponent<Image> ().color = lockColor;

					challengeButtonHolder.transform.GetChild(0).GetComponent<Button> ().enabled = true;
				}
			}
			
			challengeButtonHolder.transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => UIManagerScript.instance.OnPlayChallenege (challengeButtonHolder.GetComponent<ChallengeButtonHolderScript>().ChallengeId));
			challengeButtonHolder.transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => AudioManagerScript.instance.PlayUIButtonAudio());
		}
	}

	public void UpdateChallengeDetailsAfterCompletion(){

		int challengeCount = PlayerPrefs.GetInt ("ChallengesCompleted");
		PlayerPrefs.SetInt ("ChallengesCompleted", challengeCount + 1);
		// store the present challengeButtonHolder
		GameObject challengeButtonHolder = scrollViewContent.transform.GetChild(challengeCount).gameObject;
		
		challengeButtonHolder.transform.GetChild (0).GetComponent<Image>().sprite = targets[challenges[challengeCount].targetId].Sprite;
		
		challengeButtonHolder.transform.GetChild (0).GetComponent<Image> ().color = new Color(255, 255, 255, 1);

		challengeButtonHolder.transform.GetChild (1).transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = 
			challengeButtonHolder.transform.GetChild (1).transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = targets [challenges [challengeCount].targetId].Name;

		challengeButtonHolder.transform.GetChild (1).gameObject.SetActive (true);
		challengeButtonHolder.transform.GetChild (2).gameObject.SetActive (false);
		challengeButtonHolder.transform.GetChild(0).GetComponent<Button> ().enabled = false;

		nextChallengeID++;
		
		// store the next challengeButtonHolder
		challengeButtonHolder = scrollViewContent.transform.GetChild(nextChallengeID - 1).gameObject;

		challengeButtonHolder.transform.GetChild (2).gameObject.SetActive (true);

		challengeButtonHolder.transform.GetChild (2).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
			challengeButtonHolder.transform.GetChild (2).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = challenges[nextChallengeID - 1].coinCost.ToString();
			challengeButtonHolder.transform.GetChild (0).GetComponent<Image> ().color = lockColor;

		challengeButtonHolder.transform.GetChild(0).GetComponent<Button> ().enabled = true;
	}

	public bool CanPlayChallenge(){
		if(challenges[nextChallengeID - 1].coinCost <= ScoreControllerScript.instance.CoinAmount){
			return true;
		}
		return false;
	}
}

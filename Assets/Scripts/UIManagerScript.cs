using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour {

	public static UIManagerScript instance;

	public GameObject leftPanelHolder;
	public GameObject centerPanelHolder;
	public GameObject rightPanelHolder;
	public GameObject topPanelHolder;
	public GameObject bottomPanelHolder;

	public GameObject touchBlockerPanel;
	public GameObject inGamePanel;
	public GameObject startPanel;
	public GameObject gameOverPanel;
	public GameObject settingsPanel;
	public GameObject knifeSelectionPanel;
	public GameObject challengeSelectionPanel;
	public GameObject challengeCompletionPanel;
	public GameObject getCoinsPanel;

	public TextMeshProUGUI bestScoreText;
	public TextMeshProUGUI bestScoreTextFront;
	public TextMeshProUGUI bestStageText;
	public TextMeshProUGUI bestStageTextFront;

	public TextMeshProUGUI stageCountText;
	public TextMeshProUGUI stageCountTextFront;

	public GameObject coinParticleSystem;
	public GameObject inGamePanelCoinImage;

	public Image startPanelKnifeImage;
	public Image startPanelKnifeImageShadow;

	public float transitionInterval;
	private GameObject outMenu;
	private GameObject inMenu;

	private float interpolationTime;
	private bool doTransition;
	private Vector3 centerPosition;
	private Vector3 discardPosition;
	private Vector3 spawnPosition;

	void Awake(){
		instance = this;
	}

	void Start(){
		centerPosition = centerPanelHolder.transform.position;
		UpdateBestScoreText( PlayerPrefs.GetInt("BestScore"));
		UpdateBestStageText(PlayerPrefs.GetInt("BestStage"));
	}

	void Update(){
		if (doTransition) {
			interpolationTime += transitionInterval * Time.deltaTime;
			if (outMenu != null) {
				outMenu.transform.position = Vector3.Lerp (centerPosition, discardPosition, interpolationTime);
			}
			if (inMenu != null) {
				inMenu.transform.position = Vector3.Lerp (spawnPosition, centerPosition, interpolationTime);
			}

			if (interpolationTime >= 1) {
				touchBlockerPanel.SetActive (false);
				outMenu.SetActive (false);
				doTransition = false;
			}
		}
	}

	public void OnPlay(){
		DoTransition (inGamePanel, startPanel, 1);
		GameControllerScript.instance.PlayGame ();
		StageControllerScript.instance.TransitionOnPlay ();
	}

	public void OnPlayChallenege(int challengeId){
		if(challengeId == ChallengePanelScript.instance.NextChallengeID){

			if(ChallengePanelScript.instance.CanPlayChallenge()){
				DoTransition (inGamePanel, challengeSelectionPanel, 1);
				GameControllerScript.instance.PlayChallenge (challengeId);
				StageControllerScript.instance.TransitionOnPlay ();
			}else{
				OnGetCoins();
			}
		}
	}

	public void OnGameOver(){
		DoTransition (gameOverPanel, inGamePanel, 1);
		StageControllerScript.instance.TransitionOnGameOver ();
	}

	public void OnResume(){
		DoTransition (inGamePanel, gameOverPanel, 3);
		StageControllerScript.instance.TransitionOnResume ();
		GameControllerScript.instance.IsPlaying = true;
	}

	public void OnRestart(){
		DoTransition (inGamePanel, gameOverPanel, 3);
		StageControllerScript.instance.TransitionOnRestart ();
		GameControllerScript.instance.IsPlaying = true;
	}

	public void OnChallengeSelection(){
		DoTransition (challengeSelectionPanel, startPanel, 3);
	}

	public void OnChallengeSelectionBack(){
		DoTransition (startPanel, challengeSelectionPanel, 1);
	}

	public void OnSettings(){
		DoTransition (settingsPanel, startPanel, 0);
	}

	public void OnSettingsBack(){
		DoTransition (startPanel, settingsPanel, 2);
	}

	public void OnHome(){
		DoTransition (startPanel, gameOverPanel, 3);
		StageControllerScript.instance.TransitionOnHome ();
	}

	public void OnKnifeSelection(){
		DoTransition (knifeSelectionPanel, startPanel, 2);
	}

	public void OnKnifeSelectionBack(){
		DoTransition (startPanel, knifeSelectionPanel, 0);
	}

	public void OnChallengeCompletion(Target target){
		DoTransition (challengeCompletionPanel, inGamePanel, 1);
		ChallengeCompletionPanel.instance.Target = target;
	}

	public void OnClaimReward(){
		DoTransition (challengeSelectionPanel, challengeCompletionPanel, 3);
	}

	public void OnGetCoins(){
		touchBlockerPanel.SetActive(true);
		getCoinsPanel.SetActive(true);
	}

	public void OnGetCoinsBack(){
		touchBlockerPanel.SetActive(true);
		getCoinsPanel.GetComponent<Animator>().SetTrigger("FadeOut");
	}

	public void DisableTouchBlockerPanel(){
		touchBlockerPanel.SetActive(false);
	}

	public void DisableClaimCoinsPanel(){
		getCoinsPanel.GetComponent<Animator>().SetTrigger("FadeOut");
	}

	public void UpdateBestScoreText(int value){
		bestScoreText.text = "BEST "+ value;
		bestScoreTextFront.text = bestScoreText.text;
	}

	public void UpdateBestStageText(int value){
		bestStageText.text = "STAGE "+ value;
		bestStageTextFront.text = bestStageText.text;
	}

	public Vector3 GetIngamePanelCoinImagePosition(){
		return new Vector3(inGamePanelCoinImage.transform.position.x, inGamePanelCoinImage.transform.position.y, 0);
	}

	public void UpdateStartPanelKnifeImage(){
		Knife selectedKnife = GameControllerScript.instance.SelectedKnife;
		startPanelKnifeImage.sprite = startPanelKnifeImageShadow.sprite = selectedKnife.Sprite;
	}

	public void UpdateStageCountText(int count){
		stageCountText.text = stageCountTextFront.text = "stAgE " + count.ToString();
	}

	private void DoTransition(GameObject inMenu, GameObject outMenu, int goDirection){

		touchBlockerPanel.SetActive (true);

		switch(goDirection){
		case 0: // top to bottom
			inMenu.transform.position = topPanelHolder.transform.position;
			discardPosition = bottomPanelHolder.transform.position;
			spawnPosition = topPanelHolder.transform.position;
			break;
		case 1: // right to left
			inMenu.transform.position = rightPanelHolder.transform.position;
			discardPosition = leftPanelHolder.transform.position;
			spawnPosition = rightPanelHolder.transform.position;
			break;
		case 2: // bottom to top
			inMenu.transform.position = bottomPanelHolder.transform.position;
			discardPosition = topPanelHolder.transform.position;
			spawnPosition = bottomPanelHolder.transform.position;
			break;
		case 3: // left to right
			inMenu.transform.position = leftPanelHolder.transform.position;
			discardPosition = rightPanelHolder.transform.position;
			spawnPosition = leftPanelHolder.transform.position;
			break;
		}
		this.outMenu = outMenu;
		this.inMenu = inMenu;
		inMenu.SetActive (true);
		doTransition = true;
		interpolationTime = 0;
	}
}

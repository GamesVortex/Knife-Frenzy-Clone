using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeCompletionPanel : MonoBehaviour {

	public static ChallengeCompletionPanel instance;

	public Image targetImage;
	public Image targetImageShadow;
	public Image knifeImage;
	public Image knifeImageShadow;
	public TextMeshProUGUI targetNameText;
	public TextMeshProUGUI targetNameTextShadow;

	public GameObject challengeRewardPartcileSystemHolder;

	private Target target;
	private bool unlockKnife;

	public Target Target { 
		set { 
			target = value; 
			
			targetImage.sprite = target.sprite;
			targetImageShadow.sprite = targetImage.sprite;

			targetNameText.text = target.name;
			targetNameTextShadow.text = targetNameText.text;

			if(unlockKnife){

			}
		} 
	}

	void Awake(){
		instance = this;
		ChallengePanelScript.instance.UpdateChallengeDetailsAfterCompletion();
		StartCoroutine(WaitBeforeParticles());
	}

	private IEnumerator WaitBeforeParticles(){
		yield return new WaitForSeconds(0.5f);
		challengeRewardPartcileSystemHolder.SetActive(true);
	}
}

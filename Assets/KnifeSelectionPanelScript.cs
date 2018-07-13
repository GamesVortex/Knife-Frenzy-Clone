using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeSelectionPanelScript : MonoBehaviour {

	public GameObject scrollViewContent;
	public GameObject knifeButtonHolderPrefab;

	public Image knifeImage;
	public Image knifeImageShadow;
	public Image knifeImageLocked;
	public GameObject unlockPanelHolder;

	private Knife[] knives;
	private int knifeCount;
	private int selectedKnifeId;

	private GameObject lastSelectedKnifeButtonHolder;

	void Awake(){
		knives = GameControllerScript.instance.Knives;
		
		PlayerPrefs.SetInt("Knife0", 1);
		PlayerPrefs.SetInt("Knife1", 1);
		PlayerPrefs.SetInt("Knife2", 1);
		PlayerPrefs.SetInt("Knife3", 1);
		PlayerPrefs.SetInt("Knife4", 1);
		for (int i = 5; i<= knives.Length; i++){
			PlayerPrefs.SetInt("Knife"+i, 0);		
		}
	}

	void Start () {
		selectedKnifeId = PlayerPrefs.GetInt("SelectedKnife");

		knifeCount = knives.Length;
		
		for (int i = 0; i < knifeCount; i++) {
			GameObject knifeButtonHolder = Instantiate (knifeButtonHolderPrefab);
			knifeButtonHolder.transform.SetParent (scrollViewContent.transform, false);
			knifeButtonHolder.GetComponent<KnifeButtonHolderScript> ().KnifeId = knives[i].Id;

			if (PlayerPrefs.GetInt("Knife"+knives[i].Id) == 1){
				knifeButtonHolder.transform.GetChild(0).GetComponent<Image>().sprite = knives[i].Sprite;
			} else {
				knifeButtonHolder.transform.GetChild(0).GetComponent<Image>().sprite = knives[i].ShineSprite;	
				knifeButtonHolder.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);							
			}

			knifeButtonHolder.GetComponent<Button> ().onClick.AddListener (() => OnSelectKnife (knifeButtonHolder.GetComponent<KnifeButtonHolderScript>().KnifeId));
			knifeButtonHolder.GetComponent<Button> ().onClick.AddListener (() => AudioManagerScript.instance.PlayUIButtonAudio());
		}

		OnSelectKnife(selectedKnifeId);
	}

	public void OnSelectKnife(int knifeId){
		if(lastSelectedKnifeButtonHolder != null){
			lastSelectedKnifeButtonHolder.GetComponent<Outline>().enabled = false;
		}

		GameObject selectedKnifeButtonHolder = scrollViewContent.transform.GetChild(knifeId).gameObject;

		selectedKnifeButtonHolder.GetComponent<Outline>().enabled = true;
		lastSelectedKnifeButtonHolder = selectedKnifeButtonHolder;
		
		knifeImageShadow.sprite = knives[knifeId].Sprite;
		
		if (PlayerPrefs.GetInt("Knife" + knifeId) == 1){
			knifeImage.gameObject.SetActive(true);
			knifeImageLocked.gameObject.SetActive(false);
			knifeImage.sprite = knives[knifeId].Sprite; 		
			knifeImage.color = new Color(255, 255, 255, 1);
			unlockPanelHolder.SetActive(false);
			GameControllerScript.instance.SelectedKnifeId = knifeId;
		} else{
			knifeImage.gameObject.SetActive(false);
			knifeImageLocked.gameObject.SetActive(true);
			knifeImageLocked.sprite = knives[knifeId].Sprite;
			unlockPanelHolder.SetActive(true);		
		}
	}
}

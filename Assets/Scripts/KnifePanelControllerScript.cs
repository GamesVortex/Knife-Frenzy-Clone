using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifePanelControllerScript : MonoBehaviour {

	public static KnifePanelControllerScript instance;

	public GameObject knifeUIPrefab;
	private Knife selectedKnife;

	void Awake(){
		instance = this;
	}

	void OnEnable(){
		selectedKnife = GameControllerScript.instance.SelectedKnife;
	}

	public void CreateKnifes(int knifeCount){
		for (int i = 0; i < knifeCount; i++) {
			GameObject knifeUIObject = Instantiate (knifeUIPrefab) as GameObject;
			knifeUIObject.GetComponent<Image>().sprite = selectedKnife.Sprite;
			knifeUIObject.transform.SetParent (gameObject.transform, false);
		}
	}

	public void DarkenKnife(int index){
		gameObject.transform.GetChild (index).gameObject.GetComponent<Image> ().color = Color.black;
	}

	public void ClearUI(){
		int childCount = gameObject.transform.childCount;
		if (childCount > 0) {
			for (int i = childCount - 1; i >= 0; i--) {
				Destroy (gameObject.transform.GetChild (i).gameObject);
			}
		}
	}
}

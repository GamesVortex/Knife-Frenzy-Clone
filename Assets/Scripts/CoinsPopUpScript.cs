using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPopUpScript : MonoBehaviour {

	public void DisablePanel(){
		gameObject.SetActive(false);
		DisableTouchBlockerPanel();
	}

	public void DisableTouchBlockerPanel(){
		UIManagerScript.instance.DisableTouchBlockerPanel();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScalarScript : MonoBehaviour {

	RectTransform canvasRectTransform;

	void Awake(){
		canvasRectTransform = GetComponent<RectTransform>();
		Vector3 sizeDelta = canvasRectTransform.sizeDelta;

		// resize width as per 9/16 aspect ratio = 0.5625
		sizeDelta.x = (Camera.main.aspect * sizeDelta.x)/0.5625f;

		// resize height as per 9/16 aspect ratio = 0.5625
		//sizeDelta.y = (Camera.main.aspect * sizeDelta.y)/0.5625f;
		
		canvasRectTransform.sizeDelta = sizeDelta;
	}
}

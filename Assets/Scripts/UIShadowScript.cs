using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShadowScript : MonoBehaviour {

	public Vector2 offset;
	public RectTransform shadowCaster;

	private RectTransform rectTransform;

	void Start(){
		rectTransform = gameObject.GetComponent<RectTransform> ();
	}

	void LateUpdate () {
		rectTransform.position = new Vector2 (shadowCaster.position.x + offset.x, shadowCaster.position.y + offset.y);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowScript : MonoBehaviour {
	
	public Vector2 offset; 
	public Transform shadowCaster;
	public bool isUIObject;

	private Vector2 finalOffset;

	void LateUpdate () {
		//if (isUIObject) {
		//	finalOffset = Camera.main.WorldToScreenPoint (offset);
		//} else {
			finalOffset = offset;
		//}
		transform.position = new Vector2 (shadowCaster.position.x + finalOffset.x, shadowCaster.position.y + finalOffset.y);
	}
}

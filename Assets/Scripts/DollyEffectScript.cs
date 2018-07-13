using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyEffectScript : MonoBehaviour {

	public float speed;
	public float maxAngle;
	public float startAngle;

	private float randomValue;

	void Start(){
		randomValue = Random.Range (0.0f, 1.0f);
	}

	void Update(){
		transform.rotation = Quaternion.Euler (0, 0, startAngle + (Mathf.Cos ((randomValue + Time.time) * speed) * maxAngle));
	}
}

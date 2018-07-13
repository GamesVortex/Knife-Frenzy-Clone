using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControllerScript : MonoBehaviour {

	void Update () {
		transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 180);	
	}
}

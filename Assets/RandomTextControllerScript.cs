using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTextControllerScript : MonoBehaviour {

	public static RandomTextControllerScript  instance;

	public string[] hitRandomText;
	public GameObject randomTextPrefab;
	public GameObject randomTextHolder;

	public Transform[] randomTransforms;

	void Awake(){
		instance = this;
	}

	public void RandomTextOnHit(){
		int randomNo = Random.Range(0, randomTransforms.Length);
		GameObject randomText = Instantiate(randomTextPrefab, randomTransforms[randomNo].position, randomTransforms[randomNo].rotation);
		randomNo = Random.Range(0, hitRandomText.Length);
		randomText.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = 
		randomText.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = hitRandomText[randomNo];
		randomText.transform.SetParent(randomTextHolder.transform);
	}
}

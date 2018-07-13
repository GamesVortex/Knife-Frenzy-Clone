using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeRewardParticleControllerScript : MonoBehaviour {

	public GameObject[] particleSystems;

	void Start(){
		StartCoroutine(StartParticles());
	}

	IEnumerator StartParticles(){
		for(int i = 0; i < particleSystems.Length; i++){
			particleSystems[i].SetActive(true);
			yield return new WaitForSeconds(0.25f);
		}
	}
}

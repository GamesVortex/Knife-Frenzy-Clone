using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControllerScript : MonoBehaviour {

	public SpriteRenderer targetSprite;
	public SpriteRenderer shadowSprite;
	public SpriteRenderer shineSprite;
	public GameObject particleSystem;

	void OnEnable(){
		UpdateValues ();
	}

	private void UpdateValues(){
		Target target = GameControllerScript.instance.GetRandomTarget ();

		targetSprite.sprite = target.Sprite;
		shadowSprite.sprite = target.Sprite;
		shineSprite.sprite = target.ShineSprite;
		ParticleSystem.MainModule psMain = particleSystem.GetComponent<ParticleSystem>().main;
		psMain.startColor = target.ParticleSystemColor;
	}
}

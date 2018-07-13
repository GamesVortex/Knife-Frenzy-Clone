using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeControllerScript : MonoBehaviour {

	private bool collided;
	private bool inTarget;
	private Rigidbody2D rb;
	private int knifeNo;

	public GameObject knifeFront;
	public GameObject knifeEnd;

	private bool shootKnife;

	public bool ShootKnife { 
		set { 
			shootKnife = value; 
			knifeFront.SetActive (true);
		} 
	}

	public int KnifeNo { set { knifeNo = value; } get { return knifeNo; } }

	void Awake(){
		inTarget = false;
		collided = false;
		shootKnife = false;
		rb = GetComponent<Rigidbody2D> ();
	}	

	void FixedUpdate () {
		if (!collided && shootKnife) {
			shootKnife = false;
			GetComponent<Rigidbody2D> ().AddForce (Vector3.up * 30, ForceMode2D.Impulse);
		}
	}

	public void CollidedWithTarget(){
		RandomTextControllerScript.instance.RandomTextOnHit();
		StageControllerScript.instance.UpdateKnifeCountUI (knifeNo);
		TargetShakeScript.instance.ResetPosition ();
		GameControllerScript.instance.ShineTarget ();
		GameControllerScript.instance.CanShoot = true;
		ScoreControllerScript.instance.AddScore (1);
		AudioManagerScript.instance.PlayTargetHitAudio ();
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;
		rb.freezeRotation = true;
		transform.position = new Vector3 (0, 1.0f, 0);
		knifeFront.SetActive (false);
		knifeEnd.SetActive (true);
		GameControllerScript.instance.MakeChildOfTargetCircle (gameObject);
		TargetShakeScript.instance.DoShake ();
		StageControllerScript.instance.CheckIfLastKnife ();
		transform.rotation = Quaternion.identity;
	}

	public void CollidedWithKnife(GameObject otherKnife){
		if (GameControllerScript.instance.IsPlaying) {
			GameControllerScript.instance.GameOver ();
		}
		otherKnife.GetComponent<Animator> ().SetTrigger ("DoShine");
		gameObject.GetComponent<Animator> ().SetTrigger ("DoShine");
		knifeFront.SetActive (false);
		AudioManagerScript.instance.PlayKnifeClangAudio ();
		rb.velocity = Vector3.zero;
		rb.gravityScale = 10;
		knifeEnd.SetActive (false);
	}

	public void CollidedWithCoin(GameObject coin){
		ScoreControllerScript.instance.AddCoin ();
		AudioManagerScript.instance.PlayCoinCollectAudio();
		Destroy (coin);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (!inTarget && !collided && GameControllerScript.instance.IsPlaying && coll.gameObject.CompareTag ("Knife")) {
			collided = true;
			CollidedWithKnife (coll.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (!collided && other.gameObject.CompareTag ("Coin")) {
			CollidedWithCoin (other.gameObject);
		} else if (!inTarget && !collided && other.gameObject.CompareTag ("Target")) {
			inTarget = true;
			CollidedWithTarget ();
		}
	}

	public void DeactivateKnife(){
		Destroy (gameObject);
	}
}

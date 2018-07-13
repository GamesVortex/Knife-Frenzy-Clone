using UnityEngine; 
using System.Collections;

public class TargetShakeScript : MonoBehaviour { 

	public static TargetShakeScript instance;

	public bool shakePosition;
	public bool shakeRotation;

	public float shakeIntensity = 0.5f; 
	public float shakeDecay = 0.02f;

	private Vector3 OriginalPos;
	private Quaternion OriginalRot;

	private bool isShakeRunning = false;
	private float currentShakeIntensity;

	public float ShakeIntensity {
		set{
			shakeIntensity = value;
		}
		get{
			return shakeIntensity;
		}
	}

	public float ShakeDecay {
		set{
			shakeDecay = value;
		}
		get{
			return shakeDecay;
		}
	}

	void Awake(){
		instance = this;
		OriginalPos = StageControllerScript.instance.centerPosition;
		OriginalRot = transform.rotation;
	}

	public void ResetPosition(){
		transform.position = OriginalPos;
	}

	public void DoShake()
	{
		//OriginalPos = transform.position;
		//OriginalRot = transform.rotation;

		StartCoroutine ("ProcessShake");
	}

	IEnumerator ProcessShake()
	{
		if (!isShakeRunning) {
			isShakeRunning = true;
			currentShakeIntensity = shakeIntensity;

			while (currentShakeIntensity > 0) {
				if (shakePosition) {
					transform.position = OriginalPos + Random.insideUnitSphere * currentShakeIntensity;
				}
				if (shakeRotation) {
					transform.rotation = new Quaternion (OriginalRot.x + Random.Range (-currentShakeIntensity, currentShakeIntensity) * .2f,
						OriginalRot.y + Random.Range (-currentShakeIntensity, currentShakeIntensity) * .2f,
						OriginalRot.z + Random.Range (-currentShakeIntensity, currentShakeIntensity) * .2f,
						OriginalRot.w + Random.Range (-currentShakeIntensity, currentShakeIntensity) * .2f);
				}
				currentShakeIntensity -= shakeDecay;
				yield return null;
			}

			transform.position = OriginalPos;
			isShakeRunning = false;
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdCoinOpenScript : MonoBehaviour {

	public static AdCoinOpenScript instance;

    public GameObject coinParticleSystem;

	public TextMeshProUGUI buttonText;
	public TextMeshProUGUI buttonTextFront;

	public float freeGiftAfterTime;

	private int coinAmount;
	private int finalCoinAmount;

	private ulong lastGiftTime;
	private string tempHolder;
	private ulong diff;
	private ulong m;
	private float secondsLeft;
	private string r;
	private bool justOpened;
	private bool isInteractable;

	void Awake(){
		instance = this;
	}

	void OnEnable(){
		justOpened = false;

		if (IsFreeGiftReady()) {
			isInteractable = true;
		} else {
			isInteractable = false;
		}
	}

	void Update(){
		if (!isInteractable) {
			if (IsFreeGiftReady ()) {
				isInteractable = true;
			} else {
				isInteractable = false;
			}
		}
	}

	public bool IsFreeGiftReady(){
		tempHolder = PlayerPrefs.GetString ("LastAdStarsOpened");
		if (tempHolder != "") {
			isInteractable = true;
			//adStarTimerText.gameObject.SetActive (false);
			lastGiftTime = ulong.Parse (tempHolder);
		} else {
			lastGiftTime = 0;
		}

		diff = ((ulong) DateTime.Now.Ticks - lastGiftTime);
		m = diff / TimeSpan.TicksPerMillisecond;
		secondsLeft = (float)(freeGiftAfterTime - m) / 1000.0f;
		if (secondsLeft < 0) {
			//timerText.text = "CLAIM 100";
			buttonText.text = "gET CoINs";
			buttonTextFront.text = "gET CoINs";
			return true;
		} else {
			r = "";
			//Hours
			secondsLeft -= ((int)secondsLeft / 3600) * 3600;
			//Minutes
			r += ((int) Math.Floor((double)secondsLeft / 60)).ToString ("00");
			//Seconds
			r += ":"+((int) Math.Floor((double)secondsLeft % 60)).ToString ("00");
			isInteractable = false;
			buttonText.text = "wAiT " + r.ToString ();
			buttonTextFront.text = "wAiT " + r.ToString ();
		}

		return false;
	}

	public void OpenGift() {
		if (isInteractable) {
			AdManagerScript.instance.ShowRewardedAd (0);
		} else {
			//PNUnityAndroidPlugin.Instance.ShowToast ("Wait for cooldown!");
		}
	}

	public void IsAdComplete() {
	 
        coinParticleSystem.SetActive(false);
        coinParticleSystem.SetActive(true);
		justOpened = true;
		isInteractable = false;
		lastGiftTime = (ulong)DateTime.Now.Ticks;
		PlayerPrefs.SetString ("LastAdStarsOpened", lastGiftTime.ToString ());
		UIManagerScript.instance.DisableClaimCoinsPanel();
		AudioManagerScript.instance.PlayCoinRewardAudio();
		coinAmount = PlayerPrefs.GetInt ("CoinAmount");
		finalCoinAmount = coinAmount + 100;
		PlayerPrefs.SetInt ("CoinAmount", finalCoinAmount);
        ScoreControllerScript.instance.IncreaseCoinValueTo = finalCoinAmount;
        //ScoreControllerScript.instance.IncreaseCoinCount = true;
	}
}

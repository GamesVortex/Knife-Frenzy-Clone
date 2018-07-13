using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreeCoinOpenScript : MonoBehaviour {

	public static FreeCoinOpenScript instance;

	public GameObject starSound;
    public GameObject coinParticleSystem;

	public TextMeshProUGUI claimFreeStarText;
	public TextMeshProUGUI claimFreeStarTextFront;

	public TextMeshProUGUI timerText;
	public TextMeshProUGUI timerTextFront;

	public TextMeshProUGUI rewardGiftText;
	public TextMeshProUGUI rewardGiftTextFront;

	public GameObject coinImage;

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
			timerText.gameObject.SetActive(false);
			timerTextFront.gameObject.SetActive(false);
			claimFreeStarText.gameObject.SetActive(true);
			claimFreeStarText.gameObject.SetActive(true);
			coinImage.SetActive(true);
		} else {
			isInteractable = false;
			timerText.gameObject.SetActive(true);
			timerTextFront.gameObject.SetActive(true);
			claimFreeStarText.gameObject.SetActive(false);
			claimFreeStarText.gameObject.SetActive(false);
			coinImage.SetActive(false);
		}
	}

	void Update(){
		if (!isInteractable) {
			if (IsFreeGiftReady ()) {
				isInteractable = true;
				timerText.gameObject.SetActive(false);
				timerTextFront.gameObject.SetActive(false);
				claimFreeStarText.gameObject.SetActive(true);
				claimFreeStarTextFront.gameObject.SetActive(true);
				coinImage.SetActive(true);
			} else {
				isInteractable = false;
				timerText.gameObject.SetActive(true);
				timerTextFront.gameObject.SetActive(true);
				claimFreeStarText.gameObject.SetActive(false);
				claimFreeStarTextFront.gameObject.SetActive(false);
				coinImage.SetActive(false);
			}
		}
	}

	public bool IsFreeGiftReady(){
		tempHolder = PlayerPrefs.GetString ("LastFreeStarsOpened");
		if (tempHolder != "") {
			isInteractable = true;
			timerText.gameObject.SetActive(false);
			timerTextFront.gameObject.SetActive(false);
			claimFreeStarText.gameObject.SetActive(true);
			claimFreeStarTextFront.gameObject.SetActive(true);
			coinImage.SetActive(true);
			lastGiftTime = ulong.Parse (tempHolder);
		} else {
			lastGiftTime = 0;
		}

		diff = ((ulong) DateTime.Now.Ticks - lastGiftTime);
		m = diff / TimeSpan.TicksPerMillisecond;
		secondsLeft = (float)(freeGiftAfterTime - m) / 1000.0f;
		if (secondsLeft < 0) {
			//timerText.text = "CLAIM 100";
			rewardGiftText.text = "rEAdY!";
			rewardGiftTextFront.text = "rEAdY!";
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
			timerText.text = "wAIt " + r.ToString ();
			timerTextFront.text = "wAIt " + r.ToString ();
			rewardGiftText.text = r.ToString();
			rewardGiftTextFront.text = r.ToString();
		}

		return false;
	}

	public void OpenGift() {
		if (isInteractable) {
			ClaimCoins();
		} else {
			//PNUnityAndroidPlugin.Instance.ShowToast ("Wait for cooldown!");
		}
	}

	public void ClaimCoins() {
	 
        coinParticleSystem.SetActive(false);
        coinParticleSystem.SetActive(true);
		justOpened = true;
		isInteractable = false;
		lastGiftTime = (ulong)DateTime.Now.Ticks;
		PlayerPrefs.SetString ("LastFreeStarsOpened", lastGiftTime.ToString ());
		UIManagerScript.instance.DisableClaimCoinsPanel();
		AudioManagerScript.instance.PlayCoinRewardAudio();
		coinAmount = PlayerPrefs.GetInt ("CoinAmount");
		finalCoinAmount = coinAmount + 100;
		PlayerPrefs.SetInt ("CoinAmount", finalCoinAmount);
        ScoreControllerScript.instance.IncreaseCoinValueTo = finalCoinAmount;
        //ScoreControllerScript.instance.IncreaseCoinCount = true;
	}
}

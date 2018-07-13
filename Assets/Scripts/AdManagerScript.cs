using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AudienceNetwork;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdManagerScript : MonoBehaviour {

	public static AdManagerScript instance;

//	private InterstitialAd interstitialAd;
	private bool isLoaded;

	private string interstitialAdId = "911955518951655_911956018951605";

	//private AdView adView;

	public bool IsLoaded {
		set{
			isLoaded = value;
		}
		get{
			return isLoaded;
		}
	}
		
	private bool adFinished;
	private bool showOnce;
	private int indicator;

	public bool AdFinished {
		set{
			adFinished = value;
		}
		get{
			return adFinished;
		}
	}

	void Awake ()
	{
		instance = this;
		DontDestroyOnLoad (instance);
	}
		
	// Load button
	public void LoadInterstitial ()
	{
		/* 
		//Debug.Log("Loading interstitial ad...");

		//PNUnityAndroidPlugin.Instance.ShowToast ("Loading interstitial ad...");
		// Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
		// Use different ID for each ad placement in your app.
		InterstitialAd interstitialAd = new InterstitialAd (interstitialAdId);
		this.interstitialAd = interstitialAd;
		this.interstitialAd.Register (this.gameObject);

		// Set delegates to get notified on changes or when the user interacts with the ad.
		this.interstitialAd.InterstitialAdDidLoad = (delegate() {
			//Debug.Log ("Interstitial ad loaded.");
			this.isLoaded = true;
			//Debug.Log("Ad loaded. Click show to present!");
		});
		interstitialAd.InterstitialAdDidFailWithError = (delegate(string error) {
			//Debug.Log ("Interstitial ad failed to load with error: " + error);
			//Debug.Log("Interstitial ad failed to load. Check console for details.");
		});
		interstitialAd.InterstitialAdWillLogImpression = (delegate() {
			//Debug.Log ("Interstitial ad logged impression.");
		});
		interstitialAd.InterstitialAdDidClick = (delegate() {
			//Debug.Log ("Interstitial ad clicked.");
		});
			
		// Initiate the request to load the ad.
		this.interstitialAd.LoadAd ();
		*/
	}

	// Show button
	public void ShowInterstitial ()
	{
		/* 
		if (this.isLoaded) {
			this.interstitialAd.Show ();
			this.isLoaded = false;
			//Debug.Log("");
		} else {
			//Debug.Log("Ad not loaded. Click load to request an ad.");
		}
		*/
	}

	public void ShowRewardedAd(int indicator)
	{
		this.indicator = indicator;
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		} else {
			//PNUnityAndroidPlugin.Instance.ShowToast ("Ad not ready");
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			//Debug.Log ("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			adFinished = true;
			if (indicator == 0) { // for stars
				AdCoinOpenScript.instance.IsAdComplete ();
			} else { // for respawn
				//CanvasManagerScript.instance.IsAdComplete ();
			}
			//PlayerPrefs.SetInt ("LastScore", ScoreControllerScript.instance.Score);
			//GameOverPanelManagerScript.instance.OnHome ();
			break;
		case ShowResult.Skipped:
			adFinished = false;
			//Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			adFinished = false;
			//PNUnityAndroidPlugin.Instance.ShowToast ("The ad failed to be shown.");
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}

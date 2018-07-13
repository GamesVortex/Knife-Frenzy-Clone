using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

	public static AudioManagerScript instance;

	public GameObject knifeClangAudioSourceHolder;
	public GameObject targetHitAudioSourceHolder;
	public GameObject coinCollectAudioSourceHolder;
	public GameObject coinRewardAudioSourceHolder;
	public GameObject coinParticlesCollectAudioSourceHolder;
	public GameObject uiButtonAudioSourceHolder;
	public GameObject knifeThrowAudioSourceHolder;

	private AudioSource knifeClangAudioSource;
	private AudioSource targetHitAudioSource;
	private AudioSource coinCollectAudioSource;
	private AudioSource coinRewardAudioSource;
	private AudioSource coinParticlesCollectAudioSource;
	private AudioSource uiButtonAudioSource;
	private AudioSource knifeThrowAudioSource;

	void Awake(){
		instance = this;
		knifeClangAudioSource = knifeClangAudioSourceHolder.GetComponent<AudioSource> ();
		targetHitAudioSource = targetHitAudioSourceHolder.GetComponent<AudioSource> ();
		coinCollectAudioSource = coinCollectAudioSourceHolder.GetComponent<AudioSource> ();
		coinRewardAudioSource = coinRewardAudioSourceHolder.GetComponent<AudioSource> ();
		coinParticlesCollectAudioSource = coinParticlesCollectAudioSourceHolder.GetComponent<AudioSource>();
		uiButtonAudioSource = uiButtonAudioSourceHolder.GetComponent<AudioSource>();
		knifeThrowAudioSource = knifeThrowAudioSourceHolder.GetComponent<AudioSource>();
	}

	public void PlayKnifeClangAudio(){
		knifeClangAudioSource.Play ();
	}

	public void PlayTargetHitAudio(){
		targetHitAudioSource.Play ();
	}

	public void PlayCoinCollectAudio(){
		coinCollectAudioSource.Play ();
	}

	public void PlayCoinRewardAudio(){
		coinRewardAudioSource.Play ();
	}

	public void PlayCoinParticlesCollectAudio(){
		coinParticlesCollectAudioSource.Play();
	}

	public void StopCoinParticlesCollectAudio(){
		coinParticlesCollectAudioSource.Stop();
	}

	public void PlayUIButtonAudio(){
		uiButtonAudioSource.Play();
	}

	public void PlayKnifeThrowAudio(){
		knifeThrowAudioSource.Play();
	}
}

﻿using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;
	[Range(.01f, .5f)] [SerializeField] private float fadeInTime = .05f;

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioSource MusicAudioSource;
	[SerializeField] private AudioSource SFXAudioSource;
	[SerializeField] private AudioSource ambientAudioSource;
	[SerializeField] private AudioSource environmentAmbient;	
	[Space]
	[SerializeField] private AudioClip[] music;
	[SerializeField] private AudioClip[] ambientClips;
	[SerializeField] private AudioClip[] movementClips;
	[Space]
	[SerializeField] private AudioClip collectClip;
	[SerializeField] private AudioClip buttonClick;
	[SerializeField] private AudioClip bubbleClip;

	private float audioVolume = 1f;
	private int clipIndex = 0;

	void Awake(){
		if (instance != null){
			Destroy(gameObject);
		} else{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("master_volume")) {
			ChangeMasterVolume(PlayerPrefsManager.GetMasterVolume());
		}

		if (PlayerPrefs.HasKey("music_volume")) {
			ChangeMusicVolume(PlayerPrefsManager.GetMusicVolume());
		}

		if (PlayerPrefs.HasKey("sfx_volume")) {
			ChangeSFXVolume(PlayerPrefsManager.GetSFXVolume());
		}
	}

	private void Update(){
		if (ambientAudioSource.isPlaying) {

		}
		PlayRandomAmbient();
		MusicSelect();
		VolumeFadeIn(MusicAudioSource);   
	}

	void VolumeFadeIn(AudioSource audioSource) {
		if (audioVolume <= 1f){
			audioVolume += fadeInTime * Time.deltaTime;
			audioSource.volume = audioVolume;
		} else{
			audioVolume = 1f;
		}

		if (audioSource.clip != null){
			if (!audioSource.isPlaying){
				audioSource.Play();
				audioSource.volume = 0f;
				audioVolume = 0f;
			}
		}
	}

	void VolumeFadeOut(AudioSource audioSource) {
		if (audioVolume >= 1f){
			audioVolume -= fadeInTime * Time.deltaTime;
			audioSource.volume = audioVolume;
		} else{
			audioVolume = 0f;
		}

		if (audioSource.volume <= 0f){
			audioSource.Stop();
		}
	}

	public void MusicSelect()
	{
		switch (LevelManager.Instance.currentScene) {
			case "Main Menu":
				MusicAudioSource.clip = music[0];
				break;

			case "Options":
				MusicAudioSource.clip = music[0];
				break;

			case "Game Level 1":
				MusicAudioSource.clip = music[1];
				environmentAmbient.Play();
				break;

			default:
				break;
		}
	}


	void PlayRandomAmbient()
	{
		if (!ambientAudioSource.isPlaying) {
			clipIndex = Random.Range(0, ambientClips.Length);
			ambientAudioSource.PlayOneShot(ambientClips[clipIndex]);
		}
	}

	public void StartAudio(){
		MusicAudioSource.Play();
	}

	public void SetButtonClip(){
		SFXAudioSource.PlayOneShot(buttonClick, .6f);
	}

	public void PlayBoostClip() {
		SFXAudioSource.PlayOneShot(movementClips[1], .2f);
	}

	public void PlayDropClip()
	{
		SFXAudioSource.PlayOneShot(movementClips[2], .2f);
	}

	public void PlayCollectClip() {
		SFXAudioSource.PlayOneShot(collectClip, .3f);
	}

	public void BubbleImpactClip() {
		SFXAudioSource.PlayOneShot(bubbleClip, .3f);
	}

	public void ChangeMasterVolume(float volume) {
		audioMixer.SetFloat("Master", volume);
		if (volume == -40f){
			audioMixer.SetFloat("Master", -80f);
		}
	}

	public void ChangeMusicVolume(float volume){
		audioMixer.SetFloat("Music", volume);
		if (volume == -40f){
			audioMixer.SetFloat("Music", -80f);
		}
	}

	public void ChangeSFXVolume(float volume){
		audioMixer.SetFloat("SFX", volume);
		if (volume == -40f){
			audioMixer.SetFloat("SFX", -80f);
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
	In charge of handling the functions that change the background music and random sounds
*/


public static class MusicManager {

	public static AudioSource myPlayer;

	/*
		Use the camera one
	*/
	public static void RegisterAudioSource() {
		myPlayer = Camera.main.GetComponent<AudioSource>();
	}
	

	public static void PlaySong(AudioClip music) {
	
		if (myPlayer != null) {
			if (myPlayer.isPlaying) {
				//myPlayer.Stop(); //TODO: Fade out
				//myPlayer.SetScheduledEndTime(Time.time + 3f);
			}
			
			
		}
	}


}

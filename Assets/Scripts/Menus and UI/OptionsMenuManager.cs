using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenuManager : MenuManager {

	public GameObject resolutions;
	public Toggle defaultToggleResolutionWindowed;

	void Awake(){

	}

	//We set fixed resolutions to keep the aspect ratio when in fullscreen
	public void setFullScreen(bool fullScreen){
		if(fullScreen){
			defaultToggleResolutionWindowed.isOn = true;
			resolutions.SetActive(false);
		}else{
			resolutions.SetActive(true);
		}
		GameManager.optionsManager.setFullScreen (fullScreen);
	}

	public void OnResolutionChanged(int width,int height){
		GameManager.optionsManager.OnResolutionChanged (width, height);
	}
	
	public void OnQualityChanged(string newQuality){
		GameManager.optionsManager.OnQualityChanged (newQuality);
	}
	
	public void OnMusicVolumeSliderChange(float newVolume){
		Debug.Log ("Music Volume Changed To: " + newVolume);
	}
	
	public void OnFXVolumeSliderChange(float newVolume){
		Debug.Log ("FX Volume Changed To: " + newVolume);
	}

}

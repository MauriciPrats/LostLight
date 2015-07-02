using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroVideoManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		RawImage ri = this.GetComponent<RawImage>();
		MovieTexture mt = ri.mainTexture as MovieTexture;
		mt.Play();
		GameManager.audioManager.PlaySong( mt.audioClip);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.anyKeyDown) {
			GUIManager.fadeOutChangeMenuFadeIn(Menu.MainMenu);
			GameManager.audioManager.playSong(0);
		}
		
	}
}

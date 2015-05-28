using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public bool onAnyButtonChangeToMainMenu = false;

	public GameObject firstButtonFocus;

	public void ChangeMenu(Menu newMenu){
		GUIManager.activateMenu (newMenu);
		//GameManager.actualSceneManager.ChangeScene (newMenu);
	}

	public void GoToControlsMenu(){
		GameManager.audioManager.PlayStableSound(0);
		GUIManager.fadeOutChangeMenuFadeIn (Menu.ControlsMenu);
	}
	
	public void GoToCreditsMenu(){
		//GUIManager.fadeOutChangeMenuFadeIn (Menu.CreditsMenu);
		GameManager.audioManager.PlayStableSound(0);
		GUIManager.fadeOutChangeMenuFadeIn (Menu.CreditsMenu);
		
	}
	
	public void StopGame(){
		GameManager.audioManager.PlayStableSound(0);
		GameManager.actualSceneManager.CloseApplication ();
	}
	
	public void StartGame(){
		GameManager.startGame ();
		GUIManager.fadeOutChangeMenuFadeIn (Menu.None);
		//TODO: should know what part of the level is loading. 
		GameManager.audioManager.PlayStableSound(0);
		
		GameManager.audioManager.playSong(1);
		
		
	}

	public void GoToMainMenu(){
		GUIManager.fadeOutChangeMenuFadeIn (Menu.MainMenu);
	}

	public void GoToScreen(string scene){
		GameManager.actualSceneManager.ChangeScene (scene);
	}

	void Update(){
		if(onAnyButtonChangeToMainMenu){
			if(Input.anyKey){

			}
		}
	}
}

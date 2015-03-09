using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public bool onAnyButtonChangeToMainMenu = false;

	public void ChangeMenu(Menu newMenu){
		GUIManager.activateMenu (newMenu);
		//GameManager.actualSceneManager.ChangeScene (newMenu);
	}

	public void GoToControlsMenu(){
		GUIManager.fadeOutChangeMenuFadeIn (Menu.ControlsMenu);
	}
	
	public void GoToCreditsMenu(){
		//GUIManager.fadeOutChangeMenuFadeIn (Menu.CreditsMenu);
		GUIManager.fadeOutChangeMenuFadeIn (Menu.CreditsMenu);
	}
	
	public void StopGame(){
		GameManager.actualSceneManager.CloseApplication ();
	}
	
	public void StartGame(){
		GameManager.startGame ();
		GUIManager.fadeOutChangeMenuFadeIn (Menu.None);
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

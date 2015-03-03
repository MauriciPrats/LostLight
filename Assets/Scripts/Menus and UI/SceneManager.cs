using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public Menu startingMenu = Menu.None;
	string sceneToChangeTo;
	public GameObject mainMenuPrefab,
					  controlsMenuPrefab,
					  creditsMenuPrefab,
					  youWonMenuPrefab,
					  youLostMenuPrefab,
					  startingSplashScreenPrefab;
	bool hasUpdated = false;

	void Awake(){
		GameManager.registerActualSceneManager (gameObject);
		GUIManager.registerMainMenu (mainMenuPrefab);
		GUIManager.registerControlsMenu(controlsMenuPrefab);
		GUIManager.registerCreditsMenu (creditsMenuPrefab);
		GUIManager.registerYouLostMenu (youLostMenuPrefab);
		GUIManager.registerYouWonMenu (youWonMenuPrefab);
		GUIManager.registerStartingSplashScreen (startingSplashScreenPrefab);
	}

	void Update () {
		if(!hasUpdated){
			if(startingMenu == Menu.MainMenu){
				GUIManager.fadeOutChangeMenuFadeIn(startingMenu);
				GameManager.rebuildGameFromGameState ();
				GameManager.pauseGame();
			}else{
				GUIManager.fadeIn (startingMenu);
			}
			hasUpdated = true;
		}
	}

	private void QuitScene(){
		Application.Quit();
	}

	private void ChangeScene(){
		Application.LoadLevel(sceneToChangeTo);
	}

	public void ChangeScene(string sceneToChange){
		sceneToChangeTo = sceneToChange;
		GUIManager.fadeOut (ChangeScene);
	}

	public void CloseApplication(){
		GUIManager.fadeOut (QuitScene);
	}

}

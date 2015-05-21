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
					  startingSplashScreenPrefab,
					  craftingMenuPrefab,
					  interactuablePopupPrefab,
					  onPauseMenuPrefab,
					  playingGUIPrefab,
					  spaceJumpGUIPrefab;

	void Awake(){
		GameManager.registerActualSceneManager (gameObject);
		GUIManager.registerMainMenu (mainMenuPrefab);
		GUIManager.registerControlsMenu(controlsMenuPrefab);
		GUIManager.registerCreditsMenu (creditsMenuPrefab);
		GUIManager.registerYouLostMenu (youLostMenuPrefab);
		GUIManager.registerYouWonMenu (youWonMenuPrefab);
		GUIManager.registerStartingSplashScreen (startingSplashScreenPrefab);
		GUIManager.registerCraftingMenu (craftingMenuPrefab);
		GUIManager.registerInteractuablePopup (interactuablePopupPrefab);
		GUIManager.registerOnPauseMenu (onPauseMenuPrefab);
		GUIManager.registerPlayingGUI (playingGUIPrefab);
		GUIManager.registerSpaceJumpGUI (spaceJumpGUIPrefab);

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dashing"),LayerMask.NameToLayer("Enemy"),true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dashing"),LayerMask.NameToLayer("Dashing"),true);

		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("OnlyFloor"),LayerMask.NameToLayer("Enemy"),true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("OnlyFloor"),LayerMask.NameToLayer("Dashing"),true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("OnlyFloor"),LayerMask.NameToLayer("Player"),true);
	}

	void Update () {

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
		GUIManager.fadeAllOut(QuitScene);
	}

}

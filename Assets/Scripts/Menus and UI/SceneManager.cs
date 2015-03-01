using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public Menu startingMenu = Menu.None;
	string sceneToChangeTo;
	public GameObject mainMenuPrefab,controlsMenuPrefab;

	void Awake(){
		GameManager.registerActualSceneManager (gameObject);
		GUIManager.registerMainMenu (mainMenuPrefab);
		GUIManager.registerControlsMenu(controlsMenuPrefab);
	}
	void Start () {
		GUIManager.fadeManager.fadeIn ();
		GUIManager.activateMenu (startingMenu);
	}

	private void QuitScene(){
		Application.Quit();
	}

	private void ChangeScene(){
		Application.LoadLevel(sceneToChangeTo);
	}


	public void ChangeScene(string sceneToChange){
		sceneToChangeTo = sceneToChange;
		GUIManager.fadeManager.fadeOut (ChangeScene);
	}

	public void CloseApplication(){
		//Application.Quit();
		GUIManager.fadeManager.fadeOut (QuitScene);
	}

}

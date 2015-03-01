using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void Awake(){
		//GUIManager.registerMainMenu (gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeMenu(Menu newMenu){
		GUIManager.activateMenu (newMenu);
		//GameManager.actualSceneManager.ChangeScene (newMenu);
	}

	public void GoToControlsMenu(){
		GUIManager.fadeOutChangeMenuFadeIn (Menu.ControlsMenu);
	}

	public void StopGame(){
		GameManager.actualSceneManager.CloseApplication ();
	}

	public void StartGame(){
		//GUIManager.activateMenu (Menu.None);
		GameManager.actualSceneManager.ChangeScene("Game");
	}



}

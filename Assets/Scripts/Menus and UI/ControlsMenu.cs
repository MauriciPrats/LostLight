using UnityEngine;
using System.Collections;

public class ControlsMenu : MonoBehaviour {

	void Awake(){
		//GUIManager.registerControlsMenu (gameObject);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void GoToMainMenu(){
		GUIManager.fadeOutChangeMenuFadeIn (Menu.MainMenu);
	}
	
	public void StopGame(){
		GameManager.actualSceneManager.CloseApplication ();
	}
	
	public void StartGame(){
		GUIManager.activateMenu (Menu.None);
		GameManager.actualSceneManager.ChangeScene("Game");
	}

}

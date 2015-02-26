using UnityEngine;
using System.Collections;

public class ChangeSceneOnClick : MonoBehaviour {

	private string sceneToChangeTo;

	private void ChangeScene(){
		Application.LoadLevel(sceneToChangeTo);
	}

	public void ChangeToScene(string sceneToChange){
		sceneToChangeTo = sceneToChange;
		GameManager.fadeManager.fadeOut (ChangeScene);
		//Application.LoadLevel(sceneToChangeTo);
	}

	public void CloseApplication(){
		//Application.Quit();
		GameManager.fadeManager.fadeOut (QuitScene);
	}

	private void QuitScene(){
		Application.Quit();
	}
	
}

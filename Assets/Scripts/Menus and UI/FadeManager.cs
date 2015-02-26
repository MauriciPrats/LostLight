using UnityEngine;
using System;
using System.Collections;

public class FadeManager : MonoBehaviour {


	public Texture2D fadeTexture;
	
	int drawDepth = -1000;
	
	float alpha = 0f; 
	int fadeDir = -1;

	bool isFinished = true;

	private Action functionAfter;

	void Awake () {
		GameManager.registerFadeManager (gameObject);
	}

	public void fadeIn(){
		fadeDir = -1;
		alpha = 1f;
		isFinished = false;
	}

	public void fadeOut(Action f){
		fadeDir = 1;
		alpha = 0f;
		functionAfter = f;
		isFinished = false;
	}
	
	void OnGUI(){

		alpha += fadeDir * Constants.FADE_SPEED * Time.deltaTime;  
		alpha = Mathf.Clamp01 (alpha);   
	
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
	
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);

			if (!isFinished) {
				if (fadeDir == -1 && alpha == 0f) {
					isFinished = true;
				} else if (fadeDir == 1 && alpha == 1f) {
					isFinished = true;
					functionAfter ();
				}
			}
		}
}

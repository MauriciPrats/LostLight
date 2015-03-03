using UnityEngine;
using System;
using System.Collections;

public class FadeManager : MonoBehaviour {

	public Texture2D blackTexture;
	public Texture2D getHurtTexture;

	public float fadeProgression = 0.05f;
	public float getHurtFadeProgression = 0.05f;
	public float getHurtStartAlpha = 0.8f;
	public int drawDepth = -1000;


	float alpha = 0f; 
	int fadeDir = 1;
	bool isFinished = true;

	float getHurtAlpha = 0f;
	int fadeDirGetHurt = 1;
	bool isFinishedGetHurt = true;

	GameObject fadingMenu;


	private Action fadeInAction,fadeOutAction;

	void Awake () {
		GUIManager.registerFadeManager (gameObject);
	}

	public void getHurtEffect(){
		getHurtAlpha = getHurtStartAlpha;
		isFinishedGetHurt = false;
		fadeDirGetHurt = -1;
	}



	public void fadeIn(GameObject menu){
		fadeInWithAction (null, menu);
	}

	public void fadeInWithAction(Action fadeAction,GameObject menu){
		if(menu!=null){
			fadeDir = 1;
			alpha = 0f;
			isFinished = false;
			fadingMenu = menu;
			menu.GetComponent<CanvasGroup> ().alpha = 0f;
			fadeInAction = fadeAction;
		}
	}

	public void fadeOut(Action fadeAction,GameObject menu){
		fadeDir = -1;
		alpha = 1f;
		fadeOutAction = fadeAction;
		isFinished = false;
		fadingMenu = menu;
		if(menu!=null){
			menu.GetComponent<CanvasGroup> ().alpha = 1f;
		}
	}
	
	void OnGUI(){
		drawFadeTexture ();
		drawGetHurtTexture ();
	}

	void drawFadeTexture(){
		if (!isFinished) {

			alpha += fadeDir * Constants.FADE_SPEED * fadeProgression;
			alpha = Mathf.Clamp01 (alpha); 
			if(fadingMenu!=null){
				fadingMenu.GetComponent<CanvasGroup>().alpha = alpha;
			}else{
				GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
				GUI.depth = drawDepth;
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), blackTexture);
			}

			if (fadeDir == -1 && alpha == 0f) {
				isFinished = true;
				if(fadeOutAction!=null){
					fadeOutAction ();
				}
			} else if (fadeDir == 1 && alpha == 1f) {
				isFinished = true;
				if(fadeInAction!=null){
					fadeInAction();
				}
			}
		
		}
	}

	void drawGetHurtTexture(){
		if (!isFinishedGetHurt) {
			getHurtAlpha += fadeDirGetHurt * Constants.FADE_SPEED * getHurtFadeProgression;
			getHurtAlpha = Mathf.Clamp01 (getHurtAlpha); 
		  
		
			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, getHurtAlpha);
		
			GUI.depth = drawDepth;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), getHurtTexture);
		
			if (fadeDirGetHurt == -1 && getHurtAlpha == 0f) {
				isFinishedGetHurt = true;
			} else if (fadeDirGetHurt == 1 && getHurtAlpha == 1f) {
				isFinishedGetHurt = true;
			}
		}
	}
}

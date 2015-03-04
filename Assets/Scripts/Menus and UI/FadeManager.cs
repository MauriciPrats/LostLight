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

	float allAlpha = 0f;
	int fadeDirAll = 1;
	bool isFinishedAll = true;

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

	public void fadeAllOut(Action fadeAction){
		fadeDirAll = -1;
		allAlpha = 1f;
		fadeOutAction = fadeAction;
		isFinishedAll = false;
	}
	public void fadeAllIn(){
		fadeAllInWithAction (null);
	}
	
	public void fadeAllInWithAction(Action fadeAction){

			fadeDirAll = 1;
			allAlpha = 0f;
			isFinishedAll = false;
			fadeInAction = fadeAction;
	}
	
	void OnGUI(){
		drawFadeTexture ();
		drawGetHurtTexture ();
		drawFadeOutCompleteTexture ();
	}

	void drawFadeTexture(){
		if (!isFinished) {

			alpha += fadeDir * Constants.FADE_SPEED * Time.deltaTime;
			alpha = Mathf.Clamp01 (alpha); 
			if(fadingMenu!=null){
				fadingMenu.GetComponent<CanvasGroup>().alpha = alpha;
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

	void drawFadeOutCompleteTexture(){
		if (!isFinishedAll) {
			allAlpha += fadeDirAll * Constants.FADE_SPEED * Time.deltaTime;
			allAlpha = Mathf.Clamp01 (allAlpha); 
			
			
			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, 1f-allAlpha);
			
			GUI.depth = drawDepth;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), blackTexture);
			
			if (fadeDirAll == -1 && allAlpha == 0f) {
				isFinishedAll = true;
			} else if (fadeDirAll == 1 && allAlpha == 1f) {
				isFinishedAll = true;
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

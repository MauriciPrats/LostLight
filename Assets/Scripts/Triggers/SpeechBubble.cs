using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

	
	private float timeItLasts = 1f;
	private GameObject gameObjectToFollow;
	
	private Text textO;
	bool isActive = false;
	float timer = 0f;
	Vector3 offsetFromObject;
	private bool fadeOut;
	
	
	public virtual void initialize(string text,GameObject goToFollow,float timeItLasts,bool bouncingIn,bool fadeOut){
		this.fadeOut = fadeOut;
		textO = GetComponentInChildren<Text> ();
		textO.text = text;
		gameObjectToFollow = goToFollow;
		this.timeItLasts = timeItLasts;
		isActive = true;
		transform.parent = gameObjectToFollow.transform; 
		offsetFromObject = transform.localPosition;
		transform.parent = null;
		timer = 0f;

		if(bouncingIn){
			StartCoroutine ("beat");
		}
	}
	
	protected IEnumerator beat(){
		
		float timer = 0f;
		float beatTime = 0.1f;
		float extraScale = 0.1f;
		Vector3 extraScaleV = new Vector3 (extraScale, extraScale, extraScale);
		Vector3 originalScale = transform.localScale;
		
		
		while(timer<beatTime){
			timer+=Time.deltaTime;
			float ratio = timer/beatTime;
			transform.localScale = originalScale + (extraScaleV * ratio);
			yield return null;
		}
		
		timer = 0f;
		while(timer<beatTime){
			timer+=Time.deltaTime;
			float ratio = 1f-(timer/beatTime);
			transform.localScale = originalScale + (extraScaleV * ratio);
			yield return null;
		}
		
	}
	
	void Update(){
		if(isActive){
			timer+=Time.deltaTime;
			if(timer>timeItLasts){
				GameManager.dialogueManager.dialogueFinished(gameObject);
				deactivate();
			}else{
				transform.parent = gameObjectToFollow.transform;
				transform.localPosition = offsetFromObject;
				transform.parent = null;
				transform.up = gameObjectToFollow.transform.up;
			}
		}
	}

	public void deactivate(){
		isActive = false;
	}

}

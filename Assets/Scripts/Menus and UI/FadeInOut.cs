using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

	public bool fixedTime = false;
	float alpha = 0f;
	int dir = 1;

	CanvasGroup group;

	// Use this for initialization
	void Start () {
		group = GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.deltaTime;
		if (fixedTime) {
			time = Time.fixedDeltaTime;
		}
		alpha += dir * Constants.FADE_SPEED * time * 2f;
		alpha = Mathf.Clamp01 (alpha); 
		if(alpha >= 1f || alpha <= 0f){
			dir*=-1;
		}

		group.alpha = alpha;

	}
}

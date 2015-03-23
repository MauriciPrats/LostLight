using UnityEngine;
using System.Collections;

public class LightOnDeath : MonoBehaviour {
	
	Vector3 speed;
	float totalspeed = 2.5f;

	float timeHasBeenAlive;

	bool hasArrived = false;
	float timerWhenArrived;

	public void setVectorUp(Vector3 vectorUp){
		speed = new Vector3 (Random.value, Random.value, Random.value).normalized;
		speed = ((speed * 0.8f) + vectorUp).normalized;
	}
	// Use this for initialization
	void Start () {
		timeHasBeenAlive = 0f;

	}
	
	// Update is called once per frame
	void Update () {
		timeHasBeenAlive += Time.deltaTime;
		if(Vector3.Distance(GameManager.lightGemObject.transform.position,transform.position)<0.1f){
			transform.position = GameManager.lightGemObject.transform.position;
			if(!hasArrived){
				hasArrived = true;
				timerWhenArrived = 0f;
				transform.parent = GameManager.lightGemObject.transform;
			}
			timerWhenArrived+=Time.deltaTime;
			float ratio = 1f + (timerWhenArrived*1f);
			transform.localScale = new Vector3(ratio,ratio,ratio);
			if(timerWhenArrived>=0.5f){
				Destroy(gameObject);
			}

		}else{
			totalspeed = 1.5f + (timeHasBeenAlive * 0.8f);
			transform.position = transform.position+(speed * Time.deltaTime * totalspeed);
			Vector3 objectiveDirection = GameManager.lightGemObject.transform.position -  transform.position;
			int angleRotation =(int) (timeHasBeenAlive/0.25f);
			speed = Vector3.RotateTowards(speed,objectiveDirection,Mathf.Deg2Rad * angleRotation,1f).normalized;
			//transform.position = Vector3.Lerp(transform.position,GameManager.lightGemObject.transform.position,0.05f);
		}
	}
}

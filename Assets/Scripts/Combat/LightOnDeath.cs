﻿using UnityEngine;
using System.Collections;

public class LightOnDeath : MonoBehaviour {

	public int pointsToAddPerLight = 0;
	Vector3 speed;
	float totalSpeedStart = 2.5f;
	float totalspeed = 2.5f;
	float speedIncrease = 4f;
	float angleMultiplyier = 20f;

	float timeHasBeenAlive;

	bool hasArrived = false;
	float timerWhenArrived;

	//GameObject objective;

	public void setVectorUp(Vector3 vectorUp){
		speed = new Vector3 (Random.value, Random.value, Random.value).normalized;
		speed = ((speed * speedIncrease) + vectorUp).normalized;
	}
	// Use this for initialization
	void Start () {
		timeHasBeenAlive = 0f;
		//objective = GameManager.lightGemObject;
	}

	private Vector3 getObjectivePosition(){
		return GUIManager.getCorruptionBarPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		timeHasBeenAlive += Time.deltaTime;
		if(Vector3.Distance(getObjectivePosition(),transform.position)<0.1f || hasArrived){
			transform.position = getObjectivePosition();
			if(!hasArrived){
				hasArrived = true;
				timerWhenArrived = 0f;
				//transform.parent = objective.transform;
				//GameManager.lightGemSoulsManager.addPoints(pointsToAddPerLight);
				if(GameManager.actualPlanetSpawnerManager!=null){
					GameManager.actualPlanetSpawnerManager.incrementAccumulatedPoints(pointsToAddPerLight);
				}
			}
			timerWhenArrived+=Time.deltaTime;
			float ratio = 1f + (timerWhenArrived*1f);
			//transform.localScale = new Vector3(ratio,ratio,ratio);
			if(timerWhenArrived>=0.1f){
				Destroy(gameObject);
			}

		}else{
			totalspeed = totalSpeedStart + (timeHasBeenAlive * speedIncrease);
			transform.position = transform.position+(speed * Time.deltaTime * totalspeed);
			Vector3 objectiveDirection = getObjectivePosition() -  transform.position;
			int angleRotation =(int) (timeHasBeenAlive * angleMultiplyier);
			speed = Vector3.RotateTowards(speed,objectiveDirection,Mathf.Deg2Rad * angleRotation,1f).normalized;
			//transform.position = Vector3.Lerp(transform.position,GameManager.lightGemObject.transform.position,0.05f);
		}
	}

	public void getHurt(int hurtAmmount){
		//Play hurt effects
		Debug.Log ("Hurt");
		GetComponent<Killable> ().TakeDamage (hurtAmmount);
		if(GetComponent<Killable>().isDead()){
			//Play on death effects and despawn
			Debug.Log("Dead");
		}
	}
}

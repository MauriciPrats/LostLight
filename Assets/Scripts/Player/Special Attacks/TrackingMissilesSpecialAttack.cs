﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackingMissilesSpecialAttack : SpecialAttack {

	public GameObject attackCharge;
	public GameObject missilePrefab;
	public float timeToChargeAttack = 0.7f;
	public int maxAmmountOfMissilesSpawned = 4;
	public int damagePerMissile = 2;

	public float range = 8f;


	float timer;
	public override void initialize(){
		
	}

	public void enemyHit(GameObject enemy){
		enemy.GetComponent<Killable>().Damage(damagePerMissile);
	}

	private void spawnMissiles(){
		//Find all close enemies

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		List<GameObject> enemiesInRange = new List<GameObject> (0);
		foreach(GameObject enemy in enemies){
			if(Vector3.Distance(enemy.transform.position,attackCharge.transform.position)<=range){
				enemiesInRange.Add(enemy);
			}
		}

		int ammountOfMissiles = maxAmmountOfMissilesSpawned;
		if(enemiesInRange.Count<ammountOfMissiles){
			ammountOfMissiles = enemiesInRange.Count;
		}

		for(int i = 0;i<ammountOfMissiles;++i){
			GameObject newMissile = Instantiate(missilePrefab);
			newMissile.transform.position = attackCharge.transform.position;
			newMissile.GetComponent<TrackingMissile>().initialize(GameManager.player.transform.up,enemiesInRange[i],this);
		}

		Debug.Log (ammountOfMissiles);

		//Spawn missiles


	}
	
	protected override void update(){
		if(!isFinished){
			timer+=Time.deltaTime;
			if(timer>=timeToChargeAttack){
				spawnMissiles();
				attackCharge.SetActive (false);
				isFinished = true;
			}
		}
	}

	public override void startAttack(){
		timer = 0f;
		attackCharge.SetActive (true);
		attackCharge.transform.position = GameManager.lightGemObject.transform.position + (GameManager.player.transform.up* 0.5f);
		isFinished = false;
	}
}

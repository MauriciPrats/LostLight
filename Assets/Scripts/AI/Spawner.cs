using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spawner : MonoBehaviour {

	public GameObject[] enemyPrefabsCanSpawn;
	public bool isOutsidePlanet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isInRange(){
		float distanceBetweenPlayerAndSpawnPoint = Vector3.Distance (transform.position, GameManager.player.transform.position);

		if(distanceBetweenPlayerAndSpawnPoint<Constants.SPAWNING_MAXIMUM_DISTANCE_OF_PLAYER && distanceBetweenPlayerAndSpawnPoint > Constants.SPAWNING_MINIMUM_DISTANCE_OF_PLAYER){
			return true;
		}else{
			return false;
		}

	}

	public int spawnRandomEnemy(Action<GameObject> actionToCallOnEnemyDead,Action<GameObject> actionToCallOnEnemyDespawned){
		//Spawns a random enemy and returns the weight it has

		GameObject randomEnemyToSpawn = enemyPrefabsCanSpawn [UnityEngine.Random.Range (0, enemyPrefabsCanSpawn.Length)];
		GameObject newEnemySpawned = GameObject.Instantiate (randomEnemyToSpawn) as GameObject;
		newEnemySpawned.transform.position = transform.position;

		EnemySpawned enemySpawnedC = newEnemySpawned.GetComponent<EnemySpawned> ();
		enemySpawnedC.actionToCallOnDie = actionToCallOnEnemyDead;
		enemySpawnedC.actionToCallOnDespawn = actionToCallOnEnemyDespawned;
		
		return enemySpawnedC.pointsCost;
	}
}

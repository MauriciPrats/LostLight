using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Spawner : MonoBehaviour {

	public GameObject[] enemyPrefabsCanSpawn;
	public bool isOutsidePlanet;


	public bool isInRange(){
		float distanceBetweenPlayerAndSpawnPoint = Vector3.Distance (transform.position, GameManager.player.transform.position);

		if(distanceBetweenPlayerAndSpawnPoint<Constants.SPAWNING_MAXIMUM_DISTANCE_OF_PLAYER && distanceBetweenPlayerAndSpawnPoint > Constants.SPAWNING_MINIMUM_DISTANCE_OF_PLAYER){
			return true;
		}else{
			return false;
		}

	}

	public int spawnRandomEnemy(Action<GameObject> actionToCallOnEnemyDead,Action<GameObject> actionToCallOnEnemyDespawned,out EnemyType enemyTypeSpawned,List<EnemyTypeAmmount> ammounts,int ammountLeft){
		//Spawns a random enemy and returns the weight it has
		List<GameObject> enemiesCanBeSpawned = new List<GameObject> (0);
		foreach(GameObject enemyPrefab in enemyPrefabsCanSpawn){
			EnemyType type = enemyPrefab.GetComponent<EnemySpawned>().enemyType;
			bool found = false;
			if(enemyPrefab.GetComponent<EnemySpawned>().pointsCost<=(ammountLeft+1)){
				if(enemyPrefab.GetComponent<EnemySpawned>().maximumEnemiesInPlanet==0){
					enemiesCanBeSpawned.Add(enemyPrefab);
				}else{
					foreach(EnemyTypeAmmount eta in ammounts){
						if(eta.type.Equals(type)){
							if(eta.ammount<enemyPrefab.GetComponent<EnemySpawned>().maximumEnemiesInPlanet){
								enemiesCanBeSpawned.Add(enemyPrefab);
							}
							found = true;
							break;
						}
					}
					if(!found){enemiesCanBeSpawned.Add(enemyPrefab);}
				}
			}
		}

		if(enemiesCanBeSpawned.Count>0){
			GameObject randomEnemyToSpawn = enemiesCanBeSpawned [UnityEngine.Random.Range (0, enemiesCanBeSpawned.Count)];
			GameObject newEnemySpawned = GameObject.Instantiate (randomEnemyToSpawn) as GameObject;
			newEnemySpawned.transform.position = transform.position;

			EnemySpawned enemySpawnedC = newEnemySpawned.GetComponent<EnemySpawned> ();
			enemyTypeSpawned = enemySpawnedC.enemyType;
			enemySpawnedC.actionToCallOnDie = actionToCallOnEnemyDead;
			enemySpawnedC.actionToCallOnDespawn = actionToCallOnEnemyDespawned;
		
			return enemySpawnedC.pointsCost;
		}else{
			enemyTypeSpawned = EnemyType.None;
			return 0;
		}
	}
}

using UnityEngine;
using System.Collections;

public class EnemyPrefabManager : MonoBehaviour {

	public GameObject[] enemyPrefabs;
	public GameObject spawnBall;
	
	void Awake(){
		GameManager.registerEnemyPrefabManager (this);
	}

	public GameObject getPrefab(EnemyType type){
		foreach(GameObject enemy in enemyPrefabs){
			EnemySpawned enemySpawned = enemy.GetComponent<EnemySpawned>();
			if(enemySpawned.enemyType.Equals(type)){
				return enemy;
			}
		}
		return null;
	}

	public GameObject getSpawnBall(){
		return spawnBall;
	}

}

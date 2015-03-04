using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetSpawnerManager : MonoBehaviour {

	public GameObject[] spawners;
	

	public int pointsUntilSeal1ShintoDoor;
	public int pointsUntilSeal2ShintoDoor;
	public int pointsUntilSeal3ShintoDoor;
	public int maxPointsSpawnedAtSameTime;

	public float timeBetweenSpawns;
	private int ammountOfActualPointsSpawned = 0;
	private int accumulatedPoints = 0;




	private float timerSpawn = 0f;

	void Awake(){
		GameManager.registerPlanetSpawner (gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.gameState.isGameEnded){
			timerSpawn += Time.deltaTime;
			if(ammountOfActualPointsSpawned< maxPointsSpawnedAtSameTime && timerSpawn > timeBetweenSpawns){
				timerSpawn = 0f;
				//Check if any of the spawners is in range for spawning
				List<Spawner> inRangeSpawners = new List<Spawner>(0);

				foreach(GameObject spawn in spawners){
					Spawner s = spawn.GetComponent<Spawner>();
					if(s.isInRange()){
						inRangeSpawners.Add(s);
					}
				}
				//We get a random spawner and we make him spawn a random creature
				if(inRangeSpawners.Count>0){
					Spawner randomSpawner = inRangeSpawners[Random.Range(0,inRangeSpawners.Count)];
					ammountOfActualPointsSpawned+= randomSpawner.spawnRandomEnemy(onEnemyDead,onEnemyDespawned);
				}
			}
		}
	}

	public void onEnemyDead(GameObject enemy){
		EnemySpawned enemySpawned = enemy.GetComponent<EnemySpawned> ();
		accumulatedPoints+=enemySpawned.pointsCost;
		ammountOfActualPointsSpawned-=enemySpawned.pointsCost;
		
		if(accumulatedPoints>=pointsUntilSeal3ShintoDoor){
			shintoDoorEffect (3);
		}else if(accumulatedPoints>=pointsUntilSeal2ShintoDoor){
			shintoDoorEffect (2);
		}else if(accumulatedPoints>=pointsUntilSeal1ShintoDoor){
			shintoDoorEffect (1);
		}
	}

	public void onEnemyDespawned(GameObject enemy){

		EnemySpawned enemySpawned = enemy.GetComponent<EnemySpawned> ();
		//It has despawned because it's superfar away
		ammountOfActualPointsSpawned-=enemySpawned.pointsCost;
		//We despawn the enemy
		Destroy(enemy);

	}

	public void shintoDoorEffect(int level){
		//Do dramatic camera move
	}
}

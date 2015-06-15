using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (PlanetCorruption))]
public class PlanetSpawnerManager : MonoBehaviour {

	public GameObject[] spawners;
	public GameObject shintoDoor;
	public bool isActive;

	public int pointsUntilSealShintoDoor;

	public int maxPointsSpawnedAtSameTime;

	public float timeBetweenSpawns;
	private int ammountOfActualPointsSpawned = 0;
	private int accumulatedPoints = 0;

	public List<EnemyTypeAmmount> enemiesAmmount = new List<EnemyTypeAmmount> (0);
	private PlanetCorruption planetCorruption;
	
	
	private int lastLevelShintoActivated = 0;



	private float timerSpawn = 0f;

	void Awake(){
		GameManager.registerPlanetSpawner (gameObject);
	}

	// Use this for initialization
	void Start () {
		planetCorruption = GetComponent<PlanetCorruption> ();
		//shinto = shintoDoor.GetComponent<ShintoDoor> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.gameState.isGameEnded && isActive && planetCorruption.getSpawningEnabled()){
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
					EnemyType type;
					int ammountLeft = pointsUntilSealShintoDoor -(ammountOfActualPointsSpawned + accumulatedPoints);
					ammountOfActualPointsSpawned+= randomSpawner.spawnRandomEnemy(onEnemyDead,onEnemyDespawned,out type,enemiesAmmount,ammountLeft);
					if(!type.Equals(EnemyType.None)){
						addEnemyType(type);
					}
				}
			}
		}
	}

	private void addEnemyType(EnemyType type){
		bool found = false;
		foreach(EnemyTypeAmmount ea in enemiesAmmount){
			if(ea.type.Equals(type)){
				ea.ammount++;
				found = true;
				break;
			}
		}
		if(!found){
			EnemyTypeAmmount ea = new EnemyTypeAmmount();
			ea.type = type;
			ea.ammount = 1;
			enemiesAmmount.Add(ea);
		}
	}

	private void substractEnemyType(EnemyType type){

		bool found = false;
		foreach(EnemyTypeAmmount ea in enemiesAmmount){
			if(ea.type.Equals(type)){
				ea.ammount--;
				found = true;
				break;
			}
		}
	}

	public void onEnemyDead(GameObject enemy){
		if (isActive) {
			EnemySpawned enemySpawned = enemy.GetComponent<EnemySpawned> ();
			//accumulatedPoints += enemySpawned.pointsCost;
			ammountOfActualPointsSpawned -= enemySpawned.pointsCost;
			substractEnemyType (enemySpawned.enemyType);
		}
	}

	public void incrementAccumulatedPoints(int ammountPoints){
		accumulatedPoints += ammountPoints;
		GUIManager.setPercentageCorruption ((float)accumulatedPoints / (float)pointsUntilSealShintoDoor);
		if(accumulatedPoints>=pointsUntilSealShintoDoor){
			isActive = false;
			GUIManager.deactivateCorruptionBar();
			GetComponent<PlanetCorruption>().cleanCorruption();
			//We clean all the enemies of the planet
			foreach(IAController iaController in GameManager.iaManager.getActualAIs()){
				iaController.die(true);
			}
		}
	}

	public void onEnemyDespawned(GameObject enemy){
		EnemySpawned enemySpawned = enemy.GetComponent<EnemySpawned> ();
		//It has despawned because it's superfar away
		ammountOfActualPointsSpawned-=enemySpawned.pointsCost;
		substractEnemyType (enemySpawned.enemyType);
	}


	/*public void shintoDoorEffect(int level){
		if(level>lastLevelShintoActivated){
			lastLevelShintoActivated = level;
			shinto.activateKanjiLevel (level);
		}
	}*/

	public void activate(){
		if(accumulatedPoints<pointsUntilSealShintoDoor){
			isActive = true;
			if(accumulatedPoints<pointsUntilSealShintoDoor){
				GUIManager.activateCorruptionBar ();
				GUIManager.setPercentageCorruption ((float)accumulatedPoints / (float)pointsUntilSealShintoDoor);
			}
		}
	}

	public void deactivate(){
		isActive = false;
	}
}

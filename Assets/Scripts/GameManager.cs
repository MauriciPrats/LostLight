using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager{

	public static GameObject player;
	public static SceneManager actualSceneManager;
	public static GameState gameState = new GameState ();
	public static GameObject mainCamera;
	public static GameObject minimapCamera;
	public static List<PlanetSpawnerManager> planetSpawnersManagers = new List<PlanetSpawnerManager>(0);
	public static CheckpointManager checkPointManager;
	
	//Game state functions
	public static void putLoadedGameState(GameState gameStateLoaded){
		gameState = gameStateLoaded;
	}

	public static void rebuildGameFromGameState(){
		//We put the player in the last checkpoint
		CheckpointStub lastCheckpoint = checkPointManager.getLastCheckpoint();
		if(lastCheckpoint!=null){
			player.transform.position = lastCheckpoint.checkPointObject.transform.position;
		}else{
			Debug.Log("No checkpoint to put Big P. in");
		}	

		player.GetComponent<CharacterController>().reset();
		player.GetComponent<GravityBody>().attract();
		mainCamera.GetComponent<CameraFollowingPlayer>().resetPosition();
		minimapCamera.SetActive(false);

		//Despawn all the enemies
		EnemySpawned[] enemies = (EnemySpawned[])GameObject.FindObjectsOfType (typeof(EnemySpawned));
		foreach(EnemySpawned enemy in enemies){
			//It works since it doesn't destroy it immediatly?
			enemy.OnEnemyDead();
		}
	}

	public static void pauseGame(){
		//Time.timeScale = 0f;
	}

	public static void unPauseGame(){
		//Time.timeScale = 1f;
	}

	//Game functions
	public static void loseGame(){
		GameManager.gameState.isGameEnded = true;
		GUIManager.fadeInWithAction(rebuildGameFromGameState,Menu.YouLostMenu);
	}

	public static void startGame(){
		gameState.isGameEnded = false;
		minimapCamera.SetActive (true);
		//unPauseGame ();
	}

	//Registering functions
	public static void registerPlayer(GameObject playerGO){
		player = playerGO;
	}

	public static void registerCheckpointManager(GameObject checkPointManagerGO){
		checkPointManager = checkPointManagerGO.GetComponent<CheckpointManager> ();
	}

	public static void registerActualSceneManager(GameObject sceneManager){
		actualSceneManager = sceneManager.GetComponent<SceneManager> ();
	}

	public static void registerMainCamera(GameObject mainCameraGO){
		mainCamera = mainCameraGO;
	}

	public static void registerMinimapCamera(GameObject minimapCameraGO){
		minimapCamera = minimapCameraGO;
	}

	public static void registerPlanetSpawner(GameObject spawnerGO){
		planetSpawnersManagers.Add(spawnerGO.GetComponent<PlanetSpawnerManager>());
	}
}

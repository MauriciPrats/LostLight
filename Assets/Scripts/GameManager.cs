﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager{

	public static GameObject player;
	public static GameObject lightGemObject;
	public static SceneManager actualSceneManager;
	public static GameState gameState = new GameState ();
	public static GameObject mainCamera;
	public static GameObject minimapCamera;
	public static List<Light> directionalLights;
	public static EnemyAttackManager enemyAttackManager;
	public static List<PlanetSpawnerManager> planetSpawnersManagers = new List<PlanetSpawnerManager>(0);
	public static CheckpointManager checkPointManager;

	private static Color originalAmbientLight;

	public static void changeDirectionalLights(bool enabled){
		/*foreach(Light light in directionalLights){
			light.enabled = enabled;
		}
		if(enabled==false){
			RenderSettings.ambientLight = new Color(0f,0f,0f);
		}else{
			RenderSettings.ambientLight = originalAmbientLight;
		}*/
	}

	//Game state functions
	public static void putLoadedGameState(GameState gameStateLoaded){
		gameState = gameStateLoaded;
	}

	public static void rebuildGameFromGameState(){
		originalAmbientLight = RenderSettings.ambientLight;
		Light[] lights = GameObject.FindObjectsOfType<Light>() as Light[];
		directionalLights = new List<Light> (0);
		foreach(Light light in lights){
			if(light.type == LightType.Directional){
				directionalLights.Add(light);
			}
		}


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
			enemy.OnDespawn();
		}
	}

	public static void pauseGame(){
		Time.timeScale = 0f;
		gameState.isGamePaused = true;
	}

	public static void unPauseGame(){
		Time.timeScale = 1f;
		gameState.isGamePaused = false;
	}

	//Game functions
	public static void loseGame(){
		GameManager.gameState.isGameEnded = true;
		GUIManager.fadeInWithAction(rebuildGameFromGameState,Menu.YouLostMenu);
	}

	//Game functions
	public static void winGame(){
		GameManager.gameState.isGameEnded = true;
		GameManager.player.GetComponent<CharacterController> ().isInvulnerable = true;
		GUIManager.fadeInWithAction(rebuildGameFromGameState,Menu.YouWonMenu);
	}

	public static void startGame(){
		gameState.isGameEnded = false;
		minimapCamera.SetActive (true);
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

	public static void registerEnemyAttackManager(GameObject enemyAttackManagerGO){
		enemyAttackManager = enemyAttackManagerGO.GetComponent<EnemyAttackManager> ();
	}

	public static void registerLightGemObject(GameObject lightGemGO){
		lightGemObject = lightGemGO;
	}
}

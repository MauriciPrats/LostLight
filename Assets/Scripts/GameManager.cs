﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameManager{

	public static GameObject player;
	public static Animator playerAnimator;
	public static PlayerController playerController;
	public static SpaceGravityBody playerSpaceBody;
	public static InputController inputController;
	public static SceneManager actualSceneManager;
	public static GameState gameState = new GameState ();
	public static GameObject mainCamera;
	public static GameObject minimapCamera;
	public static List<Light> directionalLights;
	public static EnemyAttackManager enemyAttackManager;
	public static List<PlanetSpawnerManager> planetSpawnersManagers = new List<PlanetSpawnerManager>(0);
	public static CheckpointManager checkPointManager;
	public static LightGemEnergyManager lightGemEnergyManager;
	public static LightGemSoulsManager lightGemSoulsManager;
	public static ComboManager comboManager;
	public static DialogueManager dialogueManager;
	public static List<ProcedurallyGeneratedObject> proceduralGrass = new List<ProcedurallyGeneratedObject>(0);
	public static GrassManager grassManager;
	public static IAManager iaManager;
	public static AudioManager audioManager;
	public static LightsManager lightsManager;
	public static OptionsManager optionsManager;

	public static void setGrassPorcentualLevel(float percentage){
		foreach(ProcedurallyGeneratedObject pgo in proceduralGrass){
			pgo.setDetailPercentage(percentage);
		}
	}

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

		player.GetComponent<PlayerController>().reset();
		player.GetComponent<SpaceGravityBody> ().bindToClosestPlanet ();

		mainCamera.GetComponent<CameraFollowingPlayer>().resetPosition();
		//minimapCamera.SetActive(false);

		//Despawn all the enemies
		EnemySpawned[] enemies = (EnemySpawned[])GameObject.FindObjectsOfType (typeof(EnemySpawned));
		foreach(EnemySpawned enemy in enemies){
			//It works since it doesn't destroy it immediatly?
			enemy.OnDespawn();
		}
	}

	public static void pauseGame(){
		GameManager.inputController.disableInputController ();
		iaManager.disableIAs ();
		gameState.isGamePaused = true;
	}

	public static void unPauseGame(){
		GameManager.inputController.enableInputController ();
		iaManager.enableIAs ();
		gameState.isGamePaused = false;
	}

	public static bool isGamePaused(){
		return gameState.isGamePaused;
	}

	//Game functions
	public static void loseGame(){
		GUIManager.deactivatePlayingGUI ();
		GameManager.gameState.isGameEnded = true;
		GUIManager.fadeInWithAction(rebuildGameFromGameState,Menu.YouLostMenu);
	}

	public static void winGame(){
		GameManager.gameState.isGameEnded = true;
		GameManager.player.GetComponent<PlayerController> ().isInvulnerable = true;
		GUIManager.fadeInWithAction(rebuildGameFromGameState,Menu.YouWonMenu);
	}

	public static void startGame(){
		gameState.isGameEnded = false;
		GameManager.inputController.enableInputController ();
		if(GameManager.playerSpaceBody.getClosestPlanet()!=null){
			if(GameManager.playerSpaceBody.getClosestPlanet().isPlanetCorrupted()){
				(GameManager.playerSpaceBody.getClosestPlanet() as PlanetCorrupted).getPlanetEventsManager().startButtonPressed();
			}else{
				GUIManager.activatePlayingGUIWithFadeIn();
			}
		}

		GameManager.audioManager.PlayStableSound(0);
		GameManager.audioManager.playSong(1);
	}

	public static void restartGame(){
		gameState.isGameEnded = false;
		GUIManager.activatePlayingGUIWithFadeIn();
	}


	//Registering functions
	public static void registerPlayer(GameObject playerGO){
		player = playerGO;
		playerAnimator = player.GetComponent<PlayerController> ().getAnimator ();
		playerController = player.GetComponent<PlayerController> ();
		playerSpaceBody = player.GetComponent<SpaceGravityBody> ();
		inputController = player.GetComponent<InputController> ();
	}

	public static bool getIsInsidePlanet(){
		Planet closestPlanet = playerSpaceBody.getClosestPlanet ();
		if(closestPlanet!=null && closestPlanet.getHasInsidePlanet() && closestPlanet.getHideFrontPlanetFaceOnEnter().getIsInsidePlanet()){
			return true;
		}
		return false;
	}
	
	
	public static void RegisterAudioManager(AudioManager registerMe) {
		audioManager = registerMe;
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

	public static void registerLightGemEnergyManager(GameObject lightGemEnergyManagerGO){
		lightGemEnergyManager = lightGemEnergyManagerGO.GetComponent<LightGemEnergyManager> ();
	}

	public static void registerLightGemSoulsManager(GameObject lightGemSoulsManagerGO){
		lightGemSoulsManager = lightGemSoulsManagerGO.GetComponent<LightGemSoulsManager> ();
	}

	public static void registerProceduralGrass(ProcedurallyGeneratedObject pGrass){
		proceduralGrass.Add (pGrass);
	}

	public static void registerComboManager(GameObject comboManagerGO){
		comboManager = comboManagerGO.GetComponent<ComboManager> ();
	}

	public static void registerDialogueManager(DialogueManager dialogueM){
		dialogueManager = dialogueM;
	}

	public static void registerGrassManager(GrassManager gM){
		grassManager = gM;
	}

	public static void registerIAManager(IAManager iaM){
		iaManager = iaM;
	}

	public static void registerLightsManager(LightsManager lm){
		lightsManager = lm;
	}

	public static void registerOptionsManager(OptionsManager om){
		optionsManager = om;
	}
}

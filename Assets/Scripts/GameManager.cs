using UnityEngine;
using System.Collections;

public class GameManager{

	public static GameObject player;
	public static SceneManager actualSceneManager;
	public static GameState gameState = new GameState ();
	public static GameObject mainCamera;
	public static GameObject minimapCamera;

	public static CheckpointManager checkPointManager;


	//Game state functions
	public static void putLoadedGameState(GameState gameStateLoaded){
		gameState = gameStateLoaded;
	}

	public static void putPlayerPosition(){

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
		player.GetComponent<GravityBody> ().attract ();
		mainCamera.GetComponent<CameraFollowingPlayer> ().resetPosition ();

		minimapCamera.SetActive (false);
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
		GUIManager.fadeOutChangeMenuFadeInWithAction(rebuildGameFromGameState,Menu.YouLostMenu);
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
}

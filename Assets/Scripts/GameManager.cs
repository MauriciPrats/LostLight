using UnityEngine;
using System.Collections;

public class GameManager{

	public static GameObject player;

	public static SceneManager actualSceneManager;

	public static void registerPlayer(GameObject playerGO){
		player = playerGO;
	}

	public static void registerActualSceneManager(GameObject sceneManager){
		actualSceneManager = sceneManager.GetComponent<SceneManager> ();
	}
}

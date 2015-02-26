using UnityEngine;
using System.Collections;

public class GameManager{

	public static GameObject player;

	public static FadeManager fadeManager;

	public static void registerPlayer(GameObject playerGO){
		player = playerGO;
	}

	public static void registerFadeManager(GameObject fadeManagerGO){
		fadeManager = fadeManagerGO.GetComponent<FadeManager> ();
	}
}

using UnityEngine;
using System.Collections;

public class GameManager{

	public static GameObject player;

	public static void registerPlayer(GameObject playerGO){
		player = playerGO;
	}
}

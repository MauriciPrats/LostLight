using UnityEngine;
using System.Collections;

public class GameState{
		
	public int playerLastCheckpoint;

	public bool isGameEnded;

	public bool isCameraLockedToPlayer;

	private void initializeGameState(){
		playerLastCheckpoint = 0;
		isGameEnded = true;
		isCameraLockedToPlayer = true;
	}

	public GameState(){
		initializeGameState ();
	}
}

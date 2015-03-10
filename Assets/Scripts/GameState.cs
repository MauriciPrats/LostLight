using UnityEngine;
using System.Collections;

public class GameState{
		
	public int playerLastCheckpoint;

	public bool isGameEnded;

	public bool isCameraLockedToPlayer;

	public bool isGamePaused;

	private void initializeGameState(){
		playerLastCheckpoint = 0;
		isGameEnded = true;
		isCameraLockedToPlayer = true;
		isGamePaused = false;
	}

	public GameState(){
		initializeGameState ();
	}
}

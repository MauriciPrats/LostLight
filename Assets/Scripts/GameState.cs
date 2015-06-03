using UnityEngine;
using System.Collections;

public class GameState{
		
	public int playerLastCheckpoint;

	public bool isGameEnded;

	public bool isCameraLockedToPlayer;

	public bool isGamePaused;

	public bool arePlanetsMoving;

	public bool canPlayerSpaceJump;

	private void initializeGameState(){
		playerLastCheckpoint = 0;
		isGameEnded = true;
		isCameraLockedToPlayer = true;
		isGamePaused = false;
		arePlanetsMoving = true;
		canPlayerSpaceJump = true;
	}

	public GameState(){
		initializeGameState ();
	}
}

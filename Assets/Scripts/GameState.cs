using UnityEngine;
using System.Collections;

public class GameState{
		
	public int playerLastCheckpoint;

	public bool isGameEnded;

	private void initializeGameState(){
		playerLastCheckpoint = 0;
		isGameEnded = true;
	}

	public GameState(){
		initializeGameState ();
	}
}

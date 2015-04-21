using UnityEngine;
using System.Collections;

public class GameState{
		
	public int playerLastCheckpoint;

	public bool isGameEnded;

	public bool isCameraLockedToPlayer;

	public bool isGamePaused;

	public bool arePlanetsMoving;

	public bool isInsidePlanet;

	private void initializeGameState(){
		playerLastCheckpoint = 0;
		isGameEnded = true;
		isCameraLockedToPlayer = true;
		isGamePaused = false;
		arePlanetsMoving = true;
		isInsidePlanet = false;
	}

	public GameState(){
		initializeGameState ();
	}

	public void setIsInsidePlanet(bool insidePlanet){
		if(isInsidePlanet!=insidePlanet){
			isInsidePlanet = insidePlanet;
			GameManager.changeDirectionalLights(!insidePlanet);

		}
	}
}

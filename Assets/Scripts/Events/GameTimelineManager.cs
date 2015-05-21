using UnityEngine;
using System.Collections;

public class GameTimelineManager : MonoBehaviour {

	bool hasUpdated = false;
	void Update () {

		if(!hasUpdated){
			gameStarts();
			hasUpdated = true;
		}
	}

	void gameStarts(){
		//It's the start of the game
		GameManager.rebuildGameFromGameState ();
		GUIManager.activateMenu(Menu.MainMenu);
		GUIManager.fadeAllIn();

		//Get the actual planet and initialize it
		GameObject closestPlanetPlayer = GameManager.playerSpaceBody.getClosestGravityAttractor ();
		if(closestPlanetPlayer!=null){
			PlanetEventsManager pem = closestPlanetPlayer.GetComponent<PlanetEventsManager>();
			if(pem!=null){
				pem.activate();
			}
		}
	}



}

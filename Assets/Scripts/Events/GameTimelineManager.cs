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
		GUIManager.deactivatePlayingGUI();
		GUIManager.activateMenu(Menu.MainMenu);
		GUIManager.fadeAllIn();

		//Get the actual planet and initialize it
		Planet planet = GameManager.playerSpaceBody.getClosestPlanet ();
		if(planet!=null && planet.isPlanetCorrupted()){
			(planet as PlanetCorrupted).getPlanetEventsManager().activate();
		}
	}



}

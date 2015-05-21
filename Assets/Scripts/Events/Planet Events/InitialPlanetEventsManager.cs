﻿using UnityEngine;
using System.Collections;

public class InitialPlanetEventsManager : PlanetEventsManager {

	public GameObject bigPappadaInitialPosition;
	public GameObject littleGInitialPosition;

	public override void isActivated(){
		//First event is putting Big P in the initial position
		GameManager.player.transform.position = bigPappadaInitialPosition.transform.position;
		GameManager.playerSpaceBody.setStatic (true);
		GameManager.player.transform.rotation = Quaternion.LookRotation (Vector3.forward*-1f, bigPappadaInitialPosition.transform.up*-1f);
		//GameManager.playerAnimator.SetBool ("isChargingSpaceJumping", true);
		GameManager.playerSpaceBody.bindToClosestPlanet ();
		GameManager.inputController.disableInputController ();
	}

	IEnumerator firstCinematic(){

		float timer = 0f;
		float time = 0.2f;
		float originalZ = GameManager.player.transform.position.z;
		while(timer<time){
			timer+=Time.deltaTime;
			float ratio = timer/time;
			GameManager.player.transform.position = new Vector3(GameManager.player.transform.position.x,GameManager.player.transform.position.y,originalZ*(1f-ratio));
			yield return null;
		}
		yield return new WaitForSeconds (1.5f);
		GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Debes concentrarte Little G. ! ", 3f, false, false);
		yield return new WaitForSeconds (3f);
		GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Me voy a cazar jabalies! ", 2f, false, false);
		yield return new WaitForSeconds (2f);
		GameManager.inputController.enableInputController ();
	}

	public override void startButtonPressed(){
		if(isEnabled){
			GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().resetXAngle();
			//GameManager.playerAnimator.SetBool ("isChargingSpaceJumping", false);
			//GameManager.playerAnimator.SetBool ("isGoingUp", false);
			//GameManager.playerAnimator.SetBool ("isSpaceJumping", false);
			GameManager.playerAnimator.SetBool ("isJumping", true);
			GameManager.player.GetComponent<CharacterController>().StopMoving();
			GameManager.playerSpaceBody.setStatic (false);
			GameManager.player.GetComponent<CharacterController>().Jump(12f);
			GameManager.playerController.initializePlayerRotation();
			GameManager.player.GetComponent<CharacterController>().LookLeftOrRight(-1f);

			StartCoroutine("firstCinematic");
		}
	}

	public override void informEventActivated (CutsceneIdentifyier identifyier){
		if(isEnabled){
			if(identifyier.Equals(CutsceneIdentifyier.SanctuaryLightGem)){
				GetComponent<PlanetCorruption>().corrupt();
				GetComponent<PlanetCorruption>().activateSpawning();
			}
		}
	}

	public override void initialize(){
		GetComponent<PlanetCorruption> ().setCorruptionToClean ();

	}
}

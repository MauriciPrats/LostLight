using UnityEngine;
using System.Collections;

public class InitialPlanetEventsManager : PlanetEventsManager {

	public GameObject bigPappadaInitialPosition;
	public GameObject littleGHopper;

	public GameObject boarHuntingGO;
	public GameObject shintoDoorGO;
	public GameObject toTheBridgeGO;
	public GameObject bridgeFallGO;
	public GameObject lightGemGO;

	bool firstCinematicPlayed = false;
	bool hasBeenActivated = false;

	private GameObject bigPappadaDialogue;
	private GameObject littleGDialogue;

	//Is called when the class is activated by the GameTimelineManager
	public override void isActivated(){
		if(!hasBeenActivated){
			GameManager.player.transform.position = new Vector3(bigPappadaInitialPosition.transform.position.x,bigPappadaInitialPosition.transform.position.y,0f);
			if(isEnabled){
				hasBeenActivated = true;
				//First event is putting Big P in the initial position
				GameManager.player.transform.position = bigPappadaInitialPosition.transform.position;
				GameManager.player.transform.rotation = Quaternion.LookRotation (Vector3.forward*-1f, bigPappadaInitialPosition.transform.up*-1f);
				GameManager.mainCamera.GetComponent<CameraFollowingPlayer>().resetPosition();
				GameManager.playerSpaceBody.bindToClosestPlanet ();
				GameManager.playerSpaceBody.setStatic (true);
				GameManager.player.GetComponent<CharacterController> ().StopMoving ();
				GameManager.player.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
				GameManager.inputController.disableInputController ();
				littleGHopper.GetComponent<SpaceGravityBody> ().bindToClosestPlanet ();
				littleGHopper.GetComponent<SpaceGravityBody> ().setStatic (true);
				littleGHopper.GetComponent<CharacterController> ().StopMoving ();
				littleGHopper.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);

				toTheBridgeGO.GetComponent<Cutscene>().isActive = false;
				bridgeFallGO.GetComponent<Cutscene>().isActive = false;
				GUIManager.deactivateMinimapGUI();
			}
			boarHuntingGO.GetComponent<Cutscene>().isActive = false;
			boarHuntingGO.GetComponent<Cutscene> ().Initialize ();
		}
	}

	//This cinematic corresponds to the first cinematic that will play when the play button is pressed
	IEnumerator firstCinematic(){
		if(!firstCinematicPlayed){
			if(isEnabled){
				firstCinematicPlayed = true;
				float timer = 0f;
				float time = 0.2f;
				littleGHopper.GetComponentInChildren<Animator>().SetBool("isFallingDown",true);
				yield return new WaitForSeconds (1f);
				GameManager.audioManager.PlayStableSound(1);
				littleGHopper.GetComponent<SpaceGravityBody> ().setStatic (false);
				float originalZ = littleGHopper.transform.position.z;

				littleGDialogue = littleGHopper.GetComponent<DialogueController> ().createNewDialogue ("AAaaah..", 2.5f, true, false);
				while(timer<time){
					timer+=Time.deltaTime;
					float ratio = timer/time;
					littleGHopper.transform.position = new Vector3(littleGHopper.transform.position.x,littleGHopper.transform.position.y,originalZ*(1f-ratio) -0.15f);
					yield return null;
				}

				yield return new WaitForSeconds (0.7f);
				littleGHopper.GetComponent<CharacterController> ().setOriginalOrientation ();
				littleGHopper.GetComponent<CharacterController> ().LookLeftOrRight (1f);
				yield return new WaitForSeconds (0.7f);
				GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().resetXAngle();
				GameManager.playerAnimator.SetBool ("isJumping", true);
				GameManager.player.GetComponent<CharacterController>().StopMoving();
				GameManager.playerSpaceBody.setStatic (false);
				GameManager.player.GetComponent<CharacterController>().Jump(12f);
				GameManager.playerController.initializePlayerRotation();
				GameManager.player.GetComponent<CharacterController>().LookLeftOrRight(-1f);
				
				timer = 0f;
				originalZ = GameManager.player.transform.position.z;
				while(timer<time){
					timer+=Time.deltaTime;
					float ratio = timer/time;
					GameManager.player.transform.position = new Vector3(GameManager.player.transform.position.x,GameManager.player.transform.position.y,(originalZ *(1f-ratio)) -0.1f);
					yield return null;
				}
				yield return new WaitForSeconds (1f);
				littleGHopper.GetComponentInChildren<Animator>().SetBool("isFallingDown",false);
				yield return new WaitForSeconds (1f);
				bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Debes concentrar \n tu energia Little G.", 4f, false, false);
				yield return StartCoroutine(WaitInterruptable (4f,bigPappadaDialogue));
				bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("El equilibrio no está en el cuerpo,\n está en la mente. ", 4f, false, false);
				yield return StartCoroutine(WaitInterruptable (4f,bigPappadaDialogue));
				littleGDialogue = littleGHopper.GetComponent<DialogueController> ().createNewDialogue ("Si, maestro ", 3f, false, false);
				yield return StartCoroutine(WaitInterruptable (3f,littleGDialogue));
				bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Ya es suficiente entrenamiento \n por hoy. ", 4f, false, false);
				yield return StartCoroutine(WaitInterruptable (4f,bigPappadaDialogue));
				bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Ve al templo,\n Yo ire a cazar algo para cenar ", 4f, false, false);
				yield return StartCoroutine(WaitInterruptable (4f,bigPappadaDialogue));
				littleGHopper.GetComponent<CharacterController> ().Move (-1f);
				littleGHopper.GetComponentInChildren<Animator> ().SetBool ("isWalking", true);
				GameManager.inputController.enableInputController ();
				
				boarHuntingGO.SetActive (true);
				boarHuntingGO.GetComponent<Cutscene>().isActive = true;
				
				yield return new WaitForSeconds (7f);
				littleGHopper.GetComponent<CharacterController> ().StopMoving ();
				littleGHopper.GetComponentInChildren<Animator> ().SetBool ("isWalking", false);
			}
		}
	}

	//Cinematic that corresponds to the boar hunting event
	IEnumerator secondCinematic(){
		if(isEnabled){
			boarHuntingGO.GetComponent<FirstPlanetBoarHunting>().makeBoarGoAway();
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Un Jabali!", 2f, false, false);
			yield return null;
		}
	}

	//Cinematic that corresponds to the third cinematic
	IEnumerator thirdCinematic(){
		if(isEnabled){
			shintoDoorGO.GetComponent<Cutscene>().isActive = false;
			boarHuntingGO.GetComponent<Cutscene>().isActive = false;
			boarHuntingGO.GetComponent<FirstPlanetBoarHunting>().boar.SetActive(false);
			GameManager.inputController.disableInputController ();
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Que raro... ", 1.5f, false, false);
			yield return StartCoroutine(WaitInterruptable (1.5f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Nunca habia visto \n los sellos brillar de esta forma", 4f, false, false);
			yield return StartCoroutine(WaitInterruptable (4f,bigPappadaDialogue));
			GetComponent<PlanetCorruption>().corrupt();
			GameManager.audioManager.playSong(2);
			
			shintoDoorGO.GetComponent<FirstPlanetShintoDoor>().shintoDoor.GetComponent<ShintoDoor>().disableKanjis();
			yield return new WaitForSeconds(2f);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Waaah!!!", 1f, true, false);
			yield return StartCoroutine(WaitInterruptable (1f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡¿Que es estoo?!", 1f, true, false);
			yield return StartCoroutine(WaitInterruptable (5f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("El templo!! Little G.!", 2f, true, false);
			yield return StartCoroutine(WaitInterruptable (2f,bigPappadaDialogue));
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().planetGettingCorrupted.SetActive(true);
			GameManager.player.GetComponent<PlayerController>().Move(1f);
			toTheBridgeGO.GetComponent<Cutscene>().isActive = true;



			//GameManager.inputController.enableInputController ();

		}
	}
	IEnumerator fourthCinematic(){
		if (isEnabled) {
			littleGHopper.GetComponentInChildren<ParticleSystem>().Play();
			GameManager.player.GetComponent<PlayerController>().Jump();
			GameManager.player.GetComponent<PlayerController>().Move(1f);
			toTheBridgeGO.GetComponent<Cutscene>().isActive = false;
			bridgeFallGO.GetComponent<Cutscene>().isActive = true;
			yield return null;
		}
	}
	IEnumerator fifthCinematic(){
		if(isEnabled){
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().isActive = false;
			GameManager.player.GetComponent<PlayerController>().StopMove();
			littleGDialogue = littleGHopper.GetComponent<DialogueController> ().createNewDialogue ("Aaahhh!!\n MAESTROO!!", 1.5f, true, false);
			yield return StartCoroutine(WaitInterruptable (1f,littleGDialogue));
			littleGHopper.GetComponent<CharacterController>().Jump(35f);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡Little G!!", 1f, false, false);
			yield return StartCoroutine(WaitInterruptable (1f,bigPappadaDialogue));
			GameManager.inputController.disableInputController ();
			//bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().bridge.GetComponent<Collider>().enabled = false;
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().bridge.GetComponent<RotateAndMoveOverTime>().changeOverTime(1f);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡Aaaaah!!", 1.5f, false, false);
			GameManager.playerAnimator.SetTrigger("isHurt");
			GUIManager.fadeIn(Menu.BlackMenu);
			yield return new WaitForSeconds(5f);
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().planetGettingCorrupted.SetActive(false);
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().hideOutsidePlane.SetActive(true);
			littleGHopper.SetActive(false);
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().fallenRocks.SetActive(true);
			GUIManager.fadeOut(null);
			GameManager.audioManager.playSong(3);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¿Donde estoy? \n ¿Que es este lugar?", 3f, false, false);
			yield return StartCoroutine(WaitInterruptable (3f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Parece que no tengo \n otra alternativa que \n seguir adelante...", 3f, false, false);
			yield return StartCoroutine(WaitInterruptable (3f,bigPappadaDialogue));
			GameManager.inputController.enableInputController();
		}
	}


	IEnumerator sixthCinematic(){
		if(isEnabled){
			GameManager.inputController.disableInputController ();
			lightGemGO.GetComponent<SanctuaryLightGem>().isActive = false;
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("La lightgem de\n Whiteheart sensei!?", 3f, false, false);
			yield return StartCoroutine(WaitInterruptable (3f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Me pregunto que hace aqui...", 3f, false, false);
			yield return StartCoroutine(WaitInterruptable (3f,bigPappadaDialogue));
			GameManager.player.GetComponent<PlayerController>().Move(-1f);
			yield return new WaitForSeconds(0.25f);
			GameManager.player.GetComponent<PlayerController>().StopMove();
			float timer = 0f;
			float time = 3f;
			Vector3 lightGemOriginalPosition = lightGemGO.GetComponent<SanctuaryLightGem>().lightGemGO.transform.position;

			while(timer<time){
				timer+=Time.deltaTime;
				float proportion = timer/time;
				lightGemGO.GetComponent<SanctuaryLightGem>().lightGemGO.transform.position = Vector3.Lerp(lightGemOriginalPosition,GameManager.playerController.lightGemObject.transform.position,proportion);
				yield return null;
			}
			foreach(ParticleSystem particles in lightGemGO.GetComponentsInChildren<ParticleSystem>()){
				particles.Stop();
			}
			GameManager.playerAnimator.SetBool("isDoingMissiles",true);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Fuaaah!!! Que poder!", 2f, true, false);
			yield return StartCoroutine(WaitInterruptable (2f,bigPappadaDialogue));
			GameManager.playerAnimator.SetBool("isDoingMissiles",false);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Quizas puedo utilizar \n esto para salir de aqui!", 3f, true, false);
			yield return StartCoroutine(WaitInterruptable (3f,bigPappadaDialogue));
			foreach(Collider collider in bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().fallenRocks.GetComponentsInChildren<Collider>()){
				collider.enabled = false;
			}
			foreach(Collider collider in lightGemGO.GetComponent<SanctuaryLightGem>().rocksGO.GetComponentsInChildren<Collider>()){
				collider.enabled = false;
			}

			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().hideOutsidePlane.SetActive(false);
			GameManager.playerAnimator.SetBool("isChargingSpaceJumping",true);
			yield return new WaitForSeconds(2f);
			float originalForce = GameManager.player.GetComponent<PlayerController>().spaceJumpForce;
			GameManager.player.GetComponent<PlayerController>().spaceJumpForce = 25f;
			GameManager.player.GetComponent<PlayerController>().SpaceJump(GameManager.player.transform.up);
			GUIManager.deactivateSpaceJumpGUI();
			GameManager.player.GetComponent<PlayerController>().spaceJumpForce = originalForce;
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().fallenRocks.GetComponentInChildren<ParticleSystem>().Play();
			lightGemGO.GetComponent<SanctuaryLightGem>().rocksGO.GetComponentInChildren<ParticleSystem>().Play();
			yield return new WaitForSeconds(1f);
			GameManager.player.GetComponent<CharacterController>().Move(1f);
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().fallenRocks.SetActive(false);
			bridgeFallGO.GetComponent<FirstPlanetFallingFromTheBridge>().fallenRocksAfter.SetActive(true);
			lightGemGO.GetComponent<SanctuaryLightGem>().rocksGO.SetActive(false);
			lightGemGO.GetComponent<SanctuaryLightGem>().rocksGOAfter.SetActive(true);
			yield return new WaitForSeconds(1f);
			GameManager.player.GetComponent<PlayerController>().StopMove();
			yield return new WaitForSeconds(2f);
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡Al fin estoy fuera!", 2f, true, false);
			yield return StartCoroutine(WaitInterruptable (2f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡¿Donde esta little G?!", 2f, true, false);
			yield return StartCoroutine(WaitInterruptable (2f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡Tengo que encontrarle!", 2f, true, false);
			yield return StartCoroutine(WaitInterruptable (2f,bigPappadaDialogue));
			bigPappadaDialogue = GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡Primero tengo que \n encontrar una forma \n de vencer la corrupcion!", 4f, true, false);
			yield return StartCoroutine(WaitInterruptable (4f,bigPappadaDialogue));
			GetComponent<PlanetCorruption>().activateSpawning();
			GameManager.inputController.enableInputController();
			
		}
	}


	public override void startButtonPressed(){
		if(isEnabled){
			StartCoroutine("firstCinematic");
		}else{
			GetComponent<PlanetCorruption>().activateSpawning();
		}
	}

	public override void informEventActivated (CutsceneIdentifyier identifyier){
		if(isEnabled){
			if(identifyier.Equals(CutsceneIdentifyier.FirstPlanetBoarHunting)){
				StartCoroutine("secondCinematic");
			}else if(identifyier.Equals(CutsceneIdentifyier.FirstPlanetShintoDoor)){
				StartCoroutine("thirdCinematic");
			}else if(identifyier.Equals(CutsceneIdentifyier.FirstPlanetToTheBridge)){
				StartCoroutine("fourthCinematic");
			}else if(identifyier.Equals(CutsceneIdentifyier.FirstPlanetFallingFromTheBridge)){
				StartCoroutine("fifthCinematic");
			}else if(identifyier.Equals(CutsceneIdentifyier.SanctuaryLightGem)){
				StartCoroutine("sixthCinematic");
			}
		}
	}

	public override void initialize(){
		if(isEnabled){
			GetComponent<PlanetCorruption> ().setCorruptionToClean ();
		}

	}
}

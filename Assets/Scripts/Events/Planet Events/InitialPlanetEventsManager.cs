using UnityEngine;
using System.Collections;

public class InitialPlanetEventsManager : PlanetEventsManager {

	public GameObject bigPappadaInitialPosition;
	public GameObject littleGHopper;

	public GameObject boarHuntingGO;
	public GameObject shintoDoorGO;

	//Is called when the class is activated by the GameTimelineManager
	public override void isActivated(){
		GameManager.player.transform.position = new Vector3(bigPappadaInitialPosition.transform.position.x,bigPappadaInitialPosition.transform.position.y,0f);
		if(isEnabled){
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
		}
		boarHuntingGO.GetComponent<Cutscene>().isActive = false;
		boarHuntingGO.GetComponent<Cutscene> ().Initialize ();
	}

	//This cinematic corresponds to the first cinematic that will play when the play button is pressed
	IEnumerator firstCinematic(){
		if(isEnabled){
			float timer = 0f;
			float time = 0.2f;
			littleGHopper.GetComponent<DialogueController> ().createNewDialogue ("AAaaah..", 4f, true, false);
			littleGHopper.GetComponentInChildren<Animator>().SetBool("isFallingDown",true);
			yield return new WaitForSeconds (1f);
			littleGHopper.GetComponent<SpaceGravityBody> ().setStatic (false);
			float originalZ = littleGHopper.transform.position.z;
			while(timer<time){
				timer+=Time.deltaTime;
				float ratio = timer/time;
				littleGHopper.transform.position = new Vector3(littleGHopper.transform.position.x,littleGHopper.transform.position.y,originalZ*(1f-ratio));
				yield return null;
			}
			yield return new WaitForSeconds (0.7f);
			littleGHopper.GetComponent<CharacterController> ().setOriginalOrientation ();
			littleGHopper.GetComponent<CharacterController> ().LookLeftOrRight (1f);
			yield return new WaitForSeconds (1.3f);
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
				GameManager.player.transform.position = new Vector3(GameManager.player.transform.position.x,GameManager.player.transform.position.y,originalZ*(1f-ratio));
				yield return null;
			}
			yield return new WaitForSeconds (1f);
			littleGHopper.GetComponentInChildren<Animator>().SetBool("isFallingDown",false);
			yield return new WaitForSeconds (1f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Debes concentrar \n tu energia Little G.", 4f, false, false);
			yield return new WaitForSeconds (4f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("El equilibrio no está en el cuerpo,\n está en la mente. ", 4f, false, false);
			yield return new WaitForSeconds (4f);
			littleGHopper.GetComponent<DialogueController> ().createNewDialogue ("Si, maestro ", 3f, false, false);
			yield return new WaitForSeconds (3f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Ya es suficiente entrenamiento \n por hoy. ", 4f, false, false);
			yield return new WaitForSeconds (4f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Ve al templo,\n Yo ire a cazar algo para cenar ", 4f, false, false);
			yield return new WaitForSeconds (4f);
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

	//Cinematic that corresponds to the boar hunting event
	IEnumerator secondCinematic(){
		if(isEnabled){
			boarHuntingGO.GetComponent<FirstPlanetBoarHunting>().makeBoarGoAway();
			GameManager.inputController.disableInputController ();
			GameManager.player.GetComponent<PlayerController>().StopMove();
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Un Jabali!", 2f, false, false);
			yield return new WaitForSeconds(0.5f);
			GameManager.inputController.enableInputController ();
		}
	}

	//Cinematic that corresponds to the third cinematic
	IEnumerator thirdCinematic(){
		if(isEnabled){
			shintoDoorGO.GetComponent<Cutscene>().isActive = false;
			boarHuntingGO.GetComponent<Cutscene>().isActive = false;
			boarHuntingGO.GetComponent<FirstPlanetBoarHunting>().boar.SetActive(false);
			GameManager.inputController.disableInputController ();
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Que raro... ", 1.5f, false, false);
			yield return new WaitForSeconds(1.5f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Nunca habia visto \n los sellos brillar de esta forma", 4f, false, false);
			yield return new WaitForSeconds(4f);
			GetComponent<PlanetCorruption>().corrupt();
			GetComponent<PlanetCorruption>().activateSpawning();
			yield return new WaitForSeconds(2f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("Waaah!!!", 1f, true, false);
			yield return new WaitForSeconds(1f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("¡¿Que es estoo?!", 1f, true, false);
			yield return new WaitForSeconds(1f);
			GameManager.player.GetComponent<DialogueController> ().createNewDialogue ("El templo!! Little G.!", 2f, true, false);
			yield return new WaitForSeconds(2f);
			GameManager.inputController.enableInputController ();
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
			if(identifyier.Equals(CutsceneIdentifyier.SanctuaryLightGem)){
				GetComponent<PlanetCorruption>().corrupt();
				GetComponent<PlanetCorruption>().activateSpawning();
			}else if(identifyier.Equals(CutsceneIdentifyier.FirstPlanetBoarHunting)){
				StartCoroutine("secondCinematic");
			}else if(identifyier.Equals(CutsceneIdentifyier.FirstPlanetShintoDoor)){
				StartCoroutine("thirdCinematic");
			}
		}
	}

	public override void initialize(){
		if(isEnabled){
			GetComponent<PlanetCorruption> ().setCorruptionToClean ();
		}

	}
}

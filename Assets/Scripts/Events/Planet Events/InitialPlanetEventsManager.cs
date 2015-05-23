using UnityEngine;
using System.Collections;

public class InitialPlanetEventsManager : PlanetEventsManager {

	public GameObject bigPappadaInitialPosition;
	public GameObject littleGInitialPosition;
	public GameObject littleGHopper;

	public override void isActivated(){
		GameManager.player.transform.position = new Vector3(bigPappadaInitialPosition.transform.position.x,bigPappadaInitialPosition.transform.position.y,0f);
		if(isEnabled){
			//First event is putting Big P in the initial position
			GameManager.player.transform.position = bigPappadaInitialPosition.transform.position;
			GameManager.playerSpaceBody.setStatic (true);
			GameManager.player.transform.rotation = Quaternion.LookRotation (Vector3.forward*-1f, bigPappadaInitialPosition.transform.up*-1f);
			//GameManager.playerAnimator.SetBool ("isChargingSpaceJumping", true);
			GameManager.playerSpaceBody.bindToClosestPlanet ();
			GameManager.inputController.disableInputController ();
			littleGHopper.GetComponent<SpaceGravityBody> ().setStatic (true);
			littleGHopper.GetComponent<SpaceGravityBody> ().bindToClosestPlanet ();
			littleGHopper.GetComponent<CharacterController> ().StopMoving ();
			littleGHopper.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		}
	}

	IEnumerator firstCinematic(){
		if(isEnabled){
			float timer = 0f;
			float time = 0.2f;

			littleGHopper.GetComponentInChildren<Animator>().SetTrigger("isFallingDown");
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
			//littleGHopper.transform.forward = new Vector3(-1f,0f,0f);
			littleGHopper.GetComponent<CharacterController> ().setOriginalOrientation ();
			littleGHopper.GetComponent<CharacterController> ().LookLeftOrRight (1f);
			yield return new WaitForSeconds (1.3f);
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

			timer = 0f;
			originalZ = GameManager.player.transform.position.z;
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
			littleGHopper.GetComponent<CharacterController> ().Move (-1f);
			littleGHopper.GetComponentInChildren<Animator> ().SetBool ("isWalking", true);
			GameManager.inputController.enableInputController ();

			littleGHopper.GetComponent<DialogueController> ().createNewDialogue ("Que te jodan! ", 3f, false, false);
			yield return new WaitForSeconds (7f);
			littleGHopper.GetComponent<CharacterController> ().StopMoving ();
			littleGHopper.GetComponentInChildren<Animator> ().SetBool ("isWalking", false);
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
			}
		}
	}

	public override void initialize(){
		if(isEnabled){
			GetComponent<PlanetCorruption> ().setCorruptionToClean ();
		}

	}
}

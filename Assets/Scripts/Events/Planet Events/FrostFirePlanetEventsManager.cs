using UnityEngine;
using System.Collections;

public class FrostFirePlanetEventsManager : PlanetEventsManager {

	private GameObject bigPappadaDialogue;

	private DialogueController bigPappadaDialogueController;

	public Checkpoint planetCheckpoint;
	public GameObject corruptionBlockade;
	public GameObject penguinAttackEvent;
	public GameObject hydraAttackEvent;

	public GameObject burningCore;
	public GameObject[] platforms;
	public GameObject rotatingFire;

	public float objectiveGrowingScale = 1.75f;
	private Vector3 startingScale;
	private Vector3 objectiveScale;
	private Quaternion startingFireRotation;


	private bool hydraEventCinematicFinished = false;
	private bool hydraEventCinematicOngoing = false;

	private bool hasPlayedOnLandCinematic = false;
	private bool hasBeenAttackedByPenguins = false;

	private bool onGoingRunnerCinematic;
	
	public override void informEventActivated (CutsceneIdentifyier identifyier){
		if(identifyier.Equals(CutsceneIdentifyier.FrostFirePlanetPenguinAttack)){
			if(!hasBeenAttackedByPenguins){
				penguinAttackEvent.SetActive(false);
				GetComponent<PlanetSpawnerManager> ().enabled = true;
				hasBeenAttackedByPenguins = true;
			}
		}else if(identifyier.Equals(CutsceneIdentifyier.FrostFirePlanetHydraAppearence)){
			if(!hydraEventCinematicFinished && !hydraEventCinematicOngoing){
				hydraEventCinematicOngoing = true;
				startHydraCombat();
			}
		}
	}

	private void movePlatforms(float speed){
		foreach(GameObject platform in platforms){
			Vector3 platformPosition = platform.GetComponent<Renderer>().bounds.center;
			Vector3 direction = platformPosition - transform.position;
			platform.transform.position += direction.normalized * speed * Time.deltaTime;
		}
	}

	private IEnumerator doRunningEvent(){
		float timeItLasts = 15f;
		float timer = 0f;
		rotatingFire.SetActive (true);
		rotatingFire.GetComponent<RotateAroundItself> ().rotationPerSecond = new Vector3 (0f, 0f,-8f);
		while(timer<3f){
			timer+=Time.deltaTime;
			burningCore.transform.localScale += Time.deltaTime *Vector3.one * 0.01f;
			yield return null;
		}

		while(timer<timeItLasts){
			timer+=Time.deltaTime;
			movePlatforms(0.6f);
			burningCore.transform.localScale += Time.deltaTime *Vector3.one * 0.04f;
			yield return null;
		}
		rotatingFire.GetComponent<RotateAroundItself> ().rotationPerSecond = new Vector3 (0f, 0f,-14f);

		timer = 0f;
		while (timer<10f) {
			timer+=Time.deltaTime;
			yield return null;
		}
		timer = 0f;
		while(timer<40f){
			timer+=Time.deltaTime;
			burningCore.transform.localScale += Time.deltaTime *Vector3.one * 0.02f;
			yield return null;
		}

		rotatingFire.GetComponent<RotateAroundItself> ().rotationPerSecond = new Vector3 (0f, 0f,0f);
	}

	private void cleanHydraCombat(){

	}

	private void startHydraCombat(){
		//Temporarily there is no hydra
		StartCoroutine (doRunningEvent ());
	}

	public override void initialize (){
		if(isEnabled){
			GetComponent<PlanetSpawnerManager> ().enabled = false;
			bigPappadaDialogueController = GameManager.player.GetComponent<DialogueController>();
			corruptionBlockade.SetActive(true);
			objectiveScale = new Vector3 (objectiveGrowingScale, objectiveGrowingScale, objectiveGrowingScale);
			startingScale = new Vector3 (1f, 1f, 1f);
			rotatingFire.SetActive(false);
			startingFireRotation = rotatingFire.transform.rotation;
		}else{
			corruptionBlockade.SetActive(false);
		}
	}

	private IEnumerator onLandCinematic(){
		if(!hasPlayedOnLandCinematic){
			while(GameManager.playerController.getIsSpaceJumping()){
				yield return null;
			}
			GameManager.persistentData.playerLastCheckpoint = planetCheckpoint.checkPointIndex;
			GameManager.inputController.disableInputController ();
			yield return new WaitForSeconds(1.5f);

			bigPappadaDialogue = bigPappadaDialogueController.createNewDialogue ("Tsk...", 1f, false, false);
			yield return StartCoroutine (WaitInterruptable (1f, bigPappadaDialogue));

			bigPappadaDialogue = bigPappadaDialogueController.createNewDialogue ("Another corrupted planet...", 2f, false, false);
			yield return StartCoroutine (WaitInterruptable (2f, bigPappadaDialogue));

			bigPappadaDialogue = bigPappadaDialogueController.createNewDialogue ("I must find a way to cleanse it!", 2f, false, false);
			yield return StartCoroutine (WaitInterruptable (2f, bigPappadaDialogue));
			
			GameManager.inputController.enableInputController ();
			hasPlayedOnLandCinematic = true;
		}
	}

	public override void planetCleansed(){
		if(isEnabled){
			corruptionBlockade.SetActive(false);
		}
	}
	
	public override void isActivated (){
		StartCoroutine (onLandCinematic ());
	}

	public override void isDeactivated (){

	}

	public override void playerDies (){
		if(hydraEventCinematicOngoing){

		}
	}




}

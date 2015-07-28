using UnityEngine;
using System.Collections;

public class FrostFirePlanetEventsManager : PlanetEventsManager {

	private GameObject bigPappadaDialogue;

	private DialogueController bigPappadaDialogueController;

	public Checkpoint planetCheckpoint;
	public GameObject corruptionBlockade;
	public GameObject penguinAttackEvent;

	private bool hasPlayedOnLandCinematic = false;
	private bool hasBeenAttackedByPenguins = false;
	
	public override void informEventActivated (CutsceneIdentifyier identifyier){
		if(identifyier.Equals(CutsceneIdentifyier.FrostFirePlanetPenguinAttack)){
			if(!hasBeenAttackedByPenguins){
				penguinAttackEvent.SetActive(false);
				GetComponent<PlanetSpawnerManager> ().enabled = true;
				hasBeenAttackedByPenguins = true;
			}
		}
	}

	public override void initialize (){
		if(isEnabled){
			GetComponent<PlanetSpawnerManager> ().enabled = false;
			bigPappadaDialogueController = GameManager.player.GetComponent<DialogueController>();
			corruptionBlockade.SetActive(true);
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




}

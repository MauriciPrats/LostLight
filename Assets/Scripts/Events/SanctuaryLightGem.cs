using UnityEngine;
using System.Collections;

public class SanctuaryLightGem : Cutscene {

	public override void Initialize() {
		identifyier = CutsceneIdentifyier.SanctuaryLightGem;
	}

	public override void ActivateTrigger() {
		CharacterController characterController = GameManager.player.GetComponent<CharacterController>();
		GameManager.player.GetComponent<DialogueController>().createNewDialogue("Jodo! Una lightgem!",2f,false,false);
	}
}

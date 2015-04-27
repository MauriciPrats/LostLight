using UnityEngine;
using System.Collections;

public class SanctuaryLightGem : Cutscene {
	
	public override void ActivateTrigger() {
		CharacterController characterController = GameManager.player.GetComponent<CharacterController>();
		characterController.Jump (20);
	}
}

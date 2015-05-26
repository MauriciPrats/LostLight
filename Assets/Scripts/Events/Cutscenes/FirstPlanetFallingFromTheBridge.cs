using UnityEngine;
using System.Collections;

public class FirstPlanetFallingFromTheBridge : Cutscene {

	public GameObject bridge; 

	public override void Initialize() {
		identifyier = CutsceneIdentifyier.FirstPlanetFallingFromTheBridge;
	}

	public override void ActivateTrigger() {
	}
}

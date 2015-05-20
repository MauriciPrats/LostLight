using UnityEngine;
using System.Collections;

public enum CutsceneIdentifyier {SanctuaryLightGem};

abstract public class Cutscene: MonoBehaviour  {

	public enum EndMode { ByTime, Inheritance };

	public EndMode endMode; 
	public float endTime;
	public bool isActive = true;
	public LayerMask collisionMask;
	public GameObject planetEventsManagerGO;


	public abstract void ActivateTrigger();
	public abstract void Initialize();
	private bool running = false;
	private PlanetEventsManager planetEventsManager;
	protected CutsceneIdentifyier identifyier;

	void Start(){
		planetEventsManager = planetEventsManagerGO.GetComponent<PlanetEventsManager> ();
	}

	void OnTriggerEnter(Collider other) {
		if (isActive && !running){
			planetEventsManager.informEventActivated(identifyier);
			running = true;
			if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
				StartCutscene ();
				if (endMode == EndMode.ByTime) {
					StartCoroutine ("checkTime");
				}
				ActivateTrigger ();
			}
		}
	}


	void StartCutscene() {
		//Stop all player input. 
		if (endMode == EndMode.ByTime) {
			GameManager.inputController.disableInputController();
		}
	}

	void EndCutScene(){
		GameManager.inputController.enableInputController();
	}

	IEnumerator checkTime() {
		yield return new WaitForSeconds(endTime);
		running = false;
		EndCutScene();
	}
	


}

using UnityEngine;
using System.Collections;

abstract public class Cutscene: MonoBehaviour  {

	public enum EndMode { ByTime, Inheritance };

	public EndMode endMode; 
	public float endTime;
	public bool isActive;
	public LayerMask collisionMask;

	public abstract void ActivateTrigger();
	private bool running;

	void OnTriggerEnter(Collider other) {
		if (isActive && !running){
			running = true;
			//TODO: we should check this with a layer collision, not hard coded. 
			if(other.gameObject.name == "BigPDef") {
				Debug.Log(other.gameObject.layer);
				InitializeCutscene ();
				if (endMode == EndMode.ByTime) {
					StartCoroutine ("checkTime");
				}
				ActivateTrigger ();
			}
		}
	}

	void InitializeCutscene() {
		//Stop all player input. 
		GameObject player = GameManager.player;
		player.GetComponent<InputController> ().enabled = false;
		player.GetComponent<CharacterController> ().StopMoving ();	
		player.GetComponent<PlayerController> ().StopMove ();
	}

	void EndCutScene(){
		GameObject player = GameManager.player;
		player.GetComponent<InputController> ().enabled = true;
	}

	IEnumerator checkTime() {
		yield return new WaitForSeconds(endTime);
		running = false;
		EndCutScene();
	}
	


}

using UnityEngine;
using System.Collections;

abstract public class Cutscene: MonoBehaviour  {

	public enum EndMode { ByTime, Inheritance };

	public EndMode endMode; 
	public float endTime;
	public bool isActive = true;
	public LayerMask collisionMask;

	public abstract void ActivateTrigger();
	private bool running = false;

	void OnTriggerEnter(Collider other) {
		if (isActive && !running){
			running = true;
			if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
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
		if (endMode == EndMode.ByTime) {
			player.GetComponent<InputController> ().enabled = false;
		}
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

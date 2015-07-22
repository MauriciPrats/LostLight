using UnityEngine;
using System.Collections;

public class BreathingSpace : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.tag.Equals("Player")){
			GameManager.player.GetComponent<PlayerController>().setCanDrownInSpace(false);
		}
	}
	void OnTriggerExit(Collider collider){
		if(collider.gameObject.tag.Equals("Player")){
			GameManager.player.GetComponent<PlayerController>().setCanDrownInSpace(true);
		}
	}
}

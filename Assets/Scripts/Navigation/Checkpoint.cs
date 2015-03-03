using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public int checkPointIndex;

	public GameObject checkPointsManager;

	void Awake(){
		checkPointsManager.GetComponent<CheckpointManager> ().registerCheckpoint (gameObject,checkPointIndex);
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Player"){
			GameManager.gameState.playerLastCheckpoint = checkPointIndex;
			Debug.Log("New Checkpoint: "+checkPointIndex);
		}
	}
}

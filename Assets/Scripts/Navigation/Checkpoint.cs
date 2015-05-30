using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public int checkPointIndex;
	public GameObject checkPointsManager;
	public GameObject planetGO;
	private Planet planet;

	void Awake(){
		checkPointsManager.GetComponent<CheckpointManager> ().registerCheckpoint (gameObject,checkPointIndex);
	}

	void Start(){
		planet = planetGO.GetComponent<Planet> ();
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Player"){
			GameManager.gameState.playerLastCheckpoint = checkPointIndex;
		}
	}


	public Planet Planet {
		get {
			return planet;
		}
	}
}

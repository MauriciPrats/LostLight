using UnityEngine;
using System.Collections;

public class CorruptionPlane : MonoBehaviour {

	public float speed = 3f;
	public float distance = 45f; 
	private float distanceGone = 0f;
	
	// Update is called once per frame
	void Update () {
		distanceGone += speed * Time.deltaTime;
		if(distanceGone>=distance){
			Destroy(gameObject);
		}
		transform.position += transform.up * speed * Time.deltaTime;
	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag == "Player"){
			GameManager.playerController.kill();
		}
	}
}

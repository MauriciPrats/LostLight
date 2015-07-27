using UnityEngine;
using System.Collections;

public class DieOnTouchSpace : MonoBehaviour {

	void OnCollisionEnter (Collision collider) {
		if(collider.gameObject.tag == "Player"){
			collider.gameObject.transform.GetComponent<PlayerController>().dieInSpace();
		}
	}

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject.tag == "Player"){
			collider.gameObject.transform.GetComponent<PlayerController>().dieInSpace();
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}

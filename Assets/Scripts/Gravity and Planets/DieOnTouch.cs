using UnityEngine;
using System.Collections;

public class DieOnTouch : MonoBehaviour {


	void OnCollisionEnter (Collision collider) {
		if(collider.gameObject.tag == "Player"){
			collider.gameObject.transform.GetComponent<PlayerController>().kill();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

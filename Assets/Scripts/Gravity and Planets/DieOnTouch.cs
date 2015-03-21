using UnityEngine;
using System.Collections;

public class DieOnTouch : MonoBehaviour {


	void OnCollisionEnter (Collision collider) {
		Debug.Log ("aaa");
		if(collider.gameObject.tag == "Player"){
			Debug.Log(collider.gameObject.tag);
			collider.gameObject.transform.GetComponent<CharacterController>().kill();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

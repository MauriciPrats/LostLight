using UnityEngine;
using System.Collections;

public class DieOnTouchSpace : MonoBehaviour {

	private float cooldown = 0.1f;
	private float timer = 0f;

	void OnCollisionEnter (Collision collider) {
		if(collider.gameObject.tag == "Player" && timer>cooldown){
			timer = 0f;
			collider.gameObject.transform.GetComponent<PlayerController>().dieInSpace();
		}
	}

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject.tag == "Player" && timer>cooldown){
			timer = 0f;
			collider.gameObject.transform.GetComponent<PlayerController>().dieInSpace();
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

	}
}

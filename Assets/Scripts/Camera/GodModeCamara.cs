using UnityEngine;
using System.Collections;

public class GodModeCamara : MonoBehaviour {
	
	public bool areYouGod = false;
	public float wheelspeed = 5f;
	public float godspeed = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Period)) {
			if (areYouGod) {
				areYouGod = false;
				this.gameObject.GetComponent<CameraFollowingPlayer> ().enabled = true;
				GameManager.player.GetComponent<InputController> ().enabled = true;
			} else {
				areYouGod = true;
				this.gameObject.GetComponent<CameraFollowingPlayer> ().enabled = false;
				GameManager.player.GetComponent<InputController> ().enabled = false;
			}
			
		}
		
		
		if (areYouGod) {
			float xmove = Input.GetAxis ("Horizontal");
			float ymove = Input.GetAxis ("Vertical");
			float zmove = Input.GetAxis ("Mouse ScrollWheel");
			//TODO: Restore the correct position in Z
			this.transform.position += new Vector3 (xmove * godspeed, ymove * godspeed, zmove * wheelspeed);
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				transform.position = new Vector3(0.0f,0.0f,transform.position.z);
			}
			
			
			
		}
		
		
	}
}

using UnityEngine;
using System.Collections;

public class CameraFollowingPlayer : MonoBehaviour {

	public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GravityBody playerGravityBody = player.GetComponent<GravityBody> ();
		if (playerGravityBody.getIsTouchingPlanet ()) {
			Vector3 objectiveUp = new Vector3(player.transform.up.x,player.transform.up.y,player.transform.up.z); ;
			Vector3 newUpPosition = Vector3.Lerp (transform.up, objectiveUp, Constants.CAMERA_ANGLE_FOLLOWING_SPEED * Time.deltaTime);

			transform.up = newUpPosition;
		}
		Vector3 objectivePosition = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);

		transform.position = objectivePosition;
	}
}

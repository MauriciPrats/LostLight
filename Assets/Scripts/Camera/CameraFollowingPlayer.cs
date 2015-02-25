using UnityEngine;
using System.Collections;

public class CameraFollowingPlayer : MonoBehaviour {
	
	//For debugging purposes
	public Vector3 newUp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		if (!playerGravityBody.getUsesSpaceGravity()) {
			Vector3 objectiveUp = new Vector3(GameManager.player.transform.up.x,GameManager.player.transform.up.y,GameManager.player.transform.up.z);
			Vector3 newUpPosition = Vector3.Lerp (transform.up, objectiveUp, Constants.CAMERA_ANGLE_FOLLOWING_SPEED * Time.deltaTime);

			//Debug.Log(objectiveUp);
			transform.up = newUpPosition;
		}
		//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 0f, transform.eulerAngles.z);
		Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y, transform.position.z);

		objectivePosition += transform.up*1.2f;
		

		transform.position = objectivePosition;
		
		//transform.up = Quaternion.Euler(0, -45, 0) * transform.up;
		//newUp = transform.up;
		
		
	}
}




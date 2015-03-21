using UnityEngine;
using System.Collections;

public class CameraFollowingPlayer : MonoBehaviour {
	
	//For debugging purposes
	public Vector3 newUp;
	public float spaceJumpZ;

	private float objectiveZ;
	private float originalZ;


	void Awake(){
		GameManager.registerMainCamera (gameObject);
	}

	// Use this for initialization
	void Start () {
		originalZ = transform.position.z;
		objectiveZ = originalZ;
	}

	void updatePosition(){
		
		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		if (!playerGravityBody.getUsesSpaceGravity()) {
			Vector3 objectiveUp = new Vector3(GameManager.player.transform.up.x,GameManager.player.transform.up.y,GameManager.player.transform.up.z);
			Vector3 newUpPosition = Vector3.Lerp (transform.up, objectiveUp, Constants.CAMERA_ANGLE_FOLLOWING_SPEED * Time.deltaTime);
			
			//Debug.Log(objectiveUp);
			transform.up = newUpPosition;
		}
		//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 0f, transform.eulerAngles.z);
		Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y, transform.position.z);
		//objectivePosition = Vector3.Lerp (transform.position, objectivePosition, Time.deltaTime);
		objectivePosition += transform.up*1.2f;

		//We make a lerp to the new Z
		Vector3 objectivePositionZ = new Vector3 (objectivePosition.x, objectivePosition.y, objectiveZ);
		objectivePosition = Vector3.Lerp(objectivePosition,objectivePositionZ,Time.fixedDeltaTime);

		transform.position = objectivePosition;
		transform.eulerAngles = new Vector3 (0f, 0f, transform.eulerAngles.z);



	}

	public void returnOriginalZ(){
		setObjectiveZ (originalZ);
	}

	public void setObjectiveZ(float newZ){
		objectiveZ = newZ;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(GameManager.gameState.isCameraLockedToPlayer){
			updatePosition ();
		}
	}

	public void resetPosition(){
		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		Vector3 objectiveUp = new Vector3(GameManager.player.transform.up.x,GameManager.player.transform.up.y,GameManager.player.transform.up.z);
		transform.up = objectiveUp;
		
		Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y, transform.position.z);
		objectivePosition += transform.up*1.2f;
		transform.position = objectivePosition;
	}
}




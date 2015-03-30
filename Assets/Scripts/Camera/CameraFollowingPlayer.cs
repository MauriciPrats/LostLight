using UnityEngine;
using System.Collections;

public class CameraFollowingPlayer : MonoBehaviour {


	public float upMultiplyierWithAngle = 2.5f;
	public float upMultiplyierWithoutAngle = 1.2f;
	public float lerpMultiplyierZPosition = 4f;
	public float minimumUpDistanceOnStartLerpingXAngle = 1f;
	public float xAngle = 21f;
	public float lerpMultiplyierXAngle = 0.25f;
	public float lerpMultiplyierUp = 4.5f;
	public float lerpMultiplyierPos = 5f;
	private float objectiveZ;
	private float originalZ;

	float timer = 0f;
	float timerZPosition = 0f;


	void Awake(){
		GameManager.registerMainCamera (gameObject);
	}

	// Use this for initialization
	void Start () {
		originalZ = transform.position.z;
		objectiveZ = originalZ;
	}

	void updatePosition(){
		timerZPosition += Time.deltaTime;
		Vector3 objectiveUp = new Vector3(GameManager.player.transform.up.x,GameManager.player.transform.up.y,0f).normalized;
		Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y,transform.position.z);
		Vector3 objectiveVectorZ = new Vector3 (objectivePosition.x, objectivePosition.y, objectiveZ);
		objectivePosition = Vector3.Lerp (objectivePosition, objectiveVectorZ, timerZPosition * lerpMultiplyierZPosition );

		Vector3 rightWithoutZ = new Vector3 (transform.right.x, transform.right.y, 0f).normalized;

		//Modifying objective up
		if (!GameManager.player.GetComponent<CharacterController> ().getIsSpaceJumping () && !GameManager.playerAnimator.GetBool ("isChargingSpaceJumping") && !GameManager.gameState.isInsidePlanet) {
			if(Vector3.Distance(objectiveUp,transform.up)<=minimumUpDistanceOnStartLerpingXAngle){

			Vector3 objectiveUpRotated = (Quaternion.AngleAxis (xAngle, rightWithoutZ) * objectiveUp);
			timer+=Time.deltaTime;
				objectiveUp = Vector3.Lerp(objectiveUp,objectiveUpRotated,timer * lerpMultiplyierXAngle);
			}
			objectivePosition += GameManager.player.transform.up*upMultiplyierWithAngle;
		}else{
			timer = 0f;
			objectivePosition += GameManager.player.transform.up*upMultiplyierWithoutAngle;
		}

		Vector3 newUp;
		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		if (!playerGravityBody.getUsesSpaceGravity()) {
			newUp = Vector3.Lerp (transform.up, objectiveUp,Time.deltaTime * lerpMultiplyierUp);
		}else{
			newUp = transform.up;
		}
		Vector3 newForward = Quaternion.AngleAxis(90,rightWithoutZ) * newUp;
		transform.rotation = Quaternion.LookRotation(newForward,newUp);
		transform.position = Vector3.Lerp (transform.position, objectivePosition, Time.deltaTime * lerpMultiplyierPos);

	}

	/*void updatePosition(){

		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y, transform.position.z);
		if (!playerGravityBody.getUsesSpaceGravity()) {
			Vector3 objectiveUp = new Vector3(GameManager.player.transform.up.x,GameManager.player.transform.up.y,0f).normalized;
			if(!GameManager.player.GetComponent<CharacterController>().getIsSpaceJumping() && !GameManager.playerAnimator.GetBool("isChargingSpaceJumping")){
				//Vector3 objectiveUpRight = Vector3.Cross(objectiveUp,Vector3.right);
				objectivePosition += playerGravityBody.transform.up*1.2f;
				//objectiveUp = (Quaternion.AngleAxis(30,transform.right) * objectiveUp);
			}
			Vector3 newUpPosition = Vector3.Lerp (transform.up, objectiveUp, Constants.CAMERA_ANGLE_FOLLOWING_SPEED * Time.deltaTime);

			//Debug.Log(objectiveUp);
			transform.up = newUpPosition;
			//transform.localRotation = new Quaternion(0f,0f,GameManager.player.transform.rotation.z,GameManager.player.transform.rotation.w) * Quaternion.AngleAxis(30,transform.right);
		}
		//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 0f, transform.eulerAngles.z);
		//Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y, transform.position.z);
		//objectivePosition = Vector3.Lerp (transform.position, objectivePosition, Time.deltaTime);
		//objectivePosition += transform.up*2f;

		//We make a lerp to the new Z
		Vector3 objectivePositionZ = new Vector3 (objectivePosition.x, objectivePosition.y, objectiveZ);
		objectivePosition = Vector3.Lerp(objectivePosition,objectivePositionZ,Time.fixedDeltaTime);

		transform.position = objectivePosition;
		//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 0f, transform.eulerAngles.z);

		//Incline the camera
		if(!GameManager.player.GetComponent<CharacterController>().getIsSpaceJumping() && !GameManager.playerAnimator.GetBool("isChargingSpaceJumping")){
			//transform.RotateAround (transform.position, transform.right, 20f);
		}
		//Quaternion.angle
		//transform.LookAt (GameManager.player.transform.position);

	}*/

	public void returnOriginalZ(){
		setObjectiveZ (originalZ);
	}

	public void setObjectiveZ(float newZ){
		timerZPosition = 0f;
		objectiveZ = newZ;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GameManager.gameState.isCameraLockedToPlayer){
			updatePosition ();
		}
	}

	public void resetPosition(){
		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		Vector3 objectiveUp = new Vector3(GameManager.player.transform.up.x,GameManager.player.transform.up.y,0f);
		transform.up = objectiveUp;
		
		Vector3 objectivePosition = new Vector3 (GameManager.player.transform.position.x, GameManager.player.transform.position.y, transform.position.z);
		objectivePosition += transform.up*1.2f;
		transform.position = objectivePosition;
	}
}




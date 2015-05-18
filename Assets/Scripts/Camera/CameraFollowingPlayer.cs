using UnityEngine;
using System.Collections;

public class CameraFollowingPlayer : MonoBehaviour {


	public float distanceCameraOnSpaceJump = 100;
	public float distanceCameraOnCleansePlanet = 40;
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

	public GameObject objective;
	private bool followingObjective;

	float timer = 0f;
	float timerZPosition = 0f;


	void Awake(){
		GameManager.registerMainCamera (gameObject);
	}

	// Use this for initialization
	void Start () {
		originalZ = transform.position.z;
		objectiveZ = originalZ;
		followingObjective = false;

	}

	void updatePosition(){
		if(objective==null){
			objective = GameManager.player;
		}
		timerZPosition += Time.deltaTime;
		Vector3 objectiveUp;
		Vector3 objectivePosition;
		Vector3 objectiveVectorZ;
		if(!followingObjective){
			objectiveUp = new Vector3(objective.transform.up.x,objective.transform.up.y,0f).normalized;
			objectivePosition = new Vector3 (objective.transform.position.x, objective.transform.position.y,transform.position.z);
			objectiveVectorZ = new Vector3 (objectivePosition.x, objectivePosition.y, objectiveZ);
		}else{
			objectiveUp =  transform.up.normalized;
			objectivePosition = new Vector3 (objective.transform.position.x, objective.transform.position.y,transform.position.z);
			objectiveVectorZ = new Vector3 (objectivePosition.x, objectivePosition.y, objectiveZ);
		}
			objectivePosition = Vector3.Lerp (objectivePosition, objectiveVectorZ, timerZPosition * lerpMultiplyierZPosition );

			Vector3 rightWithoutZ = new Vector3 (transform.right.x, transform.right.y, 0f).normalized;

			//Modifying objective up
			if (!GameManager.player.GetComponent<PlayerController> ().getIsSpaceJumping () && !GameManager.playerAnimator.GetBool ("isChargingSpaceJumping") && !GameManager.gameState.isInsidePlanet) {
				if(Vector3.Distance(objectiveUp,transform.up)<=minimumUpDistanceOnStartLerpingXAngle){

				Vector3 objectiveUpRotated = (Quaternion.AngleAxis (xAngle, rightWithoutZ) * objectiveUp);
				timer+=Time.deltaTime;
					objectiveUp = Vector3.Lerp(objectiveUp,objectiveUpRotated,timer * lerpMultiplyierXAngle);
				}
				objectivePosition += objective.transform.up*upMultiplyierWithAngle;
			}else{
				timer = 0f;
				objectivePosition += objective.transform.up*upMultiplyierWithoutAngle;
			}

			Vector3 newUp;
			SpaceGravityBody playerGravityBody = GameManager.player.GetComponent<SpaceGravityBody> ();
			if (!playerGravityBody.getUsesSpaceGravity()) {
				newUp = Vector3.Lerp (transform.up, objectiveUp,Time.deltaTime * lerpMultiplyierUp);
			}else{
				newUp = transform.up;
			}
			Vector3 newForward = Quaternion.AngleAxis(90,rightWithoutZ) * newUp;
			transform.rotation = Quaternion.LookRotation(newForward,newUp);
			transform.position = Vector3.Lerp (transform.position, objectivePosition, Time.deltaTime * lerpMultiplyierPos);

		float zProportion = Mathf.Abs (transform.position.z - originalZ) / Mathf.Abs (distanceCameraOnSpaceJump - originalZ);
		GameManager.setGrassPorcentualLevel (zProportion);
	}

	public void returnOriginalZ(){
		timerZPosition = 0f;
		objectiveZ = originalZ;
	}

	public void setObjectiveZCameraOnSpaceJump(){
		timerZPosition = 0f;
		objectiveZ = -distanceCameraOnSpaceJump;
	}

	public void setObjectiveZCameraCleansePlanet(){
		timerZPosition = 0f;
		objectiveZ = -distanceCameraOnCleansePlanet;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GameManager.gameState.isCameraLockedToPlayer){
			updatePosition ();
		}
	}

	public void resetPosition(){
		if(objective==null){
			objective = GameManager.player;
		}
		GravityBody playerGravityBody = GameManager.player.GetComponent<GravityBody> ();
		Vector3 objectiveUp = new Vector3(objective.transform.up.x,objective.transform.up.y,0f);
		transform.up = objectiveUp;
		
		Vector3 objectivePosition = new Vector3 (objective.transform.position.x, objective.transform.position.y, transform.position.z);
		objectivePosition += transform.up*1.2f;
		transform.position = objectivePosition;
	}

	public void followObjective(GameObject objectiveGO){
		objective = objectiveGO;
		//followingObjective = true;
	}

	public void resetObjective(){
		objective = GameManager.player;
		//followingObjective = true;
	}

	public void unfollowObjective(){
		objective = null;
		followingObjective = false;
	}

}




using UnityEngine;
using System.Collections;

[RequireComponent (typeof (WalkOnMultiplePaths))]
public class IAController : MonoBehaviour {


	public float minimumDistanceSeePlayer = 50f;
	public LayerMask layersToFindCollision;
	public float speed = 2f;
	public float cooldownChangeBehaviour = 0.1f;
	public float timePatroling = 3f;
	public float jumpCooldown = 2f;
	public float jumpStrength = 10f;


	private float minimumDistanceFront = 0f;
	private float timeToChangeBehaviour = 0f;
	private float timeToJump = 0f;

	public bool isIdle = true;

	private bool isBlockedBySomethingInFront = false;
	private Vector3 moveAmount;

	private bool isLookingRight = true;

	private bool isWalkingRight = true;

	private float timeWalkingDirectionIdle = 0f;
	
	private GameObject player;

	private float lastTimeCheckedClosestThingInFront = 0f;
	private float cooldownRaycastingClosestThingInFront = 0.2f;
	private float closestThingInFront = 0f;

	private WalkOnMultiplePaths walkOnMultiplePaths;

	// Use this for initialization
	void Start () {
		player = GameManager.player;
		moveAmount = new Vector3 (0f, 0f, 0f);
		transform.forward = new Vector3(1f,0f,0f);
		minimumDistanceFront = (Random.value)*0.2f + 5f;

		walkOnMultiplePaths = GetComponent<WalkOnMultiplePaths> ();

	}

	private bool canSeePlayer(){
		Vector3 playerDirection = player.transform.rigidbody.worldCenterOfMass - transform.position;
		if(playerDirection.magnitude<minimumDistanceSeePlayer){
			RaycastHit hit;

			if (Physics.Raycast(rigidbody.worldCenterOfMass,playerDirection, out hit, minimumDistanceSeePlayer,layersToFindCollision))
			{
				//Debug.Log(hit.collider.transform.name +" "+playerDirection);
				Collider target = hit.collider; // What did I hit?
				//Debug.Log(target.name);
				if(target.tag == "Player"){ 
					return true;
				}
				else{
					return false; 
				}
			}
		}

		return false;
	}

	public bool isPlayerLeft(){
		float angle = Mathf.DeltaAngle(Mathf.Atan2(player.transform.up.y, player.transform.up.x) * Mathf.Rad2Deg,Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg);

		if(angle>0f){
			return false;
		}else{
			return true;
		}
		return true;
	}


	private float closestThingInFrontDistance(){
		lastTimeCheckedClosestThingInFront += Time.deltaTime;
		if(lastTimeCheckedClosestThingInFront>cooldownRaycastingClosestThingInFront){
			lastTimeCheckedClosestThingInFront = 0f;
			RaycastHit hit;
			if (Physics.Raycast(rigidbody.worldCenterOfMass,transform.forward, out hit))
			{
				Collider target = hit.collider; // What did I hit?
				closestThingInFront = hit.distance;
			}else{
				closestThingInFront = float.PositiveInfinity;
			}
		}
		return closestThingInFront;

	}

	private void offensiveMoves(){
		if(isPlayerLeft()){
			turnLeft();
		}else{
			turnRight();
		}
		walk ();

		jump ();
	}

	private void idleWalking(){
		timeWalkingDirectionIdle += Time.deltaTime;
		if(timeWalkingDirectionIdle>timePatroling || isBlockedBySomethingInFront){
			isWalkingRight = !isWalkingRight;
			timeWalkingDirectionIdle = 0f;
		}

		if(isWalkingRight){
			turnRight();
		}else{
			turnLeft();
		}
		walk ();
	}

	private void turnRight(){
		if(!isLookingRight){
			transform.Rotate(0f,180f,0f);
			isLookingRight = true;
		}
	}

	private void walk(){
		if(closestThingInFrontDistance() > minimumDistanceFront){
			if(isLookingRight){
				moveAmount = (speed) * -this.transform.right;
			}else if(!isLookingRight){
				moveAmount = (speed) * this.transform.right;
			}
			isBlockedBySomethingInFront = false;
		}else{
			isBlockedBySomethingInFront  = true;
		}
	}
	private void turnLeft(){
		if(isLookingRight){
			transform.Rotate(0f,180f,0f);
			isLookingRight = false;
		}
	}

	private void jump(){
		if(timeToJump>jumpCooldown){
			rigidbody.AddForce(transform.up * jumpStrength,ForceMode.Impulse);
			timeToJump = 0f+Random.value;
		}
	}

	private void attack(){

	}

	private void updateTimers(){
		timeToJump = timeToJump + Time.deltaTime;
		timeToChangeBehaviour = timeToChangeBehaviour + Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		updateTimers ();

		moveAmount = new Vector3 (0f, 0f, 0f);

		if(!walkOnMultiplePaths.getIsChangingPath()){
			if(timeToChangeBehaviour>cooldownChangeBehaviour){
				timeToChangeBehaviour = 0f;
				if(canSeePlayer ()){
					isIdle = false;
				}else{
					isIdle = true;
				}
			}

			if(isIdle){
				idleWalking();
			}else{
				offensiveMoves();
			}
		}

	}

	void FixedUpdate(){
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
	}
}

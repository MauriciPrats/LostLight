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
	public bool isIdle = true;

	private Animator iaAnimator;
	private float minimumDistanceFront = 0f;
	private float timeToChangeBehaviour = 0.1f;
	private float timeToJump = 0f;
	private bool isBlockedBySomethingInFront = false;
	private Vector3 moveAmount;
	private bool isLookingRight = true;
	private bool isWalkingRight = true;
	private float timeWalkingDirectionIdle = 0f;
	private GameObject player;
	private float lastTimeCheckedClosestThingInFront = 0f;
	private float cooldownRaycastingClosestThingInFront = 0.1f;
	private float closestThingInFront = 0f;

	private WalkOnMultiplePaths walkOnMultiplePaths;

	// Use this for initialization
	void Start () {
		iaAnimator = GetComponentInChildren<Animator> ();
		player = GameManager.player;
		moveAmount = new Vector3 (0f, 0f, 0f);
		transform.forward = new Vector3(1f,0f,0f);
		minimumDistanceFront = ((Random.value)*0.2f) + 0.2f;
		//Debug.Log (minimumDistanceFront);

		walkOnMultiplePaths = GetComponent<WalkOnMultiplePaths> ();

	}

	private bool canSeePlayer(){
		Vector3 playerDirection = player.rigidbody.worldCenterOfMass - transform.position;
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

	public bool isElementLeft(GameObject element){
		if(Util.getPlanetaryAngleFromAToB(gameObject,element)>0f){
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

			float enemyDistanceFront = walkOnMultiplePaths.getClosestEnemyInFront ();
			CharacterController chaCon = player.GetComponent<CharacterController>();
			float playerDistance = Vector3.Distance (player.rigidbody.worldCenterOfMass, transform.position) - (walkOnMultiplePaths.centerToExtremesDistance + chaCon.centerToExtremesDistance);


			if(playerDistance<enemyDistanceFront){
				closestThingInFront = playerDistance;
			}else{
				closestThingInFront = enemyDistanceFront;
			}
			//Debug.Log ("E: "+enemyDistanceFront);
			//Debug.Log ("P: "+playerDistance);
			//Debug.Log ("C: "+closestThingInFront);
		}
		return closestThingInFront;
		/*if(lastTimeCheckedClosestThingInFront>cooldownRaycastingClosestThingInFront){
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
		return closestThingInFront;*/

	}

	private void offensiveMoves(){
		//Check how far away is the player

		//If it's far away, approach him

		//If it's close, check with the attack budgeter

			//Random if it wants to attack

			//If you can attack, then:
				
				//Charge attack

				//Do attack

		//If it's not close continue with approaching


		if(isElementLeft(player)){

			turnLeft();
		}else{
			turnRight();
		}
		walk ();

		//jump ();
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
			iaAnimator.SetBool("isWalking",true);
		}else{
			isBlockedBySomethingInFront  = true;
			iaAnimator.SetBool("isWalking",false);
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

	void FixedUpdate(){
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
	}

	public bool getIsLookingRight(){
		return isLookingRight;
	}
}

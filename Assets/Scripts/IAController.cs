using UnityEngine;
using System.Collections;

public class IAController : MonoBehaviour {


	public float minimumDistanceSeePlayer = 30f;
	public LayerMask layersToFindCollision;
	public LayerMask layersToFindCollisionEnemies;
	public float speed = 2f;
	public float cooldownChangeBehaviour = 0.1f;
	public float timePatroling = 3f;
	public float jumpCooldown = 2f;
	public float jumpStrength = 10f;


	private float minimumDistanceFront = 0f;
	private float timeToChangeBehaviour = 0f;
	private float timeToJump = 0f;
	public bool isFrontPath = true;

	public bool isIdle = true;

	private bool isBlockedBySomethingInFront = false;
	private Vector3 moveAmount;

	private bool isLookingRight = true;

	private bool isWalkingRight = true;

	private float timeWalkingDirectionIdle = 0f;
	
	private GameObject player;
	
	private bool isChangingPath = false;
	private float changingTimer = 0f;

	private float lastTimeCheckedClosestThingInFront = 0f;
	private float cooldownRaycastingClosestThingInFront = 0.1f;
	private float closestThingInFront = 0f;

	private float lastTimeCheckedOtherPaths ;
	private float cooldownRaycastingCheckOtherPaths = 1f;
	// Use this for initialization
	void Start () {
		player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		moveAmount = new Vector3 (0f, 0f, 0f);
		transform.forward = new Vector3(1f,0f,0f);
		changePath ();
		minimumDistanceFront = (Random.value)*0.2f + 0.4f;
		lastTimeCheckedOtherPaths = Random.value * cooldownRaycastingCheckOtherPaths;
	}

	private bool canSeePlayer(){
		Vector3 playerDirection = player.transform.position - transform.position;
		if(playerDirection.magnitude<minimumDistanceSeePlayer){
			RaycastHit hit;

			if (Physics.Raycast(transform.position,playerDirection, out hit, minimumDistanceSeePlayer,layersToFindCollision))
			{
				Collider target = hit.collider; // What did I hit?
				//Debug.Log(target.name);
				if(target.tag == "Player"){ return true;}
				else{ return false; }
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

	public bool IsFrontPath(){
		return isFrontPath;
	}

	public void changePath(){
		changingTimer = 1f;
		isChangingPath = false;
		if (isFrontPath) {transform.position = new Vector3(transform.position.x,transform.position.y,Constants.FRONT_PATH_Z_OFFSET);}
		else{transform.position = new Vector3(transform.position.x,transform.position.y,Constants.BACK_PATH_Z_OFFSET);}
		/*changingTimer += Time.deltaTime;
		if(changingTimer>=1f){changingTimer = 1f; isChangingPath = false;}
		Vector3 frontPath = new Vector3(transform.localPosition.x,transform.localPosition.y,Constants.FRONT_PATH_Z_OFFSET);
		Vector3 backPath = new Vector3(transform.localPosition.x,transform.localPosition.y,Constants.BACK_PATH_Z_OFFSET);
		//Debug.Log("changiing")
		if(isFrontPath){
			transform.localPosition = Vector3.Lerp(backPath,frontPath,changingTimer);
		}else{
			transform.localPosition = Vector3.Lerp(frontPath,backPath,changingTimer);
		}*/

	}

	public int ammountOfEnemiesToPlayer(bool isFrontPath,int jumps){
		jumps += 1;
		//TODO: find out how many enemies are in this path between him and the player
		if(jumps<5){
			RaycastHit hit;
			Vector3 position;
			if(isFrontPath){position = new Vector3(transform.position.x,transform.position.y,Constants.FRONT_PATH_Z_OFFSET);}
			else{position = new Vector3(transform.position.x,transform.position.y,Constants.BACK_PATH_Z_OFFSET);}
			if (Physics.Raycast(position,transform.forward, out hit, minimumDistanceSeePlayer,layersToFindCollisionEnemies))
			{
				Collider target = hit.collider; // What did I hit?
				//Debug.Log(target.name);
				if(target.tag != "Enemy"){ return 0;}
				else if(target.tag == "Enemy"){ 
					IAController controller = target.gameObject.GetComponent<IAController>();
					return controller.ammountOfEnemiesToPlayer(isFrontPath,jumps)+1;
				}
			}
		}
		return 0;
	}

	private bool hasToChangePath(){
		lastTimeCheckedOtherPaths += Time.deltaTime;
		if(lastTimeCheckedOtherPaths>cooldownRaycastingCheckOtherPaths){
			lastTimeCheckedOtherPaths = Random.value*0.1f;
			int frontEnemies = ammountOfEnemiesToPlayer (true,0);
			int backEnemies = ammountOfEnemiesToPlayer (false,0);

			Debug.Log("F: "+frontEnemies+" B: "+backEnemies);

			if(isFrontPath && backEnemies<frontEnemies){
				return true;
			}else if(!isFrontPath && backEnemies>frontEnemies){
				return true;
			}
			return false;
		}else{
			return false;
		}
	}

	private float closestThingInFrontDistance(){
		lastTimeCheckedClosestThingInFront += Time.deltaTime;
		if(lastTimeCheckedClosestThingInFront>cooldownRaycastingClosestThingInFront){
			lastTimeCheckedClosestThingInFront = 0f;
			RaycastHit hit;
			if (Physics.Raycast(transform.position,transform.forward, out hit))
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

		if(isChangingPath){
			changePath();
		}
		if(!isChangingPath){
			if(hasToChangePath()){
				isFrontPath = !isFrontPath;
				isChangingPath = true;
			}

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

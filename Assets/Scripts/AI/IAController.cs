using UnityEngine;
using System.Collections;

[RequireComponent (typeof (WalkOnMultiplePaths))]
public class IAController : MonoBehaviour {


	public float minimumDistanceSeePlayer = 50f;
	public LayerMask layersToFindCollision;
	public float cooldownChangeBehaviour = 0.1f;
	public float timePatroling = 3f;
	public float jumpCooldown = 2f;
	public float jumpStrength = 10f;
	public bool isIdle = true;
	public float minimumDistanceAttackPlayer = 1f;
	public float attackChoosingCooldown = 0.5f;
	public float attackChance = 0.2f;
	public GameObject[] shortRangeAttacks;
	public GameObject[] middleRangeAttacks;
	public GameObject[] longRangeAttacks;
	public GameObject onHitEffect;
	public GameObject secondOnHitEffect;
	public GameObject thirdOnHitEffect;
	public GameObject onDeathEffect;

	public GameObject onDeathLight;
	public int numberOfLightsAvg = 2;

	private bool isStunned;
	private float stunnedTime;
	private float stunnedTimer;
	private float timeToDie = 0.5f;

	private CharacterController characterController;
	private Animator iaAnimator;
	private float minimumDistanceFront = 0f;
	private float timeToChangeBehaviour = 0.1f;
	private float timeToJump = 0f;
	private bool isBlockedBySomethingInFront = false;
	private bool isWalkingRight = true;
	private float timeWalkingDirectionIdle = 0f;
	private GameObject player;
	private float lastTimeCheckedClosestThingInFront = 0f;
	private float cooldownRaycastingClosestThingInFront = 0.1f;
	private float closestThingInFront = 0f;
	private float attackTimer = 0f;
	private bool isDoingAttack = false;
	private BaseAttack actualAttack;
	private bool isDead;
	private float timeHasBeenDead;
	private WalkOnMultiplePaths walkOnMultiplePaths;

	private GameObject[] hitParticles;

	// Use this for initialization
	void Start () {
		iaAnimator = GetComponentInChildren<Animator> ();
		player = GameManager.player;
		characterController = GetComponent<CharacterController> ();
		transform.forward = new Vector3(1f,0f,0f);
		minimumDistanceFront = ((Random.value)*0.2f) + 0.2f;
		//Debug.Log (minimumDistanceFront);
		isDead = false;
		timeHasBeenDead = 0f;
		walkOnMultiplePaths = GetComponent<WalkOnMultiplePaths> ();

		//Particle Inicialization
		hitParticles = new GameObject[3];
		
		hitParticles[0] = GameObject.Instantiate (onHitEffect) as GameObject;
		hitParticles[1] = GameObject.Instantiate (secondOnHitEffect) as GameObject;
		hitParticles[2] = GameObject.Instantiate (thirdOnHitEffect) as GameObject;
		
		foreach (GameObject particles in hitParticles) {
			particles.transform.parent = gameObject.transform;
		}
		
		//baseAttack = GetComponent<BaseAttack> ();
	}

	private bool canSeePlayer(){
		Vector3 playerDirection = player.GetComponent<Rigidbody>().worldCenterOfMass - GetComponent<Rigidbody>().worldCenterOfMass;
		if(playerDirection.magnitude<minimumDistanceSeePlayer){
			RaycastHit hit;
			if (Physics.Raycast(GetComponent<Rigidbody>().worldCenterOfMass,playerDirection, out hit, minimumDistanceSeePlayer,layersToFindCollision))
			{

				Collider target = hit.collider; // What did I hit?
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
			PlayerController chaCon = player.GetComponent<PlayerController>();
			float playerDistance = Vector3.Distance (player.GetComponent<Rigidbody>().worldCenterOfMass, transform.position) - (walkOnMultiplePaths.centerToExtremesDistance + chaCon.centerToExtremesDistance);


			if(playerDistance<enemyDistanceFront){
				closestThingInFront = playerDistance;
			}else{
				closestThingInFront = enemyDistanceFront;
			}
		}
		return closestThingInFront;

	}

	private void offensiveMoves(){

		if(isDoingAttack){
			if(actualAttack.isAttackFinished()){
				isDoingAttack = false;
			}else{
				actualAttack.doAttack();
			}
		}else{
			//Check how far away is the player
			attackTimer += Time.deltaTime;

			float distanceToPlayer = Vector3.Distance (transform.GetComponent<Rigidbody>().worldCenterOfMass, player.GetComponent<Rigidbody>().worldCenterOfMass);
			distanceToPlayer -= (player.GetComponent<PlayerController> ().centerToExtremesDistance + walkOnMultiplePaths.centerToExtremesDistance);

			if(distanceToPlayer<= minimumDistanceAttackPlayer){
				if(attackTimer>= attackChoosingCooldown){
					attackTimer = 0f;
					if(Random.value<attackChance){
						//If we actually want to attack

						//Choose the actual attack
						GameObject attack = shortRangeAttacks[Random.Range(0,shortRangeAttacks.Length)];
						BaseAttack bAttack = attack.GetComponent<BaseAttack>();

						if(GameManager.enemyAttackManager.askForNewAttack(bAttack.getAttackValue())){
							isDoingAttack = true;
							actualAttack = bAttack;
							actualAttack.startAttack();
						}
					}else{
						offensiveMovement();
					}
				}else{
					offensiveMovement();
				}
			}else{
				offensiveMovement();
			}
		}
	}

	private void offensiveMovement(){
		float moveDirection = 0f;
		if(isElementLeft(player)){
			moveDirection = -1f;
		}else{
			moveDirection = 1f;
		}
		characterController.LookLeftOrRight (moveDirection);

		if(!isStunned){
			walk (moveDirection);
		}else{
			//Stunned animation
			//idleWalking();
		}
	}

	private void idleWalking(){
		timeWalkingDirectionIdle += Time.deltaTime;
		if(timeWalkingDirectionIdle>timePatroling || isBlockedBySomethingInFront){
			isWalkingRight = !isWalkingRight;
			timeWalkingDirectionIdle = 0f;
		}

		float moveDirection = 0f;
		if(isWalkingRight){
			moveDirection = 1f;
		}else{
			moveDirection = -1f;
		}
		characterController.LookLeftOrRight (moveDirection);
		walk (moveDirection);
	}



	private void walk(float moveDirection){
		if(GetComponent<GravityBody>().getIsTouchingPlanet()){
			if(closestThingInFrontDistance() > minimumDistanceFront){
				characterController.Move(moveDirection);
				isBlockedBySomethingInFront = false;
				iaAnimator.SetBool("isWalking",true);
			}else{
				isBlockedBySomethingInFront  = true;
				iaAnimator.SetBool("isWalking",false);
			}
		}else{
			iaAnimator.SetBool("isWalking",false);
		}
	}

	private void jump(){
		if(timeToJump>jumpCooldown){
			GetComponent<Rigidbody>().AddForce(transform.up * jumpStrength,ForceMode.Impulse);
			timeToJump = 0f+Random.value;
		}
	}

	private void updateTimers(){
		timeToJump = timeToJump + Time.deltaTime;
		timeToChangeBehaviour = timeToChangeBehaviour + Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		updateTimers ();
		if (isDead) {
			timeHasBeenDead+=Time.deltaTime;
			//float ratio = timeHasBeenDead/timeToDie;
			//transform.localScale = new Vector3(0f,transform.localScale.y * (1f-ratio),0f);
			//transform.GetComponent<Rigidbody>().centerOfMass = originalCenterOfMass;
			//if(timeHasBeenDead>=(timeToDie/2f)){
				//GetComponent<Collider>().enabled = false;
				//transform.localScale = transform.localScale +new Vector3(0.1f,0.1f,0.1f);
			//}
			if(timeHasBeenDead>=timeToDie ){
				onDeath();
				Destroy(gameObject);
			}
		}else{
			if(isStunned){
				stunnedTimer+=Time.deltaTime;
				if(stunnedTimer>=stunnedTime){
					isStunned = false;
				}
			}

			characterController.StopMoving();
			
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

			if(walkOnMultiplePaths.getIsChangingPath()){
				iaAnimator.SetBool("isWalking",true);
			}
		}
	}

	public bool getIsLookingRight(){
		return characterController.getIsLookingRight();
	}

	public void stun(float timeStunned){
		stunnedTimer = 0f;
		stunnedTime = timeStunned;
		isStunned = true;
	}

	public void getHurt(int hurtAmmount,Vector3 hitPosition){
		//Play hurt effects
		//Particles
		
		foreach (GameObject particles in hitParticles) {
			particles.GetComponent<ParticleSystem>().Play();
			particles.transform.position = hitPosition + (transform.up * 0.2f);
		}
		
		GetComponent<Killable> ().Damage (hurtAmmount);
		iaAnimator.SetTrigger("isHurt");
		if(GetComponent<Killable>().isDead()){
			//Play on death effects and despawn
			//Animation and lots of particles
			if(!isDead){
				timeHasBeenDead = 0f;
				iaAnimator.SetTrigger("Die");
				GameObject particlesOnDeath = GameObject.Instantiate (onDeathEffect) as GameObject;
				particlesOnDeath.transform.position = GetComponent<Rigidbody>().worldCenterOfMass;
				particlesOnDeath.transform.parent = transform;
			}
			iaAnimator.SetBool("isWalking",false);
			iaAnimator.SetBool("isChargingAttack",false);
			iaAnimator.SetBool("isDoingAttack",false);
			isDead = true;
		}
	}

	private void onDeath(){
		Vector3 centerBoar = GetComponent<Rigidbody> ().worldCenterOfMass;
		int numberLights = numberOfLightsAvg + Random.Range (-1, 1);
		for(int i = 0;i<numberLights;i++){
			GameObject newLight = GameObject.Instantiate(onDeathLight) as GameObject;
			newLight.transform.position = (centerBoar);
			newLight.GetComponent<LightOnDeath>().setVectorUp(transform.up);
			int randRGB = UnityEngine.Random.Range(0,3);
			Color color = new Color(1f,1f,1f);
			float complementary = 1f;
			float mainColor = 0.85f;
			if(randRGB==0){
				color = new Color(mainColor,complementary,complementary);
			}else if(randRGB==1){
				color = new Color(complementary,mainColor,complementary);
			}else{
				color = new Color(complementary,complementary,mainColor);
			}
			newLight.GetComponent<TrailRenderer>().material.color = color;
		}

	}
}

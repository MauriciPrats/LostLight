using UnityEngine;
using System.Collections;

public enum EnemyType{Jabali,BigJabali,DarkPappada,None}

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
	public float timeToChangeBehaviour = 0.1f;

	//Private variables for being frozen, dying,juimping and checking things in front
	private float frozenTime;
	private float frozenTimer;

	private float stunnedTime;
	private float stunnedTimer;

	private float timeToDie = 0.5f;
	private float minimumDistanceFront = 0f;
	private float timeToJump = 0f;
	private float lastTimeCheckedClosestThingInFront = 0f;
	private float cooldownRaycastingClosestThingInFront = 0.1f;
	private float timeHasBeenDead;

	//Elements connected to the AI
	protected CharacterController characterController;
	protected Animator iaAnimator;
	protected WalkOnMultiplePaths walkOnMultiplePaths;
	protected CharacterAttackController attackController;
	protected GameObject player;

	//State of the AI
	protected bool isBlockedBySomethingInFront = false;
	protected bool isWalkingRight = true;
	protected float closestThingInFront = 0f;
	protected bool isDoingAttack = false;
	protected bool isDead;
	protected bool isOnGuard;
	protected bool isFrozen;
	protected bool isStunned;

	private GameObject[] hitParticles;

	private bool hasBeenInitialized = false;
	// Use this for initialization
	void Start () {
		attackController = GetComponent<CharacterAttackController> ();
		isOnGuard = false;
		iaAnimator = GetComponentInChildren<Animator> ();
		player = GameManager.player;
		characterController = GetComponent<CharacterController> ();
		transform.forward = new Vector3(1f,0f,0f);
		minimumDistanceFront = ((Random.value)*0.2f) + 0.2f;

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
	}

	protected virtual void initialize(){

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

	protected virtual void offensiveMoves(){

	}

	protected virtual void idleWalking(){
		
	}







	protected void walk(float moveDirection){
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

	protected void jump(){
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
		if(!hasBeenInitialized){
			initialize();
			hasBeenInitialized = true;
		}
		updateTimers ();
		if (isDead) {
			timeHasBeenDead+=Time.deltaTime;
			if(timeHasBeenDead>=timeToDie ){
				onDeath();
				Destroy(gameObject);
			}
		}else{
			if(isFrozen){
				frozenTimer+=Time.deltaTime;
				if(frozenTimer>=frozenTime){
					isFrozen = false;
				}
			}
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

	public void freeze(float timeFrozen){
		frozenTimer = 0f;
		frozenTime = timeFrozen;
		isFrozen = true;
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
		
		GetComponent<Killable> ().TakeDamage (hurtAmmount);
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
		int numberLights = numberOfLightsAvg;
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

	public void interruptAttack(){
		attackController.interruptActualAttacks ();
	}


	public void breakGuard(){

	}

	public Animator getIAAnimator(){
		return iaAnimator;
	}

}

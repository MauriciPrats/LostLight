using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum EnemyType{Jabali,BigJabali,Rat,Monkey,None}

[RequireComponent (typeof (WalkOnMultiplePaths))]
public class IAController : MonoBehaviour {

	//GameObjects
	public GameObject onHitEffect;
	public GameObject secondOnHitEffect;
	public GameObject thirdOnHitEffect;
	public GameObject onDeathEffect;
	public GameObject onDeathLight;

	//Public Variables
	public float minimumDistanceSeePlayer = 50f;
	public LayerMask layersToFindCollision;
	public float timePatroling = 3f;
	public float jumpCooldown = 2f;
	public float jumpStrength = 10f;
	public bool isIdle = true;
	public float minimumDistanceAttackPlayer = 1f;
	public float meleeRange = 1f; 
	public float attackChoosingCooldown = 0.5f;
	public float attackChance = 0.2f;
	public int numberOfLightsAvg = 2;
	public float timeToChangeBehaviour = 0.1f;
	public float timeToDie = 0.5f;

	//Private Variables
	private float frozenTime;
	private float frozenTimer;
	private float stunnedTime;
	private float stunnedTimer;

	private float minimumDistanceFront = 0f;
	private float timeToJump = 0f;
	private float lastTimeCheckedClosestThingInFront = 0f;
	private float cooldownRaycastingClosestThingInFront = 0.1f;
	private GameObject[] hitParticles;
	private bool hasBeenInitialized = false;

	//Protected Variables to be used by all the AI
	protected CharacterController characterController;
	protected Animator iaAnimator;
	protected WalkOnMultiplePaths walkOnMultiplePaths;
	protected CharacterAttackController attackController;
	protected GameObject player;

	//State of the AI
	protected float closestThingInFront = 0f;
	public bool isDead;
	protected bool isOnGuard;
	protected bool isFrozen;
	protected bool isStunned;


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

	protected float closestThingInFrontDistance(){
		lastTimeCheckedClosestThingInFront += Time.deltaTime;
		if(lastTimeCheckedClosestThingInFront>cooldownRaycastingClosestThingInFront){
			lastTimeCheckedClosestThingInFront = 0f;

			//Enemy distance
			float enemyDistanceFront = walkOnMultiplePaths.getClosestEnemyInFront ();

			//Player distance
			PlayerController chaCon = player.GetComponent<PlayerController>();
			float playerDistance = Vector3.Distance (player.GetComponent<Rigidbody>().worldCenterOfMass, transform.position) - (walkOnMultiplePaths.centerToExtremesDistance + chaCon.centerToExtremesDistance);

			//terrain elements distance
			float terrainDistance = float.PositiveInfinity;
			RaycastHit hit;
			if (Physics.Raycast(GetComponent<Rigidbody>().worldCenterOfMass,transform.forward, out hit, 100000f,layersToFindCollision))
			{
				Collider target = hit.collider; // What did I hit?
				if(target.gameObject.layer == LayerMask.NameToLayer("Planets")){ 
					terrainDistance = hit.distance;
				}
			}

			if(!getIsLookingRight() == isElementLeft(player)){
				if(playerDistance<enemyDistanceFront){
					closestThingInFront = playerDistance;
				}else{
					closestThingInFront = enemyDistanceFront;
				}
			}else{
				closestThingInFront = enemyDistanceFront;
			}

			if(terrainDistance<closestThingInFront){
				closestThingInFront = terrainDistance;
			}
		}
		return closestThingInFront;
	}

	//Functions to override
	protected virtual void initialize(){
	}

	protected virtual void UpdateAI(){
	}

	//FUNCTIONS FOR MOVING
	public void Move(float moveDirection){
		if(!isDead){
			characterController.Move(moveDirection);
			iaAnimator.SetBool("isWalking",true);
		}
	}

	protected void StopMoving(){
		characterController.Move(0f);
		iaAnimator.SetBool("isWalking",false);
	}

	protected void Jump(){
		characterController.Jump (jumpStrength);
	}

	protected bool getIsTouchingPlanet(){
		return GetComponent<GravityBody> ().getIsTouchingPlanet ();
	}

	//FUNCTIONS FOR GETTING INFORMATION
	protected bool canSeePlayer(){
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

	protected float getPlayerDistance(){
		PlayerController chaCon = player.GetComponent<PlayerController>();
		return Vector3.Distance (player.GetComponent<Rigidbody>().worldCenterOfMass, transform.position) - (walkOnMultiplePaths.centerToExtremesDistance + chaCon.centerToExtremesDistance);
	}

	protected float getPlayerDirection(){
		if(isElementLeft(player)){
			return -1f;
		}else{
			return 1f;
		}
	}

	protected List<GameObject> getListCloseAllies(){
		return walkOnMultiplePaths.getListCloseAllies();
	}

	protected bool getIsBlockedBySomethingInFront(){
		if(closestThingInFrontDistance()<minimumDistanceFront){
			return true;
		}else{
			return false;
		}
	}

	protected float getLookingDirection(){
		if(getIsLookingRight()){
			return 1f;
		}else{
			return -1f;
		}
	}

	protected void lookAtDirection(float direction){
		characterController.LookLeftOrRight (direction);
	}
	
	public bool getIsLookingRight(){
		return characterController.getIsLookingRight();
	}


	//BASE AI FUNCTIONS
	public bool isElementLeft(GameObject element){
		if(Util.getPlanetaryAngleFromAToB(gameObject,element)>0f){
			return false;
		}else{
			return true;
		}
		return true;
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
		if(getIsTouchingPlanet()){
			characterController.stopJumping();
		}
		UpdateAI ();
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
		if(!isDead){
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
					interruptAttack();
					iaAnimator.SetTrigger("Die");
					GameObject particlesOnDeath = GameObject.Instantiate (onDeathEffect) as GameObject;
					particlesOnDeath.transform.position = GetComponent<Rigidbody>().worldCenterOfMass;
					particlesOnDeath.transform.parent = transform;
					gameObject.layer = LayerMask.NameToLayer("OnlyFloor");
					StopMoving();
					StartCoroutine("disappearOnDeath");
				}
				iaAnimator.SetBool("isWalking",false);
				isDead = true;
			}
		}
	}
	IEnumerator disappearOnDeath(){
		float timeHasBeenDead = 0f;
		while(timeHasBeenDead<timeToDie){
			timeHasBeenDead+=Time.deltaTime;
			float ratio = timeHasBeenDead/timeToDie;
			GetComponent<Dissolve>().setDisolution(1f-ratio);
			yield return null;
		}
		onDeath();
		Destroy(gameObject);
	}

	private void onDeath(){
		Vector3 center = GetComponent<Rigidbody> ().worldCenterOfMass;
		int numberLights = numberOfLightsAvg;
		for(int i = 0;i<numberLights;i++){
			GameObject newLight = GameObject.Instantiate(onDeathLight) as GameObject;
			newLight.transform.position = (center);
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
		//Unused
	}

	public Animator getIAAnimator(){
		return iaAnimator;
	}

	protected void SetVisionRange(float value) {
		minimumDistanceSeePlayer = value;
	}

	protected void SetMeleeRange(float value) {
		meleeRange = value;
	}

	protected bool isAtVisionRange(){
		if (getPlayerDistance () < minimumDistanceSeePlayer) {
			return true;
		} else {
			return false;
		}
	}

	protected bool isAtMeleeRange(){
		if (getPlayerDistance () < meleeRange) {
			return true;
		} else {
			return false;
		}
	}


	//Functions and variables for the AI
	//UpdateAI
	//Move
	//Jump
	//canSeePlayer()
	//getIsBlockedBySomethingInFront()
	//getPlayerDistance
	//closestThingInFront()



}

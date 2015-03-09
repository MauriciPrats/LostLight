using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {

	public float moveSpeed = 5f;
	public float normalJumpForce = 10f;
	public float spaceJumpForce = 100f;
	public GameObject particleSystemJumpCharge;
	public GameObject animationBigPappada;
	public GameObject breathingBubble;
	public float timeBetweenDamageWhenNotBreathing = 0.5f;
	public int damageWhenNotBreathing = 1;
	public float minimumBreathingBubbleScale = 6f;
	public float maximumBreathingBubbleScale = 1f;
	//Weapon stuff
	public Transform leftHand; //Exponer la mano izquierda de Big Pappada para ponerle su arma
	public GameObject weapon;
	//End weapon stuff
	public float timeToDieInSpace = 4f;

	public float centerToExtremesDistance = 0f;
	public float extraSafeDistanceFromEnemies = 0.3f;
	public bool isInvulnerable = false;

	public GameObject pappada;


	private Animator bpAnimator;
	private CharacterAttackController cAttackController;
	private bool isAttacking = false;
	private GravityBody body;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;
	private float timeJumpPressed;
	private bool isMoving;
	private bool isLookingRight = true;
	private float timeSinceAttackStarted = 0f;
	private bool isJumping = false;
	private float jumpedTimer = 0f;
	private float jumpedTimerCooldown = 0.2f;
	private bool isGoingUp = false;
	private bool isOutsideAthmosphere;
	private float timeHasBeenInSpace = 0f;
	private Killable killable;
	private float timeHasNotBeenBreathing;
	private PappadaController pappadaC;


	void Awake(){
		GameManager.registerPlayer (gameObject);
		
		GameObject stick = (GameObject)Instantiate(weapon);
		stick.transform.parent = leftHand.transform;
		stick.transform.position = leftHand.transform.position;

	}

	void Start () {

		body = GetComponent<GravityBody> ();
		killable = GetComponent<Killable> ();
		GameObject attack = GameObject.Find("skillAttack");
		transform.forward = new Vector3(1f,0f,0f);
		cAttackController = GetComponent<CharacterAttackController>();
		bpAnimator = animationBigPappada.GetComponent<Animator>();
		pappadaC = pappada.GetComponent<PappadaController> ();

		initializeVariables ();
		
	}

	private void initializeVariables(){
		timeJumpPressed = 0;
		timeSinceAttackStarted = 0f;
		isJumping = false;
		jumpedTimer = 0f;
		jumpedTimerCooldown = 0.2f;
		isGoingUp = false;
		timeHasNotBeenBreathing = timeBetweenDamageWhenNotBreathing;
		timeHasBeenInSpace = 0f;
		centerToExtremesDistance = (animationBigPappada.collider.bounds.size.z /2f)+extraSafeDistanceFromEnemies;
		isInvulnerable = false;
		rigidbody.velocity = new Vector3 (0f, 0f, 0f);

		//Initialize the animator
		if(bpAnimator!=null){
			bpAnimator.SetBool("isJumping",false);
			bpAnimator.SetBool("isSpaceJumping",false);
			bpAnimator.SetBool("isGoingUp",false);
			bpAnimator.SetBool("isChargingSpaceJumping",false);
			bpAnimator.SetBool("isWalking",false);
		}
	}


	void Update() {
		if(isJumping){
			jumpedTimer +=Time.deltaTime;
		}
		if(body.getIsTouchingPlanet() && jumpedTimer >=jumpedTimerCooldown){
			bpAnimator.SetBool("isJumping",false);
			bpAnimator.SetBool("isSpaceJumping",false);
		}else{

		}
		if(isJumping){
			bool isGoingUp = Vector3.Angle(transform.up,rigidbody.velocity.normalized)<90;
			if (isGoingUp) {
				bpAnimator.SetBool("isGoingUp",true);
			}else{
				bpAnimator.SetBool("isGoingUp",false);
			}
		}


		if(body.getIsOutsideAthmosphere()){
			breathingBubble.SetActive(true);
			if(!GameManager.gameState.isGameEnded){
				if(timeHasBeenInSpace>=timeToDieInSpace){
					breathingBubble.transform.localScale = new Vector3(0f,0f,0f);
					timeHasNotBeenBreathing+=Time.deltaTime;
					if(timeHasNotBeenBreathing>=timeBetweenDamageWhenNotBreathing){
						getHurt(damageWhenNotBreathing);
						timeHasNotBeenBreathing = 0f;
					}
				}else{
					timeHasBeenInSpace += Time.deltaTime;
					float ratio = 1f - (timeHasBeenInSpace/timeToDieInSpace);
					float newScale = ((maximumBreathingBubbleScale - minimumBreathingBubbleScale) * ratio)+minimumBreathingBubbleScale;
					breathingBubble.transform.localScale = new Vector3(newScale,newScale,newScale);
					//GUIManager.setFadedOutScreen(timeHasBeenInSpace/timeToDieInSpace);
				}
			}

		}else{
			if(!GameManager.gameState.isGameEnded){
				timeHasBeenInSpace = 0f;
				breathingBubble.SetActive(false);
				//GUIManager.resetFadeScreen();
			}
		}
	}

	void FixedUpdate(){
		//Changed because the other way gave me some errors (Maurici)
		//rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		//this.rigidbody.MovePosition (newPosition);
		//rigidbody.velocity = rigidbody.velocity + movement;
		this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		//rigidbody.MovePosition (newPosition);
	}

	public void StartAttack() {
		cAttackController.ChargeAttack(isLookingRight,this.transform);
	}
//Legacy, ahora es solo el ataque por StartAttack
	public void Attack() { 
	//	cAttackController.Attack();
	}

	public void SpaceJump() {
		rigidbody.AddForce (transform.up * spaceJumpForce, ForceMode.Impulse);
		//If we jump into the space, stop the particle system.
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isSpaceJumping",true);
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		isJumping = true;
		jumpedTimer = 0f;
	}

	public void Jump() {
		rigidbody.AddForce (transform.up * normalJumpForce, ForceMode.VelocityChange);
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		bpAnimator.SetBool("isJumping",true);
		isJumping = true;
		jumpedTimer = 0f;
	}

	public void Move() {
		bpAnimator.SetBool("isWalking",true);
		isMoving = true;
		if (!body.getUsesSpaceGravity()) {
			float inputHorizontal = Input.GetAxisRaw ("Horizontal");

			ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
			particles.Stop ();
			bpAnimator.SetBool("isChargingSpaceJumping",false);

			moveAmount = (moveSpeed * inputHorizontal) * -this.transform.right;

			//If we change the character looking direction we change the characters orientation and we invert the z angle
			if (inputHorizontal > 0f) {
				if(!isLookingRight){
					transform.Rotate(0f,180f,0f);
					//transform.eulerAngles = new Vector3(transform.localEulerAngles.x,90f,transform.localEulerAngles.z);
					isLookingRight = true;
				}
			}else if(inputHorizontal<0f){
				if(isLookingRight){
					transform.Rotate(0f,180f,0f);
					//transform.eulerAngles = new Vector3(transform.localEulerAngles.x,-90f,transform.localEulerAngles.z);
					isLookingRight = false;
				}
			}
		}
	}

	public void StopMove() {
		bpAnimator.SetBool("isWalking",false);

		isMoving = false;
	}

	public void StopAttack() {
		isAttacking = false;
	}

	public void ChargeJump() {
		bpAnimator.SetBool("isChargingSpaceJumping",true);
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Play ();
	}

	public void getHurt(int hitPointsToSubstract){
		if(!isInvulnerable){
			GUIManager.getHurtEffect ();
			killable.HP -= hitPointsToSubstract;
			pappadaC.newProportionOfLife(killable.proportionHP());
			if(killable.HP<=0){
				GameManager.loseGame();
			}
		}
	}

	public void reset(){
		if(killable!=null){
			killable.resetHP ();
			pappadaC.newProportionOfLife(killable.proportionHP());
		}
		initializeVariables ();
	}

}

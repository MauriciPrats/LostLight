﻿using UnityEngine;
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

	//SpaceJump line
	public float lineJumpDistance;
	private LineRenderer lineRenderer;
	private bool isShowingLineJump;
	private Vector3 lineJumpDirection;
	public GameObject flyingParticles;
	private ParticleSystem flyParticles;
	public float distanceCameraOnSpaceJump;
	private float proportionDistanceJump;
	private bool isFinishedSpaceJump;
	public GameObject explosionOnDieInSpacePrefab;



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
	private bool isSpaceJumping = false;
	private float jumpedTimer = 0f;
	private float jumpedTimerCooldown = 0.2f;
	private bool isGoingUp = false;
	private bool isOutsideAthmosphere;
	private float timeHasBeenInSpace = 0f;
	private Killable killable;
	private float timeHasNotBeenBreathing;
	private PappadaController pappadaC;
	private bool isSpaceJumpCharged = false;



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
		flyParticles = flyingParticles.GetComponent<ParticleSystem> ();

		initializeVariables ();
		
	}

	private void initializeVariables(){
		timeJumpPressed = 0;
		timeSinceAttackStarted = 0f;
		isJumping = false;
		isSpaceJumping = false;
		jumpedTimer = 0f;
		jumpedTimerCooldown = 0.2f;
		isGoingUp = false;
		timeHasNotBeenBreathing = timeBetweenDamageWhenNotBreathing;
		timeHasBeenInSpace = 0f;
		centerToExtremesDistance = (animationBigPappada.collider.bounds.size.z /2f)+extraSafeDistanceFromEnemies;
		isInvulnerable = false;
		rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		isFinishedSpaceJump = false;

		lineRenderer = GetComponent<LineRenderer> ();

		//Initialize the animator
		if(bpAnimator!=null){
			bpAnimator.SetBool("isJumping",false);
			bpAnimator.SetBool("isSpaceJumping",false);
			bpAnimator.SetBool("isGoingUp",false);
			bpAnimator.SetBool("isChargingSpaceJumping",false);
			bpAnimator.SetBool("isWalking",false);
		}

		FinishSpaceJump ();
	}


	void Update() {

		/*if(isSpaceJumping || isSpaceJumpCharged){
			GameManager.gameState.arePlanetsMoving = false;
		}else{
			GameManager.gameState.arePlanetsMoving = true;
		}*/

		if(isJumping || isSpaceJumping){
			jumpedTimer +=Time.deltaTime;
		}else{
			jumpedTimer = 0f;
		}

		if(body.getIsTouchingPlanet() && jumpedTimer >=jumpedTimerCooldown){
			FinishJump();
			if(!isFinishedSpaceJump){
				FinishSpaceJump();
				//ChargeJump ();
			}
		}

		if(isJumping || isSpaceJumping){
			bool isGoingUp = Vector3.Angle(transform.up,rigidbody.velocity.normalized)<90;
			if (isGoingUp) {
				bpAnimator.SetBool("isGoingUp",true);
			}else{
				bpAnimator.SetBool("isGoingUp",false);
			}
		}


		if(body.getIsOutsideAthmosphere()){
			breathingBubble.SetActive(true);
			//rigidbody.velocity = rigidbody.velocity.normalized * (Constants.GRAVITY_FORCE_OF_PLANETS/1.5f);

			if(!GameManager.gameState.isGameEnded){
				if(timeHasBeenInSpace>=timeToDieInSpace){
					breathingBubble.transform.localScale = new Vector3(0f,0f,0f);
					timeHasNotBeenBreathing+=Time.deltaTime;
					if(timeHasNotBeenBreathing>=timeBetweenDamageWhenNotBreathing){
						kill ();
						flyParticles.Stop();
						rigidbody.velocity = new Vector3(0f,0f,0f);
						GameObject newEffect = GameObject.Instantiate(explosionOnDieInSpacePrefab) as GameObject;
						newEffect.transform.position = transform.position;

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

		if(!isShowingLineJump){
			HideArrow();
		}else{
			ActArrow();
		}
	}

	void FinishSpaceJump(){
		bpAnimator.SetBool("isSpaceJumping",false);
		isSpaceJumping = false;
		flyParticles.Stop();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().returnOriginalZ();
		HideArrow();
		isFinishedSpaceJump = true;
	}

	void FinishJump(){
		bpAnimator.SetBool("isJumping",false);
		isJumping = false;
	}

	void FixedUpdate(){
		//Changed because the other way gave me some errors (Maurici)
		//rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;

		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		//this.rigidbody.MovePosition (newPosition);
		//rigidbody.velocity = rigidbody.velocity + movement;
		if(GetComponent<CharacterAttackController>().isAttacking){
			movement = movement * 0.2f;
		}
		this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		
		//rigidbody.MovePosition (newPosition);
	}

	public void StartAttack() {
		cAttackController.ChargeAttack(isLookingRight,this.transform);
	}

	public void SpaceJump() {
		rigidbody.AddForce (lineJumpDirection * spaceJumpForce, ForceMode.Impulse);
		//If we jump into the space, stop the particle system.
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isSpaceJumping",true);
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		isSpaceJumpCharged = false;
		isSpaceJumping = true;
		jumpedTimer = 0f;
		HideArrow ();
		flyParticles.Clear();
		flyParticles.Play();

		isFinishedSpaceJump = false;
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


	public void CancelChargingSpaceJump(){
		isSpaceJumpCharged = false;
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().returnOriginalZ();
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		jumpedTimer = 0f;
		HideArrow ();
	}

	public void StopMove() {
		bpAnimator.SetBool("isWalking",false);
		isMoving = false;
		moveAmount = new Vector3 (0f, 0f, 0f);
	}

	public void StopAttack() {
		isAttacking = false;
	}

	public void ChargeJump() {
		isSpaceJumpCharged = true;
		bpAnimator.SetBool("isChargingSpaceJumping",true);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZ (-distanceCameraOnSpaceJump);
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Play ();
		ShowArrow ();
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

	public void kill(){
		getHurt(killable.HP);
	}

	public void MoveArrow(float horizontalMove,float verticalMove){
		Vector3 horizontalDirection = transform.forward * horizontalMove;
		if(!isLookingRight){
			horizontalDirection *= -1f;
		}
		/*proportionDistanceJump = proportionDistanceJump + (verticalMove * 0.02f);
		if(proportionDistanceJump<0.2f){proportionDistanceJump = 0f; CancelChargingSpaceJump();}
		if(proportionDistanceJump>1f){proportionDistanceJump = 1f;}*/
		Vector3 newPositionLine = (lineJumpDirection +(0.05f * horizontalDirection )).normalized;

		if(Vector3.Angle(transform.up,newPositionLine)<70){
			lineJumpDirection = newPositionLine;
			ActArrow();
		}
	}

	public void ActArrow(){
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition(1,transform.position + (lineJumpDirection * lineJumpDistance));
		lineRenderer.SetWidth (2f,2f);
	}

	public void ShowArrow(){
		proportionDistanceJump = 0.5f;
		lineJumpDirection = transform.up;
		isShowingLineJump = true;
	}

	public void HideArrow(){
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, transform.position);
		isShowingLineJump = false;

	}

	public void reset(){
		if(killable!=null){
			killable.resetHP ();
			pappadaC.newProportionOfLife(killable.proportionHP());
		}
		initializeVariables ();
	}

	public bool getIsSpaceJumping(){
		return isSpaceJumping;
	}

	public bool getIsLookingRight(){
		return isLookingRight;
	}
}

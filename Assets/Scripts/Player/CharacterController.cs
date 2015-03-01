﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float normalJumpForce = 10f;
	public float spaceJumpForce = 100f;
	public GameObject particleSystemJumpCharge;
	public GameObject animationBigPappada;

	private CharacterAttackController cAttackController;
	
	private bool isAttacking = false;
	private GravityBody body;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;
	private float timeJumpPressed;
	private bool isMoving;
	private AnimationController animCont;
	private bool isLookingRight = true;

	private float timeSinceAttackStarted = 0f;
	private List<GameObject> closeEnemies = new List<GameObject>();
	private bool isJumping = false;
	private float jumpedTimer = 0f;
	private float jumpedTimerCooldown = 0.2f;
	bool attackEffectDone = false;
	
	void Awake(){
		GameManager.registerPlayer (gameObject);
	}

	void Start () {
		timeJumpPressed = 0;
		body = GetComponent<GravityBody> ();
		GameObject attack = GameObject.Find("skillAttack");
		animCont = GetComponent<AnimationController> ();
		transform.forward = new Vector3(1f,0f,0f);
		
		cAttackController = GetComponent<CharacterAttackController>();
				
	}
	
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Damageable") {
			closeEnemies.Add(col.gameObject);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Damageable") {
			closeEnemies.Remove(col.gameObject);
		}
	}


	void Update() {
		if (!isMoving) {
			if(animCont!=null){
				animCont.StopWalk();
			}
		}
		if(isJumping){
			jumpedTimer +=Time.deltaTime;
		}
		if(body.getIsTouchingPlanet() && jumpedTimer >=jumpedTimerCooldown){
			animationBigPappada.GetComponent<Animator>().SetBool("isJumping",false);
			animationBigPappada.GetComponent<Animator>().SetBool("isSpaceJumping",false);
		}else{

		}
	}

	void FixedUpdate(){
		//Changed because the other way gave me some errors (Maurici)
		//rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		//this.rigidbody.MovePosition (newPosition);
		//rigidbody.velocity = rigidbody.velocity + movement;
		//this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		rigidbody.MovePosition (newPosition);
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
		animationBigPappada.GetComponent<Animator>().SetBool("isSpaceJumping",true);
		animationBigPappada.GetComponent<Animator>().SetBool("isChargingSpaceJumping",false);
		isJumping = true;
		jumpedTimer = 0f;
	}

	public void Jump() {
		rigidbody.AddForce (transform.up * normalJumpForce, ForceMode.VelocityChange);
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		animationBigPappada.GetComponent<Animator>().SetBool("isJumping",true);
		isJumping = true;
		jumpedTimer = 0f;
	}

	public void Move() {
		animationBigPappada.GetComponent<Animator>().SetBool("isWalking",true);
		isMoving = true;
		if (!body.getUsesSpaceGravity()) {
			float inputHorizontal = Input.GetAxisRaw ("Horizontal");

			ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
			particles.Stop ();

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
		animationBigPappada.GetComponent<Animator>().SetBool("isWalking",false);

		isMoving = false;
	}

	public void StopAttack() {
		isAttacking = false;
	}

	public void ChargeJump() {
		animationBigPappada.GetComponent<Animator>().SetBool("isChargingSpaceJumping",true);
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Play ();
	}

}

using UnityEngine;
using System.Collections;

public class IAControllerRat : IAController {

	static int walkingState = Animator.StringToHash("Walking");
	static int undergroundState = Animator.StringToHash("Underground");

	//public AttackType poisonAttack;
	public AttackType jumpingAttack;
	public AttackType burrowAttack;


	public float patrolTimeToTurn = 1.5f;

	private float timeWalkingDirectionIdle = 0f;
	private float attackTimer = 0f;
	private float patrolTime = 0f;
	private float initialY; 

	private GameObject burrowParticles;

	private bool isJumping;

	protected override void initialize(){
		Debug.Log ("initialize");

		Attack burrowAttackA = attackController.getAttack(burrowAttack);
		Attack jumpingAttackA = attackController.getAttack (jumpingAttack);
		burrowAttackA.informParent (gameObject);
		jumpingAttackA.informParent (gameObject);

	
		//iaAnimator.SetBool ("isWalking", true);

		burrowParticles = gameObject.transform.Find ("BurriedDust").gameObject;
		burrowParticles.SetActive (false);

		SetMeleeRange (0.025f);
		SetVisionRange (3f);
		isJumping = false;
	}

	protected override void UpdateAI(){
		/*doActualBehaviour ();
		meleeAttackTimer += Time.deltaTime;
		chargeAttackTimer += Time.deltaTime;
		changeBehaviour();*/
		updateParticles ();
		updateModelHiding ();
		//Can I see the player? 
		if(isAtVisionRange() && !attackController.isDoingAnyAttack()){
			//I'm at melee range?
			if(isAtMeleeRange()){
				if (isBurried()) {
					Emerge ();
				}
				//characterController.Move (0f);
			} else {
				if (!isBurried()) {
					Burrow ();				
				}
				//if (!isBurried && isWalkingState()){
					//characterController.Move(getPlayerDirection ());
				//}
			}
			//Patrol ();
		//I can't see the player. Just Patrol.
		}else if (!attackController.isDoingAnyAttack() && !isJumping) {
			//characterController.Move (0f);
			//Emerge ();
			//Patrol ();
		}
	}

	private void Patrol(){
		//Patrols around
		//TODO: Check collisions, so it can turn around. 
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			characterController.Move(getLookingDirection()*-1f);
		}else{
			characterController.Move(getLookingDirection());
		}
		/*if (GetComponent<GravityBody>().getIsTouchingPlanet()) {
			characterController.Jump(2);
		}*/
	}

	private void Emerge() {
		iaAnimator.SetBool ("Unearthing", true);
		iaAnimator.SetBool ("Burrowing", false);
	}

	private void Burrow() {
		iaAnimator.SetBool ("Burrowing", true);
		iaAnimator.SetBool ("Unearthing", false);
	}

	private bool isWalkingState(){
		bool value = iaAnimator.GetCurrentAnimatorStateInfo (0).IsName("Base.Walking");
		return value;
	}

	private bool isBurried() {
		bool value = iaAnimator.GetCurrentAnimatorStateInfo (0).IsName("Base.Underground");
		return value;
	}

	private void updateParticles()  {
		if (isWalkingState ()) {
			burrowParticles.SetActive (false);
		} else {
			burrowParticles.SetActive (true);
		}
	}
	private void updateModelHiding() {
		if (isBurried ()) {
			gameObject.transform.Find ("Model").gameObject.SetActive (false);
		} else {
			gameObject.transform.Find ("Model").gameObject.SetActive (true);
		}
	}
}

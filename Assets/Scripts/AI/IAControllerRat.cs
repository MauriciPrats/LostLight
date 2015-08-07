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

	private bool isBurried;
	private bool isJumping;

	protected override void initialize(){
		Debug.Log ("initialize");

		Attack burrowAttackA = attackController.getAttack(burrowAttack);
		Attack jumpingAttackA = attackController.getAttack (jumpingAttack);
		burrowAttackA.informParent (gameObject);
		jumpingAttackA.informParent (gameObject);

	
		iaAnimator.SetBool ("isWalking", true);
		isBurried = false;

		burrowParticles = gameObject.transform.Find ("BurriedDust").gameObject;
		burrowParticles.SetActive (false);

		SetMeleeRange (0.025f);
		SetVisionRange (3f);
		isBurried = false;
		isJumping = false;

		initialY = gameObject.transform.Find ("Model").gameObject.transform.position.y;
	}

	protected override void UpdateAI(){
		/*doActualBehaviour ();
		meleeAttackTimer += Time.deltaTime;
		chargeAttackTimer += Time.deltaTime;
		changeBehaviour();*/

		//Can I see the player? 
		if(isAtVisionRange() && !attackController.isDoingAnyAttack()){
			//I'm at melee range?
			if(isAtMeleeRange()){
				Debug.Log ("Estoy a mele!");
				if (isBurried) {
					Debug.Log("Salgo!");
					Emerge ();
					//gameObject.transform.Find ("Model").gameObject.transform.localPosition = new Vector3 (0.391f,0,-0.418f);
				}
				//characterController.Move (0f);
				Debug.Log ("Ataque veneno");
			} else {
				Debug.Log ("Me voy debajo tierra. A ver si puedo llegar hasta el.");
				if (!isBurried) {
					Burrow ();				
				}
				if (isBurried && isUndergroundState()) {
					gameObject.transform.Find ("Model").gameObject.SetActive(false);
				}	
				//if (!isBurried && isWalkingState()){
					characterController.Move(getPlayerDirection ());
				//}
			}

		//I can't see the player. Just Patrol.
		}else if (!attackController.isDoingAnyAttack() && !isJumping) {
			Debug.Log ("Patrullo en la superficie"); 
			//characterController.Move (0f);
			//Emerge ();
			Patrol ();
		}
	}

	private void Patrol(){
		//Patrols around
		//TODO: Check collisions, so it can turn around. 
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			Move(getLookingDirection()*-1f);
		}else{
			Move(getLookingDirection());
		}
		if (GetComponent<GravityBody>().getIsTouchingPlanet()) {
			characterController.Jump(2);
		}
	}

	private void Emerge() {
		burrowParticles.SetActive (false);
		iaAnimator.SetBool ("Unearthing", true);
		gameObject.transform.Find ("Model").gameObject.SetActive(true);
		isBurried = false;
	}

	private void Burrow() {
		iaAnimator.SetBool ("Burrowing", true);
		burrowParticles.SetActive (true);
		isBurried = true;	
	}

	private bool isWalkingState(){
		bool value = iaAnimator.GetCurrentAnimatorStateInfo (0).IsName("Base.Walking");
		return value;
	}

	private bool isUndergroundState() {
		bool value = iaAnimator.GetCurrentAnimatorStateInfo (0).IsName("Base.Underground");
		return value;
	}
}

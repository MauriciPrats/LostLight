using UnityEngine;
using System.Collections;

public class IAControllerPenguin : IAController {
	
	//IA NOT TESTED WITH FINAL MODEL
	private enum ActualBehaviour{Patroling,ChasePlayer,Sliding,WhirlwindAttack};
	
	public AttackType meleeAttack;
	public AttackType slideMove;
	public float minimumDistanceInFront = 0.4f;
	public float patrolTimeToTurn = 1.5f;
	public float maxDistancePlayer = 20f;
	public float minDistanceMeleeAttack = 0.4f;
	public float chanceToMeleeAttack = 0.6f;
	public float chanceToSlide = 0.2f;
	public float meleeAttackCooldown = 1f;
	public float slideCooldown = 3f;
	public float lookTime = 0.1f;

	private float slideTimer = 0f;
	private float meleeAttackTimer = 0f;
	private OutlineChanging outlineChanger;
	private ActualBehaviour actualBehaviour;
	private float patrolTime = 0f;
	private float changeBehaviourTimer = 0f;

	private bool isSliding = false;
	private bool isDoingMeleeAttack = false;

	private float looktimer = 0f;
	
	protected override void initialize(){
		Attack meleeAttackA = attackController.getAttack(meleeAttack);
		meleeAttackA.informParent(gameObject);

		Attack slideMoveA = attackController.getAttack(slideMove);
		slideMoveA.informParent(gameObject);
	}
	
	protected override void UpdateAI(){
		changeBehaviour();
		meleeAttackTimer += Time.deltaTime;
		slideTimer += Time.deltaTime;
		doActualBehaviour ();
	}

	//Changes the behaviour of the enemy depending on the state of it's surroundings
	private void changeBehaviour(){
		if (!attackController.isDoingAnyAttack()) {
			//We check if we have to reset the melee and charging attack timers
			if(isDoingMeleeAttack){meleeAttackTimer = 0f; isDoingMeleeAttack=false;}
			if(isSliding){slideTimer = 0f; isSliding = false;}
		}
		//Changes the behaviour depending on the conditions of the AI
		if(getPlayerDistance()>maxDistancePlayer || !canSeePlayer ()){
			actualBehaviour = ActualBehaviour.Patroling;
		}else if(canSeePlayer ()){
			if(slideTimer>slideCooldown && getPlayerDistance()>minDistanceMeleeAttack){
				slideTimer = 0f;
				if(Random.value<=chanceToSlide){
					actualBehaviour = ActualBehaviour.Sliding;
				}else{
					actualBehaviour = ActualBehaviour.ChasePlayer;
				}
			}else{
				if(getPlayerDistance()>minDistanceMeleeAttack){
					actualBehaviour = ActualBehaviour.ChasePlayer;
				}else{
					if(meleeAttackTimer>meleeAttackCooldown){
						meleeAttackTimer = 0f;
						if(Random.value<=chanceToMeleeAttack){
							actualBehaviour = ActualBehaviour.WhirlwindAttack;
						}else{
							actualBehaviour = ActualBehaviour.ChasePlayer;
						}
					}
				}
			}
		}
	}

	//Does the action that corresponds to the actual behaviour unless it is dead
	private void doActualBehaviour(){
		if(!isDead && !attackController.isDoingAnyAttack()){
			if(actualBehaviour.Equals(ActualBehaviour.ChasePlayer)){
				if(closestThingInFrontDistance()>minDistanceMeleeAttack){
					Move (getPlayerDirection());
				}else{
					StopMoving();
				}
			}else if(actualBehaviour.Equals(ActualBehaviour.Patroling)){
				Patrol ();
			}else if(actualBehaviour.Equals(ActualBehaviour.Sliding)){
				StopMoving();
				if(getIsTouchingPlanet()){
					if(attackController.doAttack(slideMove,false)){
						isSliding = true;
					}
				}
			}else if(actualBehaviour.Equals(ActualBehaviour.WhirlwindAttack)){
				StopMoving();
				if(getIsTouchingPlanet()){
					if(attackController.doAttack(meleeAttack,false)){
						isDoingMeleeAttack = true;
					}
				}
			}
		}
	}

	//Walks to one side and to another alternatively
	private void Patrol(){
		//Patrols around
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			Move(getLookingDirection()*-1f);
		}else{
			Move(getLookingDirection());
		}
	}

}

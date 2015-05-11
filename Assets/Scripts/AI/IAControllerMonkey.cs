using UnityEngine;
using System.Collections;

public class IAControllerMonkey : IAController {


	private enum ActualBehaviour{Patroling,RangedAttack,Escape,ChasePlayer,Jumping};

	public AttackType rangedAttack;
	public float minimumDistanceInFront = 0.4f;
	public float patrolTimeToTurn = 1.5f;
	public float maxDistancePlayer = 20f;
	public float chanceToRangedAttack = 0.6f;
	public float maxDistanceCharge = 8f;
	public float lookTime = 0.1f;
	public float minimumPlayerDistanceForRangedAttack = 4f;
	public float maximumPlayerDistanceForRangedAttack = 8f;
	
	private float rangedAttackTimer = 0f;
	private OutlineChanging outlineChanger;
	private ActualBehaviour actualBehaviour;
	private float patrolTime = 0f;
	private float changeBehaviourTimer = 0f;
	private bool isCharging = false;
	private bool isDoingMeleeAttack = false;
	private float looktimer = 0f;
	
	protected override void initialize(){
		Attack rangedAttackA = attackController.getAttack(rangedAttack);
		rangedAttackA.informParent(gameObject);
	}
	
	protected override void UpdateAI(){
		doActualBehaviour ();
		rangedAttackTimer += Time.deltaTime;
		changeBehaviour();
	}
	
	
	private void changeBehaviour(){
		//Changes the behaviour depending on the conditions of the AI
		actualBehaviour = ActualBehaviour.Patroling;
	}
	
	private void doActualBehaviour(){
		//Does the action that corresponds to the actual behaviour unless it is dead
		if(!isDead){
			if(getPlayerDistance()<minimumPlayerDistanceForRangedAttack){
				moveAndAvoid(getPlayerDirection()*-1f);
			}else if(getPlayerDistance()>maximumPlayerDistanceForRangedAttack && canSeePlayer()){
				Move (getPlayerDirection());
				//Patrol();
			}else if(canSeePlayer ()){
				attackController.doAttack(rangedAttack,false);
			}
		}
	}

	private void moveAndAvoid(float direction){
		if(getIsBlockedBySomethingInFront() && closestThingInFrontDistance()<=minimumDistanceInFront ){
			if(getIsTouchingPlanet()){
				Jump();
			}
			Move (direction);
		}else{
			Move (direction);
		}
	}
	
	private void Patrol(){
		//Patrols around
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			moveAndAvoid(getLookingDirection()*-1f);
		}else{
			moveAndAvoid(getLookingDirection());
		}
	}

}

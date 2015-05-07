using UnityEngine;
using System.Collections;

public class IAControllerJabali : IAController {

	private enum ActualBehaviour{Patroling,MeleeAttack,ChargeAttack,ChasePlayer,OffensiveIdle};

	public AttackType frontAttack;
	public AttackType chargeAttack;
	public float patrolTimeToTurn = 1.5f;
	public float maxDistancePlayer = 20f;
	public float maxDistanceMeleeAttack = 0.1f;
	public float changeToCharge = 0.02f;
	public float changeToAttackMelee = 0.05f;
	public float meleeAttackCooldown = 0.5f;
	public float chargeAttackCooldown = 1.5f;
	public float changeBehaviours = 0.2f;
	public float maxDistanceCharge = 8f;

	private float meleeAttackTimer = 0f;
	private float chargeAttackTimer = 0f;
	private OutlineChanging outlineChanger;
	private ActualBehaviour actualBehaviour;
	private float patrolTime = 0f;
	private float changeBehaviourTimer = 0f;
	private bool isCharging = false;
	private bool isDoingMeleeAttack = false;

	protected override void initialize(){
		Attack frontAttackA = attackController.getAttack(frontAttack);
		frontAttackA.informParent(gameObject);
		Attack chargeAttackA = attackController.getAttack(chargeAttack);
		chargeAttackA.informParent(gameObject);
	}

	protected override void UpdateAI(){
		doActualBehaviour ();
		meleeAttackTimer += Time.deltaTime;
		chargeAttackTimer += Time.deltaTime;
		changeBehaviourTimer += Time.deltaTime;
			
		//if(changeBehaviourTimer>=timeToChangeBehaviour){
				//changeBehaviourTimer = 0f;
		changeBehaviour();
		//}
	}


	private void changeBehaviour(){
			if (!attackController.isDoingAnyAttack()) {
				if(isDoingMeleeAttack){meleeAttackTimer = 0f; isDoingMeleeAttack=false;}
				if(isCharging){chargeAttackTimer = 0f; isCharging = false;}
			}

			if(!canSeePlayer()){
				actualBehaviour = ActualBehaviour.Patroling;
			}else{
				if(getPlayerDistance()>=maxDistancePlayer){
					actualBehaviour = ActualBehaviour.Patroling;
				}

			if(getPlayerDistance()<=maxDistanceMeleeAttack){
					//Is at melee range
					if(meleeAttackTimer>meleeAttackCooldown){
						meleeAttackTimer = 0f;
						if(Random.value<=changeToAttackMelee){
							actualBehaviour = ActualBehaviour.MeleeAttack;
						}else{
							actualBehaviour = ActualBehaviour.OffensiveIdle;
						}
					}else{
						actualBehaviour = ActualBehaviour.OffensiveIdle;
					}

			}else if(getPlayerDistance()<=maxDistanceCharge){
					//Is farther away
					if(chargeAttackTimer>chargeAttackCooldown){
						chargeAttackTimer = 0f;
						if(Random.value<=changeToCharge){
							actualBehaviour = ActualBehaviour.ChargeAttack;
						}else{
							actualBehaviour = ActualBehaviour.ChasePlayer;
						}
					}else{
						actualBehaviour = ActualBehaviour.ChasePlayer;
					}
					
			}else{
				actualBehaviour = ActualBehaviour.ChasePlayer;
			}
		}
	}

	private void doActualBehaviour(){
		if(!isDead){
			if(!attackController.isDoingAnyAttack() && getIsTouchingPlanet()){
				if(actualBehaviour.Equals(ActualBehaviour.Patroling)){
					Patrol();
				}else if(actualBehaviour.Equals(ActualBehaviour.MeleeAttack)){
					if(!attackController.isDoingAnyAttack()){
							StopMoving();
							lookAtDirection(getPlayerDirection());
						if(!attackController.doAttack(frontAttack,false)){
							IdleInFrontOfPlayer();
						}else{
							isDoingMeleeAttack = true;
						}
					}
				}else if(actualBehaviour.Equals(ActualBehaviour.ChargeAttack)){
					if(!attackController.isDoingAnyAttack()){
							StopMoving();
							lookAtDirection(getPlayerDirection());

						if(!attackController.doAttack(chargeAttack,false)){
							actualBehaviour = ActualBehaviour.ChasePlayer;
						}else{
							isCharging = true;
						}
					}
				}else if(actualBehaviour.Equals(ActualBehaviour.ChasePlayer)){
					if(closestThingInFrontDistance()>=minimumDistanceAttackPlayer){
						Move(getPlayerDirection());
					}else{
						StopMoving();
					}
				}
			}
		}
	}

	private void IdleInFrontOfPlayer(){
		patrolTime += Time.deltaTime;
		lookAtDirection(getPlayerDirection());

	}

	private void Patrol(){
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			Move(getLookingDirection()*-1f);
		}else{
			Move(getLookingDirection());
		}
	}


	/*private IEnumerator jumpRandomly(){
		while(true){
		float timeToWait = Random.value+0.5f;
		float timer = 0f;
			while(timer<timeToWait){
				timer+=Time.deltaTime;
				yield return null;
			}
			if(getIsTouchingPlanet()){Jump ();}
		}
	}*/
	/***FUNCTIONS***/
	//initialize()
	//UpdateAI()
	//Move(float)
	//StopMoving()
	//Jump()

	/***INFORMATION***/
	//canSeePlayer()
	//getPlayerDistance()
	//getIsBlockedBySomethingInFront()
	//closestThingInFrontDistance()
	//getIsLookingRight()
	//getListCloseAllies()
	//isDoingAttack
	//isDead;
	//isOnGuard;
	//isFrozen;
	//isStunned;

	/***STATES***/
	//

}

using UnityEngine;
using System.Collections;

public class IAControllerJabali : IAController {

	public AttackType frontAttack;
	private float timeWalkingDirectionIdle = 0f;
	private float attackTimer = 0f;

	protected override void initialize(){
		Attack frontAttackA = attackController.getAttack(frontAttack);
		frontAttackA.informParent(gameObject);
		StartCoroutine ("jumpRandomly");
	}

	protected override void UpdateAI(){
		if(getPlayerDistance()<0.1f && !attackController.isDoingAnyAttack()){
			attackController.doAttack(frontAttack);
			if(getLookingDirection()!=getPlayerDirection()){
				Move(0.01f*getPlayerDirection());
			}
		}else if(!attackController.isDoingAnyAttack()){
			if(getIsTouchingPlanet()){
			Move (getPlayerDirection ());
			}
		}else{
			StopMoving();
		}
	}

	private IEnumerator jumpRandomly(){
		while(true){
		float timeToWait = Random.value+0.5f;
		float timer = 0f;
			while(timer<timeToWait){
				timer+=Time.deltaTime;
				yield return null;
			}
			if(getIsTouchingPlanet()){Jump ();}
		}
	}
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

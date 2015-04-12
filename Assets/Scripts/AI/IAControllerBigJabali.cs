using UnityEngine;
using System.Collections;

public class IAControllerBigJabali : IAController {

	public float chargeChance = 0.2f;
	public float timeBetweenCheckAggresiveBehaviours = 2f;
	private float timerBetweenAggresiveBehaviours = 0f;
	private float timeWalkingDirectionIdle;



	//Called when can see the player
	protected override void offensiveMoves(){
		if (!isDoingAttack){
			offensiveMovement();
			timerBetweenAggresiveBehaviours += Time.deltaTime;
			if(timerBetweenAggresiveBehaviours>= timeBetweenCheckAggresiveBehaviours){
				timerBetweenAggresiveBehaviours = 0f;
				if(Random.value<=chargeChance){
					//DoCharge
					doCharge();
				}
			}
		}else{
			if(actualAttack.isAttackFinished()){
				isDoingAttack = false;
			}else{
				actualAttack.doAttack();
			}
		}
		//Random between slowly approach and charge

		//If charge, start charging 

		//(Ignore collisions with enemies?)
	}

	private void doCharge(){
		float moveDirection = 0f;
		if(isElementLeft(player)){
			moveDirection = -1f;
		}else{
			moveDirection = 1f;
		}
		characterController.LookLeftOrRight (moveDirection);

		timerBetweenAggresiveBehaviours = 0f;
		GameObject attack = shortRangeAttacks[Random.Range(0,shortRangeAttacks.Length)];
		AIAttack bAttack = attack.GetComponent<AIAttack>();
		isDoingAttack = true;
		actualAttack = bAttack;
		actualAttack.startAttack();

	}
	
	private void wanderAround(){
		timerBetweenAggresiveBehaviours = 0f;
	}

	protected override void idleWalking(){
		if(!isDoingAttack){
			timeWalkingDirectionIdle += Time.deltaTime;
			if(timeWalkingDirectionIdle>timePatroling || isBlockedBySomethingInFront){
				isWalkingRight = !isWalkingRight;
				timeWalkingDirectionIdle = 0f;
			}
			
			float moveDirection = 0f;
			if(isWalkingRight){
				moveDirection = 1f;
			}else{
				moveDirection = -1f;
			}
			characterController.LookLeftOrRight (moveDirection);
			walk (moveDirection);
		}
	}
	
	private void offensiveMovement(){
		float moveDirection = 0f;
		if(isElementLeft(player)){
			moveDirection = -1f;
		}else{
			moveDirection = 1f;
		}
		characterController.LookLeftOrRight (moveDirection);
		
		if(!isStunned){
			walk (moveDirection);
		}
	}
}

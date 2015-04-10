using UnityEngine;
using System.Collections;

public class IAControllerJabali : IAController {

	private float timeWalkingDirectionIdle = 0f;
	private float attackTimer = 0f;

	protected override void offensiveMoves(){
		if(isDoingAttack){
			if(actualAttack.isAttackFinished()){
				isDoingAttack = false;
			}else{
				actualAttack.doAttack();
			}
		}else{
			//Check how far away is the player
			attackTimer += Time.deltaTime;
			
			float distanceToPlayer = Vector3.Distance (transform.GetComponent<Rigidbody>().worldCenterOfMass, player.GetComponent<Rigidbody>().worldCenterOfMass);
			distanceToPlayer -= (player.GetComponent<PlayerController> ().centerToExtremesDistance + walkOnMultiplePaths.centerToExtremesDistance);
			
			if(distanceToPlayer<= minimumDistanceAttackPlayer){
				if(attackTimer>= attackChoosingCooldown){
					attackTimer = 0f;
					if(Random.value<attackChance){
						//If we actually want to attack
						
						//Choose the actual attack
						if(shortRangeAttacks.Length>0){
							GameObject attack = shortRangeAttacks[Random.Range(0,shortRangeAttacks.Length)];
							AIAttack bAttack = attack.GetComponent<AIAttack>();
							
							if(GameManager.enemyAttackManager.askForNewAttack(bAttack.getAttackValue())){
								isDoingAttack = true;
								actualAttack = bAttack;
								actualAttack.startAttack();
							}
						}
					}else{
						offensiveMovement();
					}
				}else{
					offensiveMovement();
				}
			}else{
				offensiveMovement();
			}
		}
	}

	protected override void idleWalking(){
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

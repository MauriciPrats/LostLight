using UnityEngine;
using System.Collections;

public class IAControllerJabali : IAController {

	public AttackType attack1;
	private float timeWalkingDirectionIdle = 0f;
	private float attackTimer = 0f;

	protected override void initialize(){
		Attack attack1ToDo = attackController.getAttack(attack1);
		attack1ToDo.informParent(gameObject);
	}

	protected override void offensiveMoves(){
		if(!attackController.isDoingAnyAttack()){
			//Check how far away is the player
			attackTimer += Time.deltaTime;
			
			float distanceToPlayer = Vector3.Distance (transform.GetComponent<Rigidbody>().worldCenterOfMass, player.GetComponent<Rigidbody>().worldCenterOfMass);
			distanceToPlayer -= (player.GetComponent<PlayerController> ().centerToExtremesDistance + walkOnMultiplePaths.centerToExtremesDistance);
			
			if(distanceToPlayer<= minimumDistanceAttackPlayer){
				if(attackTimer>= attackChoosingCooldown){
					attackTimer = 0f;
					if(Random.value<attackChance && !isStunned){
						//If we actually want to attack
						
						//Choose the actual attack
						if(shortRangeAttacks.Length>0){
							GameObject attack = shortRangeAttacks[Random.Range(0,shortRangeAttacks.Length)];
							//AIAttack bAttack = attack.GetComponent<AIAttack>();
							int aCost = attackController.getAttack(attack1).cost;
							if(GameManager.enemyAttackManager.askForNewAttack(attackController.getAttack(attack1).cost)){
								attackController.doAttack(attack1);
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
		
		if(!isFrozen && !isStunned){
			walk (moveDirection);
		}
	}
}

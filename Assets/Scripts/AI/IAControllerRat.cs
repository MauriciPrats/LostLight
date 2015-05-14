using UnityEngine;
using System.Collections;

public class IAControllerRat : IAController {

	public AttackType poisonAttack;
	public AttackType jumpingAttack;
	public AttackType burrowAttack;

	public float patrolTimeToTurn = 1.5f;

	private float timeWalkingDirectionIdle = 0f;
	private float attackTimer = 0f;
	private float patrolTime = 0f;

	protected override void initialize(){
		Attack poisonAttackA = attackController.getAttack(poisonAttack);
		Attack burrowAttackA = attackController.getAttack (burrowAttack);
		//Attack jumpingAttackA = attackController.getAttack (jumpingAttack);
		burrowAttackA.informParent (gameObject);
		//jumpingA
		//poisonAttack.informParent(gameObject);
		SetMeleeRange (1f);
		SetVisionRange (40f);
	}

	protected override void UpdateAI(){
		//Can I see the player? 
		/*if(isAtVisionRange() && !attackController.isDoingAnyAttack()){
			//I'm at melee range?
			if(isAtMeleeRange()){
				//Poison attack
				attackController.doAttack (poisonAttack,false);
			} else {
				//70% chance to burrow. 30% chance to attack jumping. 
				float randomNum = Random.Range (0,100);
				if ( randomNum > 30 ) {
					attackController.doAttack(burrowAttack,false);
				} else {
					attackController.doAttack(jumpingAttack,false);
				}
			}
		//I can't see the player. Just Patrol or Burrow
		}else if (!attackController.isDoingAnyAttack()) {
			if (getIsTouchingPlanet ()) {		
				float randomNum = Random.Range (0,100);
				if ( randomNum > 30 ) {
					//TODO: Enhance the patrolling system.
					Move (getPlayerDirection ());
				}else {
					attackController.doAttack(burrowAttack,false);
				}
			}
		} else {
			StopMoving ();
		}*/
		//Burrow ();
		Patrol ();
	}

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

	private void Burrow() {
		//TODO: play burrow Animation (not created yet). 
		attackController.doAttack(burrowAttack,false);
	}
}

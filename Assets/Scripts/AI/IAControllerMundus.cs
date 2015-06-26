using UnityEngine;
using System.Collections;

public class IAControllerMundus : IAController {

	public GameObject rightClaw;
	public GameObject leftClaw;

	public AttackType baseAttack;
	public AttackType ballOfDeathAttack;

	private float patrolTime = 0f;
	private float patrolTimeToTurn = 2f;

	protected override void initialize(){
		Attack meleeAttackA = attackController.getAttack(baseAttack);
		meleeAttackA.informParent(gameObject);

		Attack ballOfDeathA = attackController.getAttack(ballOfDeathAttack);
		ballOfDeathA.informParent(gameObject);
	}
	
	protected override void UpdateAI(){
		if (!attackController.isDoingAnyAttack ()) {
			characterController.LookLeftOrRight(getPlayerDirection());
			attackController.doAttack(ballOfDeathAttack,false);
		}
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


}

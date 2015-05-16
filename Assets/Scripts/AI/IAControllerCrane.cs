using UnityEngine;
using System.Collections;

public class IAControllerCrane : IAController {

	
	
	private enum ActualBehaviour{Patroling,ChasePlayer,Sliding,WhirlwindAttack};
	
	public AttackType meleeAttack;
	public AttackType FlyingAttack;

	public float minimumDistanceInFront = 0.4f;
	public float patrolTimeToTurn = 1.5f;
	public float maxDistancePlayer = 20f;
	public float minDistanceMeleeAttack = 0.4f;

	private OutlineChanging outlineChanger;
	private ActualBehaviour actualBehaviour;
	private float patrolTime = 0f;
	private float changeBehaviourTimer = 0f;
	
	private bool isSliding = false;
	private bool isDoingMeleeAttack = false;
	
	private float looktimer = 0f;
	
	protected override void initialize(){
		/*Attack meleeAttackA = attackController.getAttack(meleeAttack);
		meleeAttackA.informParent(gameObject);
		
		Attack slideMoveA = attackController.getAttack(slideMove);
		slideMoveA.informParent(gameObject);*/
	}
	
	protected override void UpdateAI(){
		changeBehaviour();
		doActualBehaviour ();
	}
	
	private void changeBehaviour(){

	}
	
	private void doActualBehaviour(){

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

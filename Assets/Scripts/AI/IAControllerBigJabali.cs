using UnityEngine;
using System.Collections;

public class IAControllerBigJabali : IAController {

	public AttackType attack1;
	public float chargeChance = 0.2f;
	public float timeBetweenCheckAggresiveBehaviours = 2f;
	private float timerBetweenAggresiveBehaviours = 0f;
	private float timeWalkingDirectionIdle;

	protected override void initialize(){
		Attack attack1ToDo = attackController.getAttack(attack1);
		attack1ToDo.informParent(gameObject);
	}

	protected override void UpdateAI(){

	}

}

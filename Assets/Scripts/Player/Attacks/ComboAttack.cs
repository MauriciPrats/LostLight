using UnityEngine;
using System.Collections;

public class ComboAttack : Attack {

	public AnimationEventBroadcast eventHandler;
	public Collider stick;

	
	public override void initialize() {
		eventHandler = GameManager.playerAnimator.gameObject.GetComponent<AnimationEventBroadcast>();
		stick = gameObject.GetComponent<Collider>();
	}
	
	public override void enemyCollisionEnter(GameObject enemy) {
		print ("I hit a pig");
	}



	protected override void update(){
		//just if update is needed, might be unecessary if coroutines are used
		
	}

	public bool isComboable = false;
	public int combosteep = 0;
	
	
	public override void startAttack(){
	
	if (isFinished) {
			eventHandler.subscribe(this);
			isFinished = false;
			GameManager.playerAnimator.SetTrigger("doComboAttack");	
			combosteep = 1;
	} else {
		if (isComboable && combosteep < 3) {
				isComboable = false;
				GameManager.playerAnimator.SetTrigger("doComboAttack");	
				combosteep++;
		}
	}
	
		
	}

	public void enableHitbox() {
		stick.enabled = true;
	}
	
	public void allowChaining() {
		isComboable = true;
	}
	
	public void dissableHitbox() {
		stick.enabled = false;
	}
	
	public void endCombo() {
		//gameObject.GetComponent<ParticleSystem>().enableEmission = true;
		eventHandler.unsubscribe(this);
		isComboable = false;
		isFinished = true;
		GameManager.playerAnimator.ResetTrigger("doComboAttack");
		print ("combo ended");
	}
	
	public override bool isAttackFinished(){
		return isFinished || isComboable;
	}
	
	
}

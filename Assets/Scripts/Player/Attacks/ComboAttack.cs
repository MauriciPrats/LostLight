using UnityEngine;
using System.Collections;

public class ComboAttack : Attack {

	public AnimationEventBroadcast eventHandler;
	GameObject stick;
	public override void initialize() {
		eventHandler = GameManager.playerAnimator.gameObject.GetComponent<AnimationEventBroadcast>();
		stick = GameObject.FindGameObjectWithTag("Weapon");
	}
	
	public override void enemyCollisionEnter(GameObject enemy){
		//Called when an AttackCollider with this object associated enters collision with an enemy

	}



	protected override void update(){
		//just if update is needed, might be unecessary if coroutines are used
		
	}

	public bool isComboable = false;
	public int combosteep = 0;
	public override void startAttack(){
		StartCoroutine ("comboAttack");
	}

	IEnumerator comboAttack(){
		isFinished = false;
		GameManager.playerAnimator.SetTrigger("doComboAttack");
		//eventHandler.subscribe(this);
		yield return new WaitForSeconds (0.5f);
		endCombo ();
	}
	
	public void enableHitbox() {
		stick.GetComponent<CapsuleCollider>().enabled = true;
	}
	
	public void allowChaining() {
		isComboable = true;
	}
	
	public void dissableHitbox() {
		stick.GetComponent<CapsuleCollider>().enabled = false;
	}
	
	public void endCombo() {
		print ("returning to base");
		isComboable = false;
		isFinished = true;
		GameManager.playerAnimator.ResetTrigger("doComboAttack");
	}
	
	public override bool isAttackFinished(){
		return isFinished || isComboable;
	}
	
	
}

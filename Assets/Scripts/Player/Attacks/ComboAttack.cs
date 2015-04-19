using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboAttack : Attack, AnimationSubscriber {

	private AnimationEventBroadcast eventHandler;
	private Collider stick;
	private AttackCollider attackCollider;
	private Xft.XWeaponTrail weaponEffects;
	private float timeOfAnimation;
	
	private List<GameObject> enemiesHit;
	private bool hasHitEnemy;
	public override void initialize() {
		eventHandler = GameManager.playerAnimator.gameObject.GetComponent<AnimationEventBroadcast>();
		eventHandler.subscribe(this);
		stick = GameManager.player.GetComponent<PlayerController>().weapon.GetComponentInChildren<Collider>();
		attackCollider = GameManager.player.GetComponent<PlayerController>().weapon.GetComponentInChildren<AttackCollider>();
		weaponEffects = GameManager.player.GetComponent<PlayerController>().weapon.GetComponentInChildren<Xft.XWeaponTrail>();
		//weaponEffects.StopSmoothly(0.1f);
	}
	
	public override void enemyCollisionEnter(GameObject enemy) {
		if(!enemiesHit.Contains(enemy)){
			enemiesHit.Add(enemy);
			enemy.GetComponent<IAController>().getHurt(1,(enemy.transform.position));
			enemy.GetComponent<IAController> ().interruptAttack ();
			GameManager.comboManager.addCombo ();
			if(!hasHitEnemy){
				hasHitEnemy = true;
				GameManager.lightGemEnergyManager.addPoints(1);
			}
		}
	}



	protected override void update(){
		//just if update is needed, might be unecessary if coroutines are used
		
	}

	public bool isComboable = false;
	public int combosteep = 0;
	
	
	public override void startAttack(){
	
	if (isFinished) {
			enemiesHit = new List<GameObject>(0);
			hasHitEnemy = false;
			attackCollider.attack = gameObject;
			//stick.enabled = true;
			weaponEffects.Activate();

			isFinished = false;
			GameManager.playerAnimator.SetTrigger("doComboAttack");	
			combosteep = 1;
	} else {
		if (isComboable && combosteep < 3) {
				enemiesHit = new List<GameObject>(0);
				hasHitEnemy = false;
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
		weaponEffects.StopSmoothly(0.5f);
	//	eventHandler.unsubscribe(this);
		isComboable = false;
		isFinished = true;
		GameManager.playerAnimator.ResetTrigger("doComboAttack");
		attackCollider.attack = null;
		stick.enabled = false;
		//print ("combo ended");
	}
	
	public override bool isAttackFinished(){
		return isFinished || isComboable;
	}
	
	void AnimationSubscriber.handleEvent(string idEvent) {
		switch (idEvent) {
		case "start": 
			enableHitbox();
			break;
		case "end":
			dissableHitbox();
			break;
		case "done":
			endCombo();
			break;
		case "combo":
			allowChaining();
			break;
		default: 
			
			break;
		}
		
	}
	
	
	string AnimationSubscriber.subscriberName() {
		return  "ComboAttack";	
	}
	
	
}

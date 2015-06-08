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
		attackType = AttackType.Combo;
		eventHandler = GameManager.playerAnimator.gameObject.GetComponent<AnimationEventBroadcast>();
		eventHandler.subscribe(this);
		
		stick = GameManager.player.GetComponent<PlayerController>().weapon.GetComponentInChildren<Collider>();
		attackCollider = GameManager.player.GetComponent<PlayerController>().weapon.GetComponentInChildren<AttackCollider>();
		weaponEffects = GameManager.player.GetComponent<PlayerController>().weapon.GetComponentInChildren<Xft.XWeaponTrail>();
		//weaponEffects.StopSmoothly(0.1f);
	}
	//Cuando se hace el combo 3 con el enemies hit  activo, los enemigos no son afectados por el tercer golpe. 
	public override void enemyCollisionEnter(GameObject enemy) {
		if(!enemiesHit.Contains(enemy) && !enemy.GetComponent<IAController>().isDead){
			enemiesHit.Add(enemy);
			enemy.GetComponent<IAController>().getHurt(1,(enemy.transform.position));
			GameManager.comboManager.addCombo ();
			GameManager.audioManager.PlayStableSound(10);
			//Pruebas de que lanze al enemigo por los aires)
			//Vector3 direction = enemy.GetComponent<Rigidbody>().worldCenterOfMass - GameManager.player.transform.position;
			//enemy.GetComponent<Rigidbody>().velocity+=(direction*4f);
			if(!hasHitEnemy){
				hasHitEnemy = true;
				GameManager.lightGemEnergyManager.addPoints(1);
			}
		}
	}

	public bool isComboable = false;
	public int combosteep = 0;
	
	
	public override void startAttack(){
	
	if (isFinished) {
			attackCollider.attack = gameObject;
			//stick.enabled = true;
			weaponEffects.Activate();

			isFinished = false;
			GameManager.playerAnimator.SetTrigger("doComboAttack");	
			combosteep = 1;
	} else {
		if (isComboable) {
				isComboable = false;
				GameManager.playerAnimator.SetTrigger("doComboAttack");	
				combosteep = (combosteep%3)+1;
		}
	}
	
		
	}

	public void enableHitbox() {
		if(!isFinished){
			
			GameManager.audioManager.PlaySound(3);
			
			stick.enabled = true;
			enemiesHit = new List<GameObject>(0);
			hasHitEnemy = false;
		}
	}
	
	public void allowChaining() {
		isComboable = true;
	}
	
	public void disableHitbox() {
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
		return isFinished;
	}

	public override bool canDoNextAttack()
	{
		return isFinished || isComboable;
	}

	public override void interruptAttack(){
		endCombo ();
	}


	
	void AnimationSubscriber.handleEvent(string idEvent) {
		switch (idEvent) {
		case "start": 
			enableHitbox();
			break;
		case "end":
			disableHitbox();
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

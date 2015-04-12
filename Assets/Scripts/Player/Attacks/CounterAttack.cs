using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CounterAttack : Attack {
	public int damage = 1;
	public float positionInFrontPlayer = 0.5f;
	public float timeToActivate = 0.1f;
	public float timeToDeactivate = 0.4f;
	private GameObject weapon;

	private Xft.XWeaponTrail weaponEffects;
	private float timeOfAnimation;

	private List<GameObject> enemiesHit;

	private bool hasHitEnemy;

	public override void initialize() {
		weapon = GameManager.player.GetComponent<PlayerController> ().weapon;
		weaponEffects = weapon.GetComponentInChildren<Xft.XWeaponTrail>();
	}

	public override void enemyCollisionEnter(GameObject enemy){
		if(!enemiesHit.Contains(enemy)){
			enemiesHit.Add(enemy);
			enemy.GetComponent<IAController>().getHurt(damage,(enemy.transform.position));
			enemy.GetComponent<IAController> ().interruptAttack ();
			GameManager.comboManager.addCombo ();
			if(!hasHitEnemy){
				hasHitEnemy = true;
				GameManager.lightGemEnergyManager.addPoints(1);
			}
		}
	}
	
	private IEnumerator doCounterAttack(){
		enemiesHit = new List<GameObject>(0);
		weaponEffects.Activate();
		GameManager.playerAnimator.SetTrigger("isDoingCounterAttack");
		hasHitEnemy = false;
		isFinished = false;
		yield return new WaitForSeconds(timeToActivate);
		weapon.GetComponentInChildren<AttackCollider> ().attack = gameObject;
		weapon.GetComponentInChildren<Collider> ().enabled = true;
		yield return new WaitForSeconds (timeToDeactivate);
		isFinished = true;
		weapon.GetComponentInChildren<AttackCollider> ().attack = gameObject;
		weapon.GetComponentInChildren<Collider> ().enabled = false;
		weaponEffects.StopSmoothly(0.1f);
	}
	
	public override void startAttack(){
		StartCoroutine ("doCounterAttack");
	}
}

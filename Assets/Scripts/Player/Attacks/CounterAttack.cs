using UnityEngine;
using System.Collections;

public class CounterAttack : Attack {
	public GameObject triggerBox;
	public int damage = 1;
	public float positionInFrontPlayer = 0.5f;
	public float timeToActivate = 0.1f;
	public float timeToDeactivate = 0.4f;

	private float timeOfAnimation;

	private bool hasHitEnemy;

	public override void enemyCollisionEnter(GameObject enemy){
		enemy.GetComponent<IAController>().getHurt(damage,(enemy.transform.position));
		enemy.GetComponent<IAController> ().interruptAttack ();
		GameManager.comboManager.addCombo ();
		if(!hasHitEnemy){
			hasHitEnemy = true;
			GameManager.lightGemEnergyManager.addPoints(1);
		}
	}
	
	private IEnumerator doCounterAttack(){
		GameManager.playerAnimator.SetTrigger("isDoingCounterAttack");
		hasHitEnemy = false;
		triggerBox.transform.position = GameManager.player.GetComponent<Rigidbody> ().worldCenterOfMass + GameManager.player.transform.forward.normalized * positionInFrontPlayer;
		isFinished = false;
		yield return new WaitForSeconds(timeToActivate);
		triggerBox.SetActive(true);
		yield return new WaitForSeconds (timeToDeactivate);
		isFinished = true;
		triggerBox.SetActive(false);
	}
	
	public override void startAttack(){
		StartCoroutine ("doCounterAttack");
	}
}

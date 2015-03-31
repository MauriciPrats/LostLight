using UnityEngine;
using System.Collections;

public class UppercutAttack : Attack {

	public GameObject triggerBox;
	public float positionInFrontPlayer = 0.5f;
	public float timeToActivate = 0.1f;
	public float timeToDeactivate = 0.4f;
	public int damage = 1;

	protected override void update(){

	}

	public override void enemyCollisionEnter(GameObject enemy){
		enemy.GetComponent<Rigidbody> ().velocity = enemy.transform.up * 4f;
		enemy.GetComponent<IAController>().getHurt(damage,(enemy.transform.position));
		enemy.GetComponent<IAController> ().stun (0.2f);
	}

	private IEnumerator doUppercut(){
		GameManager.playerAnimator.SetTrigger("isDoingUppercut");
		triggerBox.transform.position = GameManager.player.GetComponent<Rigidbody> ().worldCenterOfMass + GameManager.player.transform.forward.normalized * positionInFrontPlayer;
		isFinished = false;
		yield return new WaitForSeconds(timeToActivate);
		triggerBox.SetActive(true);
		yield return new WaitForSeconds (timeToDeactivate);
		isFinished = true;
		triggerBox.SetActive(false);
	}
	
	public override void startAttack(){
		StartCoroutine ("doUppercut");
	}

}

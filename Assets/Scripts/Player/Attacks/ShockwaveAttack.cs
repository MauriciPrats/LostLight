using UnityEngine;
using System.Collections;

public class ShockwaveAttack : Attack {

	public GameObject areaEffect;
	public GameObject chargeEffect;

	public float timeItDoesAttack = 0.8f;
	public float endScaleOfAttack = 4f;
	public float startChargeScale = 1f;
	public float forceExplosion = 5f;
	public float timeToCharge = 0.8f;

	private float timer;

	public override void enemyCollisionEnter(GameObject enemy){
		//If it's an enemy we damage him
		enemy.GetComponent<IAController>().getHurt(damage,enemy.transform.position);
		//We find the radius of areaEffect
		float radius = areaEffect.GetComponent<SphereCollider> ().radius * endScaleOfAttack;
		Vector3 position = areaEffect.transform.position;
		enemy.GetComponent<Rigidbody>().AddExplosionForce(forceExplosion,position,radius);
		enemy.GetComponent<IAController> ().stun (1f);
		GameManager.comboManager.addCombo ();
	}

	public override void initialize(){
		attackType = AttackType.Shockwave;
	}
	
	protected override void update(){

	}

	private IEnumerator growArea(){
		while(timer<timeItDoesAttack){
			timer += Time.deltaTime;
			float ratio = (timer/(timeItDoesAttack)) * endScaleOfAttack;
			areaEffect.transform.localScale = new Vector3(ratio,ratio,ratio);
			yield return null;
		}
		yield return true;
	}


	private IEnumerator makeSmallArea(){
		while (timer<timeToCharge) {
			timer += Time.deltaTime;
			float ratio = (1f - ((timer) / (timeToCharge))) * startChargeScale;
			chargeEffect.transform.localScale = new Vector3 (ratio, ratio, ratio);
			yield return null;
		}
		yield return true;
	}

	private IEnumerator doShockwave(){
		isFinished = false;
		areaEffect.transform.localScale = new Vector3(0f,0f,0f);
		chargeEffect.transform.localScale = new Vector3(startChargeScale,startChargeScale,startChargeScale);
		areaEffect.transform.position = GameManager.player.transform.position;
		chargeEffect.transform.position = GameManager.player.transform.position;
		chargeEffect.SetActive (true);
		GameManager.playerAnimator.SetTrigger("isChargingShockwave");

		timer = 0f;
		StartCoroutine ("makeSmallArea");
		yield return new WaitForSeconds (timeToCharge);

		areaEffect.SetActive (true);
		chargeEffect.SetActive (false);
		GameManager.playerAnimator.SetBool("isDoingShockwave",true);

		timer = 0f;
		StartCoroutine ("growArea");
		yield return new WaitForSeconds (timeItDoesAttack);

		isFinished = true;
		GameManager.playerAnimator.SetBool("isDoingShockwave",false);
		areaEffect.SetActive (false);
	}


	public override void startAttack(){
		StartCoroutine ("doShockwave");

	}
}

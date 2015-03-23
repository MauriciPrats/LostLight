using UnityEngine;
using System.Collections;

public class ShockwaveSpecialAttack : SpecialAttack {

	public GameObject areaEffect;
	public GameObject chargeEffect;

	public float timeItDoesAttack = 0.8f;
	public float endScaleOfAttack = 4f;
	public float startChargeScale = 1f;
	public int damageAmmount = 2;
	public float forceExplosion = 5f;
	public float timeToCharge = 0.8f;
	bool charged = false;

	private float timer;

	public override void enemyCollisionEnter(GameObject enemy){
		//If it's an enemy we damage him
		enemy.GetComponent<Killable>().Damage(damageAmmount);
		//We find the radius of areaEffect
		float radius = areaEffect.GetComponent<SphereCollider> ().radius * endScaleOfAttack;
		Vector3 position = areaEffect.transform.position;
		enemy.GetComponent<Rigidbody>().AddExplosionForce(forceExplosion,position,radius);
		enemy.GetComponent<IAController> ().stun (1f);
	}

	public override void initialize(){
		
	}
	
	protected override void update(){
		timer += Time.deltaTime;
		if(!isFinished){
			if(timer>timeToCharge){
				if(!charged){
					showShockwave();
					charged = true;
					hideCharge();
					GameManager.playerAnimator.SetBool("isDoingShockwave",true);
				}
				if(timer>=(timeItDoesAttack + timeToCharge)){
					isFinished = true;
					GameManager.playerAnimator.SetBool("isDoingShockwave",false);
					hideShockwave();
				}else{
					float ratio = ((timer - timeToCharge)/(timeItDoesAttack)) * endScaleOfAttack;
					areaEffect.transform.localScale = new Vector3(ratio,ratio,ratio);
				}
			}else{
				float ratio = (1f - ((timer)/(timeToCharge))) * startChargeScale;

				chargeEffect.transform.localScale = new Vector3(ratio,ratio,ratio);
			}
		}
	}

	private void hideShockwave(){
		areaEffect.SetActive (false);
	}

	private void showShockwave(){
		areaEffect.SetActive (true);
	}

	private void hideCharge(){
		chargeEffect.SetActive (false);
	}
	
	private void showCharge(){
		chargeEffect.SetActive (true);
	}

	public override void startAttack(){
		isFinished = false;
		timer = 0f;
		areaEffect.transform.localScale = new Vector3(0f,0f,0f);
		chargeEffect.transform.localScale = new Vector3(startChargeScale,startChargeScale,startChargeScale);
		areaEffect.transform.position = GameManager.player.transform.position;
		chargeEffect.transform.position = GameManager.player.transform.position;
		charged = false;
		showCharge();
		//Debug.Log ("Shockwaveee!!");
		GameManager.playerAnimator.SetTrigger("isChargingShockwave");

	}
}

using UnityEngine;
using System.Collections;

public class JabaliChargeAttack : AIAttack {


	public float timeItLastsCharge = 6f;
	public int damage = 1;	
	public GameObject animator;
	public float chargeSpeed = 6f;
	public GameObject enemy;
	public float velocityAppliedToPlayer = 2f;
	public float velocityAppliedToEnemy = 2f;
	public float timeBeforeCharge = 2f;
	public GameObject chargeParticles;
	public GameObject whileChargingParticles;

	private float attackTimer = 0f;
	private bool isAttacking = false;
	private bool isChargingAttack = false;
	private bool isDoingAttack = false;
	private float chargeAttackTimer = 0f;
	private Animator iaAnimator;
	private float originalSpeed = 0f;
	private float direction = 0f;
	
	private bool isPlayerInsideAttack = false;

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Player"){
			GameManager.player.GetComponent<Rigidbody>().velocity = GameManager.player.transform.up * velocityAppliedToPlayer;
			GameManager.player.GetComponent<PlayerController>().getHurt(1);
		}else if(col.gameObject.tag == "Enemy"){
			col.gameObject.GetComponent<Rigidbody>().velocity = col.gameObject.transform.up * velocityAppliedToEnemy;
		}
	}
	
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Player"){
			GameManager.player.GetComponent<Rigidbody>().velocity = GameManager.player.transform.up * velocityAppliedToEnemy;
			GameManager.player.GetComponent<PlayerController>().getHurt(1);
		}else if(col.gameObject.tag == "Enemy"){
			col.gameObject.GetComponent<Rigidbody>().velocity = col.gameObject.transform.up * velocityAppliedToEnemy;
		}
	}


	void Start(){
		iaAnimator = animator.GetComponent<Animator> ();
	}
	
	public override void startAttack(){
		bool isRight = enemy.GetComponent<IAController>().getIsLookingRight();
		if(isRight){
			direction = 1f;
		}else{
			direction = -1f;
		}
		isAttacking = true;
		StartCoroutine ("attack");
	}

	private IEnumerator moveStraight(){
		attackTimer = 0f;
		while(attackTimer<=timeItLastsCharge){
			attackTimer+=Time.deltaTime;
			enemy.GetComponent<CharacterController> ().Move(direction);
			yield return null;
		}
		yield return true;
	}

	private IEnumerator attack(){
		chargeParticles.SetActive (true);
		originalSpeed = enemy.GetComponent<CharacterController> ().speed;
		enemy.GetComponent<CharacterController> ().speed = chargeSpeed;
		iaAnimator.SetTrigger("isChargingChargeAttack");

		yield return new WaitForSeconds (timeBeforeCharge);
		chargeParticles.SetActive (false);
		iaAnimator.SetBool("isDoingChargeAttack",true);
		StartCoroutine ("moveStraight");
		GetComponent<Collider> ().enabled = true;
		whileChargingParticles.SetActive (true);
		yield return new WaitForSeconds(timeItLastsCharge);
		whileChargingParticles.SetActive (false);
		iaAnimator.SetBool("isDoingChargeAttack",false);
		isAttacking = false;
		enemy.GetComponent<CharacterController> ().speed = originalSpeed;
		GetComponent<Collider> ().enabled = false;
	}
	
	public override void doAttack(){

	}
	
	public override bool isAttackFinished(){
		return !isAttacking;
	}

	
	public override void interruptAttack(){

	}

}

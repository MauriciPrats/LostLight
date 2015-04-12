using UnityEngine;
using System.Collections;

public class JabaliChargeAttack : AIAttack {

	
	public float timeToChargeAttack = 0.5f;
	public float attackDuration = 0.1f;
	public float timeItLastsCharge = 6f;
	public int damage = 1;	
	public GameObject animator;
	public float chargeSpeed = 6f;
	public GameObject enemy;

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
			GameManager.player.GetComponent<Rigidbody>().AddForce(col.gameObject.transform.up * 8f,ForceMode.Impulse);
			GameManager.player.GetComponent<PlayerController>().getHurt(1);
		}else if(col.gameObject.tag == "Enemy"){
			col.gameObject.GetComponent<Rigidbody>().AddForce(col.gameObject.transform.up * 20f,ForceMode.Impulse);
		}
	}
	
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Player"){
			GameManager.player.GetComponent<Rigidbody>().AddForce(col.gameObject.transform.up * 8f,ForceMode.Impulse);
			GameManager.player.GetComponent<PlayerController>().getHurt(1);
		}else if(col.gameObject.tag == "Enemy"){
			Debug.Log("Aa2");
			col.gameObject.GetComponent<Rigidbody>().AddForce(col.gameObject.transform.up * 8f,ForceMode.Impulse);
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
		GetComponent<Collider> ().enabled = true;
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

		originalSpeed = enemy.GetComponent<CharacterController> ().speed;
		enemy.GetComponent<CharacterController> ().speed = chargeSpeed;
		StartCoroutine ("moveStraight");
		yield return new WaitForSeconds(timeItLastsCharge);

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

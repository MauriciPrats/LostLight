using UnityEngine;
using System.Collections;

public class JabaliChargeAttack : Attack {


	public float timeItLastsCharge = 6f;
	public float chargeSpeed = 6f;
	public float velocityAppliedToPlayer = 2f;
	public float velocityAppliedToEnemy = 2f;
	public float timeBeforeCharge = 2f;
	public Vector3 localPosition;
	public GameObject chargeParticles;
	public GameObject whileChargingParticles;

	private GameObject parent;
	private float attackTimer = 0f;
	private bool isChargingAttack = false;
	private bool isDoingAttack = false;
	private float chargeAttackTimer = 0f;
	private Animator iaAnimator;
	private float originalSpeed = 0f;
	private float direction = 0f;
	
	private bool isPlayerInsideAttack = false;

	public override void initialize(){
		attackType = AttackType.JabaliChargeAttack;
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Player"){
			GameManager.player.GetComponent<Rigidbody>().velocity = GameManager.player.transform.up * velocityAppliedToPlayer;
			GameManager.player.GetComponent<PlayerController>().getHurt(damage);
		}else if(col.gameObject.tag == "Enemy"){
			col.gameObject.GetComponent<Rigidbody>().velocity = col.gameObject.transform.up * velocityAppliedToEnemy;
		}
	}
	
	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Player"){
			GameManager.player.GetComponent<Rigidbody>().velocity = GameManager.player.transform.up * velocityAppliedToEnemy;
			GameManager.player.GetComponent<PlayerController>().getHurt(damage);
		}else if(col.gameObject.tag == "Enemy"){
			col.gameObject.GetComponent<Rigidbody>().velocity = col.gameObject.transform.up * velocityAppliedToEnemy;
		}
	}
	
	public override void startAttack(){
		bool isRight = parent.GetComponent<IAController>().getIsLookingRight();
		if(isRight){
			direction = 1f;
		}else{
			direction = -1f;
		}
		isFinished = false;
		StartCoroutine ("attack");
	}

	private IEnumerator moveStraight(){
		attackTimer = 0f;
		while(attackTimer<=timeItLastsCharge){
			attackTimer+=Time.deltaTime;
			parent.GetComponent<CharacterController> ().Move(direction);
			yield return null;
		}
		yield return true;
	}

	private IEnumerator attack(){
		chargeParticles.SetActive (true);
		originalSpeed = parent.GetComponent<CharacterController> ().speed;
		parent.GetComponent<CharacterController> ().speed = chargeSpeed;
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
		isFinished = true;
		parent.GetComponent<CharacterController> ().speed = originalSpeed;
		GetComponent<Collider> ().enabled = false;
	}

	
	public override void interruptAttack(){

	}

	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		//transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		transform.localPosition = localPosition;
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
	}
}

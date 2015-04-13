using UnityEngine;
using System.Collections;

public class JabaliFrontAttack : Attack {
	
	public float timeToChargeAttack = 0.5f;
	public float attackDuration = 0.1f;
	public GameObject particlesChargeAttack;
	public GameObject particlesAttack;
	public GameObject animator;
	public Vector3 localPosition;


	private float attackTimer = 0f;
	private bool isAttacking = false;
	private bool isChargingAttack = false;
	private bool isDoingAttack = false;
	private float chargeAttackTimer = 0f;
	private GameObject parent;
	private Animator iaAnimator;
	private bool isPlayerInsideAttack = false;


	public override void initialize(){
		attackType = AttackType.JabaliFrontAttack;
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Player"){
			isPlayerInsideAttack = true;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.tag == "Player"){
			isPlayerInsideAttack = false;
		}
	}

	public override void startAttack(){
		isChargingAttack = true;
		isAttacking = true;
	}

	protected override void update(){
		if(isChargingAttack){
			chargeAttack();
		}else if(isDoingAttack){
			attack();
		}
	}
	
	public override bool isAttackFinished(){
		return !isAttacking;
	}

	private void chargeAttack(){
		isInterruptable = true;
		chargeAttackTimer+=Time.deltaTime;
		iaAnimator.SetBool("isChargingAttack",true);
		particlesChargeAttack.GetComponent<ParticleSystem>().Play();
		if(chargeAttackTimer>=timeToChargeAttack){
			isChargingAttack = false;
			isDoingAttack = true;
			particlesChargeAttack.GetComponent<ParticleSystem>().Stop();
			iaAnimator.SetBool("isChargingAttack",false);
			particlesAttack.GetComponent<ParticleSystem>().Play();
			iaAnimator.SetBool("isDoingAttack",true);
		}
	}
	
	private void attack(){
		attackTimer+=Time.deltaTime;
		if(attackTimer>=attackDuration){
			particlesAttack.GetComponent<ParticleSystem>().Stop();
			isAttacking = false;
			isDoingAttack = false;
			chargeAttackTimer = 0f;
			attackTimer = 0f;
			attackEffect();
			iaAnimator.SetBool("isDoingAttack",false);
			iaAnimator.SetBool("isChargingAttack",false);
			isInterruptable = false;
		}
	}

	private void attackEffect(){
		isAttacking = false;
		if(isPlayerInsideAttack){
			GameManager.player.GetComponent<PlayerController>().getHurt(damage);
		}
	}

	public override void interruptAttack(){
		if(isInterruptable){
			isChargingAttack = false;
			isAttacking = false;
			iaAnimator.SetBool("isDoingAttack",false);
			iaAnimator.SetBool("isChargingAttack",false);
			particlesChargeAttack.GetComponent<ParticleSystem>().Stop();
			isAttacking = false;
			isDoingAttack = false;
			chargeAttackTimer = 0f;
			attackTimer = 0f;
			isInterruptable = false;
		}
	}

	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		transform.localPosition = localPosition;
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
	}

}

using UnityEngine;
using System.Collections;

public class JabaliFrontAttack : AIAttack {
	
	public float timeToChargeAttack = 0.5f;
	public float attackDuration = 0.1f;
	public int damage = 1;	
	public GameObject particlesChargeAttack;
	public GameObject particlesAttack;
	public GameObject animator;


	private float attackTimer = 0f;
	private bool isAttacking = false;
	private bool isChargingAttack = false;
	private bool isDoingAttack = false;
	private float chargeAttackTimer = 0f;
	private Animator iaAnimator;

	private bool isPlayerInsideAttack = false;

	void Start(){
		iaAnimator = animator.GetComponent<Animator> ();
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

	public override void doAttack(){
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
		isInterruptableNow = true;
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
			isInterruptableNow = false;
		}
	}

	private void attackEffect(){
		isAttacking = false;
		if(isPlayerInsideAttack){
			GameManager.player.GetComponent<PlayerController>().getHurt(damage);
		}
	}

	public override void interruptAttack(){
		Debug.Log ("interrupted");
		if(isInterruptableNow){
			isChargingAttack = false;
			isAttacking = false;
			iaAnimator.SetBool("isDoingAttack",false);
			iaAnimator.SetBool("isChargingAttack",false);
			particlesChargeAttack.GetComponent<ParticleSystem>().Stop();
			isAttacking = false;
			isDoingAttack = false;
			chargeAttackTimer = 0f;
			attackTimer = 0f;
			isInterruptableNow = false;
		}
	}


}

using UnityEngine;
using System.Collections;

public class BoarFrontAttack : Attack {
	
	public float timeToChargeAttack = 0.5f;
	public float attackDuration = 0.1f;
	public GameObject particlesAttack;
	public GameObject animator;


	private float attackTimer = 0f;
	private bool isChargingAttack = false;
	private bool isDoingAttack = false;
	private float chargeAttackTimer = 0f;
	private GameObject parent;
	private Animator iaAnimator;
	private bool isPlayerInsideAttack = false;

	private OutlineChanging outlineChanger;


	public override void initialize(){
		attackType = AttackType.BoarFrontAttack;
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
		isFinished = false;
		iaAnimator.SetTrigger ("isChargingHeadbutt");
	}

	protected override void update(){
		if(isChargingAttack){
			chargeAttack();
		}else if(isDoingAttack){
			attack();
		}
	}

	private void chargeAttack(){
		isInterruptable = true;
		chargeAttackTimer+=Time.deltaTime;
		float ratio = chargeAttackTimer / timeToChargeAttack;
		Color newColor = Color.Lerp (Color.black, Color.red, ratio);
		outlineChanger.setOutlineColor (newColor);
		if(chargeAttackTimer>=timeToChargeAttack){
			outlineChanger.setOutlineColor (Color.black);
			isChargingAttack = false;
			isDoingAttack = true;
			iaAnimator.SetTrigger ("isDoingHeadbutt");
			GameManager.audioManager.PlaySound(5);
			particlesAttack.GetComponent<ParticleSystem>().Play();
		}
	}
	
	private void attack(){
		attackTimer+=Time.deltaTime;
		if(attackTimer>=attackDuration){
			particlesAttack.GetComponent<ParticleSystem>().Stop();
			isFinished = true;
			isDoingAttack = false;
			chargeAttackTimer = 0f;
			attackTimer = 0f;
			attackEffect();
			isInterruptable = false;
		}
	}

	private void attackEffect(){
		if(isPlayerInsideAttack){
			GameManager.player.GetComponent<PlayerController>().getHurt(damage);
		}
	}

	public override void interruptAttack(){
		if(isInterruptable){
			outlineChanger.setOutlineColor (Color.black);
			iaAnimator.ResetTrigger ("isChargingHeadbutt");
			isChargingAttack = false;
			isFinished = true;
			isDoingAttack = false;
			chargeAttackTimer = 0f;
			attackTimer = 0f;
			isInterruptable = false;
		}
	}

	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		transform.position = parentObject.GetComponent<Rigidbody>().worldCenterOfMass + (parentObject.transform.forward*parentObject.GetComponent<WalkOnMultiplePaths>().centerToExtremesDistance*1.5f);
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
		outlineChanger = parent.GetComponent<OutlineChanging> ();
	}

}

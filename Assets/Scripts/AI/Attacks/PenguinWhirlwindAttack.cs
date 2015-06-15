﻿using UnityEngine;
using System.Collections;

public class PenguinWhirlwindAttack : Attack {

	public float timeToCharge = 0.8f;

	public float timeItLasts = 1f;
	public float rotationSpeed = 360f;
	public float speedToAddV = 4f;
	public float timeTillItDoesDamage = 0.2f;
	public GameObject particlesWhirlwind;

	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	private bool hasHitPlayer = false;
	private bool isAttackHurting = false;
	
	public override void initialize(){
		attackType = AttackType.PenguinWhirlwindAttack;
	}

	void OnTriggerStay(Collider enemy){
		if(enemy.gameObject.tag.Equals("Player") && !isFinished && !hasHitPlayer && isAttackHurting && !parent.GetComponent<IAController>().isDead){
			hasHitPlayer = true;
			GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
			Vector3 speedToAdd = GameManager.player.GetComponent<Rigidbody>().worldCenterOfMass - parent.transform.position;
			GameManager.player.GetComponent<Rigidbody>().velocity +=(speedToAdd.normalized * speedToAddV);
		}
	}
	
	public override void startAttack(){
		if(isFinished){
			StartCoroutine("doAttack");
			isFinished = false;
			hasHitPlayer = true;
			isAttackHurting = false;
		}
	}

	IEnumerator doAttack(){
		float timer = 0f;
		iaAnimator.SetTrigger("isChargingWhirlwind");
		while(timer<timeToCharge){
			timer+=Time.deltaTime;
			float ratio = timer/timeToCharge;
			outlineChanger.setOutlineColor(Color.Lerp(Color.black,Color.red,ratio));
			yield return null;
		}
		timer = 0f;
		iaAnimator.SetBool("isDoingWhirlwind",true);
		bool activatedHit = false;
		while (timer<timeItLasts) {
			timer+=Time.deltaTime;
			if(timer>=timeTillItDoesDamage && !isAttackHurting && !activatedHit){
				isAttackHurting = true;
				hasHitPlayer = false;
				activatedHit = true;
				particlesWhirlwind.GetComponent<ParticleSystem>().Play();
			}

			if(timer>= (timeItLasts - timeTillItDoesDamage) && isAttackHurting){
				isAttackHurting = false;
				particlesWhirlwind.GetComponent<ParticleSystem>().Stop();
				outlineChanger.setOutlineColor(Color.black);
			}
			transform.up = parent.transform.up;
			//transform.RotateAround(parent.GetComponent<Rigidbody>().worldCenterOfMass,parent.transform.up,rotationSpeed*Time.deltaTime);
			yield return null;
		}
		iaAnimator.SetBool("isDoingWhirlwind",false);

		isFinished = true;
	}
	
	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		transform.position = parentObject.GetComponent<Rigidbody>().worldCenterOfMass + (parentObject.transform.forward*parentObject.GetComponent<WalkOnMultiplePaths>().centerToExtremesDistance*1.3f);
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
		outlineChanger = parent.GetComponent<OutlineChanging> ();
	}
}

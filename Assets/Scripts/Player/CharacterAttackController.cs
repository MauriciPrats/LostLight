﻿using UnityEngine;
using System.Collections;


public enum AttackType{Missiles,Kame,Shockwave,GuardBreaker,CounterAttack,OnAir,Combo};

public class CharacterAttackController : MonoBehaviour {


	public GameObject attackMissilesGO;
	public GameObject attackKameGO;
	public GameObject attackShockwaveGO;
	public GameObject attackGuardBreakerGO;
	public GameObject attackOnAirGO;
	public GameObject attackComboGO;
	public GameObject attackCounterGO;

	public GameObject blockGO;
	public GameObject dashGO;

	private Attack attackMissiles;
	private Attack attackKame;
	private Attack attackShockwave;
	private Attack attackGuardBreaker;
	private Attack attackOnAir;
	private Attack attackCombo;
	private Attack attackCounter;

	private Block block;
	private Dash dash;
	
	// Initialization
	void Start () {
		attackMissiles = (GameObject.Instantiate(attackMissilesGO) as GameObject).GetComponent<Attack> ();
		attackKame = (GameObject.Instantiate(attackKameGO) as GameObject).GetComponent<Attack> ();
		attackShockwave = (GameObject.Instantiate(attackShockwaveGO) as GameObject).GetComponent<Attack> ();
		attackGuardBreaker = (GameObject.Instantiate(attackGuardBreakerGO) as GameObject).GetComponent<Attack> ();
		attackOnAir = (GameObject.Instantiate(attackOnAirGO) as GameObject).GetComponent<Attack> ();
		attackCounter = (GameObject.Instantiate(attackCounterGO) as GameObject).GetComponent<Attack> ();
		block = (GameObject.Instantiate (blockGO) as GameObject).GetComponent<Block> ();
		dash = (GameObject.Instantiate(dashGO) as GameObject).GetComponent<Dash> ();
		attackCombo = (GameObject.Instantiate(attackComboGO) as GameObject).GetComponent<Attack> ();	
	}

	public Attack getAttack(AttackType aType){
		if(aType.Equals(AttackType.Missiles)){
			return attackMissiles;
		}else if(aType.Equals(AttackType.Kame)){
			return attackKame;
		}else if(aType.Equals(AttackType.Shockwave)){
			return attackShockwave;
		}else if(aType.Equals(AttackType.GuardBreaker)){
			return attackGuardBreaker;
		}else if(aType.Equals(AttackType.CounterAttack)){
			return attackCounter;
		}else if(aType.Equals(AttackType.OnAir)){
			return attackOnAir;
		}else if(aType.Equals(AttackType.Combo)){
			return attackCombo;
		}
		return null;
	}

	public void Update() {

	}

	public bool isDoingDash(){
		return dash.getIsDoingDash ();
	}

	public bool isDashOnCooldown(){
		return !dash.isCooldownFinished ();
	}

	public void doDash(){
		if(dash.isCooldownFinished()){
			dash.startAction();
		}
	}

	public void  doAttack(AttackType aType){
		Attack attackToDo = getAttack (aType);

		if(attackToDo!=null){
			if(attackToDo.canPayAttackCost()){
				GameManager.lightGemEnergyManager.substractPoints(attackToDo.lightPointsCost);
				attackToDo.startAttack ();
			}
		}else{
			Debug.Log("No attack exists for the type given");
		}
	}

	public void doBlock() {
		block.StartBlock();
	}

	public bool isDoingAnyAttack(){
		if(!attackMissiles.isAttackFinished()){
			return true;
		}else if(!attackKame.isAttackFinished()){
			return true;
		}else if(!attackShockwave.isAttackFinished()){
			return true;
		}else if(!attackGuardBreaker.isAttackFinished()){
			return true;
		}else if(!attackOnAir.isAttackFinished()){
			return true;
		}else if(!attackCombo.isAttackFinished()){
			return true;
		}else if(!attackCounter.isAttackFinished()){
			return true;
		}
		return false;
	}
}

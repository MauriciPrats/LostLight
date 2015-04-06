using UnityEngine;
using System.Collections;


public enum AttackType{Missiles,Kame,Shockwave,Uppercut,OnAir,Combo};

public class CharacterAttackController : MonoBehaviour {

	public Transform leftHand; //Ubicacion de la mano izquierda


	public GameObject attackMissilesGO;
	public GameObject attackKameGO;
	public GameObject attackShockwaveGO;
	public GameObject attackUppercutGO;
	public GameObject attackOnAirGO;
	public GameObject attackComboGO;

	public GameObject dashGO;

	private Attack attackMissiles;
	private Attack attackKame;
	private Attack attackShockwave;
	private Attack attackUppercut;
	private Attack attackOnAir;
	private Attack attackCombo;

	private Dash dash;
	
	
	// Initialization
	void Start () {
		attackMissiles = (GameObject.Instantiate(attackMissilesGO) as GameObject).GetComponent<Attack> ();
		attackKame = (GameObject.Instantiate(attackKameGO) as GameObject).GetComponent<Attack> ();
		attackShockwave = (GameObject.Instantiate(attackShockwaveGO) as GameObject).GetComponent<Attack> ();
		attackUppercut = (GameObject.Instantiate(attackUppercutGO) as GameObject).GetComponent<Attack> ();
		attackOnAir = (GameObject.Instantiate(attackOnAirGO) as GameObject).GetComponent<Attack> ();
		
		dash = (GameObject.Instantiate(dashGO) as GameObject).GetComponent<Dash> ();
		
		attackCombo = attackComboGO.GetComponent<Attack>();
//		weapon.transform.parent = leftHand.transform;
//		weapon.transform.position = leftHand.transform.position;
//		weapon.transform.rotation = leftHand.transform.rotation;
//		attackCombo = weapon.GetComponent<Attack>();
		
	}

	public Attack getAttack(AttackType aType){
		if(aType.Equals(AttackType.Missiles)){
			return attackMissiles;
		}else if(aType.Equals(AttackType.Kame)){
			return attackKame;
		}else if(aType.Equals(AttackType.Shockwave)){
			return attackShockwave;
		}else if(aType.Equals(AttackType.Uppercut)){
			return attackUppercut;
		}else if(aType.Equals(AttackType.OnAir)){
			return attackOnAir;
		}else if(aType.Equals(AttackType.Combo)){
			return attackCombo;
		}
		return null;
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
	public bool isDoingAnyAttack(){
		if(!attackMissiles.isAttackFinished()){
			return true;
		}else if(!attackKame.isAttackFinished()){
			return true;
		}else if(!attackShockwave.isAttackFinished()){
			return true;
		}else if(!attackUppercut.isAttackFinished()){
			return true;
		}else if(!attackOnAir.isAttackFinished()){
			return true;
		}else if(!attackCombo.isAttackFinished()){
			return true;
		}
		return false;
	}
}

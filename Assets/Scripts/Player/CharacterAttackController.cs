using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttackType{Missiles,Kame,Shockwave,GuardBreaker,CounterAttack,OnAir,Combo,JabaliFrontAttack,JabaliChargeAttack};

public class CharacterAttackController : MonoBehaviour {

	//Attacks
	public GameObject[] attacksGO;

	private List<Attack> attacksInCharacter;

	public GameObject blockGO;
	public GameObject dashGO;

	private Block block;
	private Dash dash;
	
	// Initialization
	void Awake () {
		attacksInCharacter = new List<Attack> (0);
		foreach(GameObject attackGO in attacksGO){
			Attack attack = (Instantiate(attackGO) as GameObject).GetComponent<Attack>();
			attacksInCharacter.Add(attack);
		}

		if(blockGO!=null){block = (GameObject.Instantiate (blockGO) as GameObject).GetComponent<Block> ();}
		if(dashGO!=null){dash = (GameObject.Instantiate(dashGO) as GameObject).GetComponent<Dash> ();}
	}

	public Attack getAttack(AttackType aType){
		foreach(Attack attack in attacksInCharacter){
			if(aType.Equals (attack.getAttackType())){
				return attack;
			}
			Debug.Log(attack.getAttackType());
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

	public bool isDoingBlock(){
		return block.getIsDoingBlock();
	}

	public bool isBlockOnCooldown(){
		return !block.isCooldownFinished ();
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
				GameManager.lightGemEnergyManager.substractPoints(attackToDo.cost);
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
		foreach(Attack attack in attacksInCharacter){
			if(!attack.isAttackFinished()){return true;}
		}

		return false;
	}

	public void interruptActualAttacks(){
		foreach(Attack attack in attacksInCharacter){
			if(!attack.isAttackFinished()){attack.interruptAttack();}
		}
	}
}

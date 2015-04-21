using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

/**
Ciclo de vida de un ataque:
Al inicio de la escena characterattackcontroller instancia y almacena los prefabs
Se llaman los initialize de cada ataque

*/


	protected bool isFinished = true;
	public int damage = 1;
	public int cost = 0;
	public ElementType elementAttack = ElementType.None;
	protected bool isInterruptable;
	protected AttackType attackType;

	// Use this for initialization
	void Start () {
		initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		update ();
	}

	public virtual void enemyCollisionEnter(GameObject enemy){
	}
	
	public virtual void enemyCollisionExit(GameObject enemy){
	}

	public virtual void otherCollisionExit(GameObject enemy){
	}

	public virtual void otherCollisionEnter(GameObject enemy){
	}

	public virtual void informParent(GameObject parentObject){

	}

	public virtual void initialize(){
	}

	protected virtual void update(){
	}

	public virtual void startAttack(){
	}

	public virtual bool isAttackFinished(){
		return isFinished;
	}

	public bool canPayAttackCost(){
		return GameManager.lightGemEnergyManager.canDoSpecialAttack (cost);
	}

	public virtual void interruptAttack(){
		
	}

	public AttackType getAttackType(){
		return attackType;
	}
}

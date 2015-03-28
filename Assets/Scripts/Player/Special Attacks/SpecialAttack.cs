using UnityEngine;
using System.Collections;

public class SpecialAttack : MonoBehaviour {

	protected bool isFinished = true;
	public int lightPointsCost = 0;
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


	public virtual void initialize(){
	}

	protected virtual void update(){
	}

	public virtual void startAttack(){
	}

	public bool isSpecialAttackFinished(){
		return isFinished;
	}

	public bool canPaySpecialAttackCost(){
		return GameManager.lightGemEnergyManager.canDoSpecialAttack (lightPointsCost);
	}
}

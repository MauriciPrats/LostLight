using UnityEngine;
using System.Collections;

public class SpecialAttack : MonoBehaviour {

	protected bool isFinished = true;
	// Use this for initialization
	void Start () {
		initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		update ();
	}

	public virtual void enemyCollisionEnter(GameObject enemy){
		Debug.Log ("Empty Enemy collision of parent special attack");
	}
	
	public virtual void enemyCollisionExit(GameObject enemy){
		Debug.Log ("Empty Enemy collision of parent special attack");
	}

	public virtual void otherCollisionExit(GameObject enemy){
		Debug.Log ("Empty Enemy collision of parent special attack");
	}

	public virtual void otherCollisionEnter(GameObject enemy){
		Debug.Log ("Empty Enemy collision of parent special attack");
	}


	public virtual void initialize(){
		Debug.Log ("Empty initialize of parent special attack");
	}

	protected virtual void update(){
		Debug.Log ("Empty update of parent special attack");
	}

	public virtual void startAttack(){
		Debug.Log ("Empty start attack of parent special attack");
	}

	public bool isSpecialAttackFinished(){
		return isFinished;
	}
}

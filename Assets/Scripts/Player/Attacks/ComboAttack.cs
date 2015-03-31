using UnityEngine;
using System.Collections;

public class ComboAttack : Attack {


	public override void enemyCollisionEnter(GameObject enemy){
		//Called when an AttackCollider with this object associated enters collision with an enemy

	}

	protected override void update(){
		//just if update is needed, might be unecessary if coroutines are used
		
	}


	public override void startAttack(){
		//Combo logic, calling coroutine or whatever
		Debug.Log ("Combo attack debug");
	}
}

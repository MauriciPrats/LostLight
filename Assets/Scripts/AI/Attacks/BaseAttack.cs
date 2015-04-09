using UnityEngine;
using System.Collections;

public class BaseAttack : MonoBehaviour {

	public int attackValue;

	protected bool isInterruptableNow = false;


	public virtual void startAttack(){
	}

	public virtual void doAttack(){}

	public virtual bool isAttackFinished(){return true;}

	public int getAttackValue(){
		return attackValue;
	}

	public virtual void interruptAttack(){

	}

}

using UnityEngine;
using System.Collections;

public class BaseAttack : MonoBehaviour {

	public int attackValue;


	public virtual void startAttack(){
	}

	public virtual void doAttack(){}

	public virtual bool isAttackFinished(){return true;}

	public int getAttackValue(){
		return attackValue;
	}

}

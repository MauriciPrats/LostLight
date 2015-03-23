using UnityEngine;
using System.Collections;

public class SpecialAttackCollider : MonoBehaviour {

	public GameObject attack;

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Enemy")){
			attack.GetComponent<SpecialAttack>().enemyCollisionEnter(other.gameObject);
		}else{
			attack.GetComponent<SpecialAttack>().otherCollisionEnter(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag.Equals("Enemy")){
			attack.GetComponent<SpecialAttack>().enemyCollisionExit(other.gameObject);
		}else{
			attack.GetComponent<SpecialAttack>().otherCollisionExit(other.gameObject);
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag.Equals("Enemy")){
			attack.GetComponent<SpecialAttack>().enemyCollisionEnter(other.gameObject);
		}else{
			attack.GetComponent<SpecialAttack>().otherCollisionEnter(other.gameObject);
		}
	}
	
	void OnCollisionExit(Collision other) {
		if(other.gameObject.tag.Equals("Enemy")){
			attack.GetComponent<SpecialAttack>().enemyCollisionExit(other.gameObject);
		}else{
			attack.GetComponent<SpecialAttack>().otherCollisionExit(other.gameObject);
		}
	}

}

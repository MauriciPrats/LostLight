using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {

	public GameObject attack;

	void OnTriggerEnter(Collider other) {
		if(attack!=null){
			if(other.tag.Equals("Enemy")){
				attack.GetComponent<Attack>().enemyCollisionEnter(other.gameObject,other.ClosestPointOnBounds(transform.position));
			}else{
				attack.GetComponent<Attack>().otherCollisionEnter(other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(attack!=null){
			if(other.tag.Equals("Enemy")){
				attack.GetComponent<Attack>().enemyCollisionExit(other.gameObject);
			}else{
				attack.GetComponent<Attack>().otherCollisionExit(other.gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if(attack!=null){
			if(other.gameObject.tag.Equals("Enemy")){
				attack.GetComponent<Attack>().enemyCollisionEnter(other.gameObject,other.contacts[0].point);
			}else{
				attack.GetComponent<Attack>().otherCollisionEnter(other.gameObject);
				attack.GetComponent<Attack>().otherCollisionEnter(other);
			}
		}
	}
	
	void OnCollisionExit(Collision other) {
		if(attack!=null){
			if(other.gameObject.tag.Equals("Enemy")){
				attack.GetComponent<Attack>().enemyCollisionExit(other.gameObject);
			}else{
				attack.GetComponent<Attack>().otherCollisionExit(other.gameObject);
			}
		}
	}

}

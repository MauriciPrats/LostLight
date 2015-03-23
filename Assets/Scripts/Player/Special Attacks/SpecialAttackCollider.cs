using UnityEngine;
using System.Collections;

public class SpecialAttackCollider : MonoBehaviour {

	public GameObject attack;

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Enemy")){
			attack.GetComponent<SpecialAttack>().enemyCollision(other.gameObject);
		}
	}
}

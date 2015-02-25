using UnityEngine;
using System.Collections;

public class WeaponColider : MonoBehaviour {

	public int damage = 1;
	public Vector3 direction;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			Killable enemy = other.GetComponent<Killable>();
			enemy.Damage(damage);
		}
	}
	
}

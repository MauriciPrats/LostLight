using UnityEngine;
using System.Collections;

public class WeaponColider : MonoBehaviour {

	public int damage = 1;
	public Vector3 direction;
	public GameObject particleEffectPrefab;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			Killable enemy = other.GetComponent<Killable>();

			enemy.Damage(damage);

			if(particleEffectPrefab!=null){
				Vector3 positionParticles = (enemy.transform.position + transform.position)/2f;
				GameObject newParticles = GameObject.Instantiate(particleEffectPrefab) as GameObject;
				newParticles.transform.position = positionParticles;
			}
		}
	}
	
}

using UnityEngine;
using System.Collections;

public class DestroyRocksOnHit : MonoBehaviour {

	public int numberOfHitsNecessary = 3;
	public float cooldownHit = 0.5f;
	private int hitsMade = 0;
	private float lastHit;
	void Start(){
		lastHit = Time.time;
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
			if(((Time.time - lastHit)>cooldownHit)){
				lastHit = Time.time;
				hitsMade++;
				if(hitsMade>=numberOfHitsNecessary){
					GetComponentInParent<FirstPlanetBlockPathRocks>().informRocksHit();
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GravityBody))]
public class PlatformAbsorbed : MonoBehaviour {
	public float speed = 3f;

	// Use this for initialization
	void Start () {
		GetComponent<GravityBody> ().setHasToApplyForce (false);
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Planets"))) {
			Destroy(gameObject);
		}else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy"))) {
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.position -= speed * transform.up * Time.deltaTime;
	}
}

using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public bool isRotating = false;
	public float speedRotation = 30;
	private GameObject AthmospherePrefab;
	void Start(){
		//We create an athmosphere for this gravity attractor

		AthmospherePrefab = (GameObject)Resources.Load ("Athmosphere");
		GameObject newAthmosphere = (GameObject)Instantiate (AthmospherePrefab, transform.position, Quaternion.identity);
		newAthmosphere.transform.parent = transform.gameObject.transform;

		//We calculate the size of the athmosphere of the gravityAttractor
		float size = 0.5f * Mathf.Max(transform.lossyScale.x,transform.lossyScale.y,transform.lossyScale.z);
		float factor = (size + Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR) / size;
		newAthmosphere.transform.localScale = new Vector3(factor,factor,factor);
	}

	void FixedUpdate(){
		if (isRotating) {
			//print (Vector3.Distance (transform.parent.position, transform.position));
			transform.position = OrbiteAroundPoint (transform.position, transform.parent.position, Quaternion.Euler (0, 0, speedRotation * Time.deltaTime));
		}
	}

	public bool Attract (Transform objectToAttract){
		//Only attract the body to the planet if it is close enough.
		float distance = Vector3.Distance (transform.position, objectToAttract.position);
		GravityBody body = objectToAttract.GetComponent<GravityBody> ();

		SphereCollider sphereCollider = (SphereCollider) transform.gameObject.GetComponent (typeof(SphereCollider));
		float sphereRadius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
		distance = distance - sphereRadius;

		if (distance <= Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR) {
			Vector3 targetDir = (objectToAttract.position - transform.position).normalized;
			Vector3 objectUp = objectToAttract.up;
			objectToAttract.rotation = Quaternion.FromToRotation (objectUp, targetDir) * objectToAttract.rotation;
			objectToAttract.rigidbody.AddForce (targetDir * -Constants.GRAVITY_FORCE_OF_PLANETS,ForceMode.Acceleration);
			//We only put the body in the hierarchy if it has touched a planet after doing "Space travel".
			if(!body.getUsesSpaceGravity()){
				objectToAttract.parent = transform;
			}

			return true;
		}
		return false;
	}

	private Vector3 OrbiteAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * ( point - pivot) + pivot;
	}
}

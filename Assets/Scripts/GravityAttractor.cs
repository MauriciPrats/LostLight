using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public bool isRotating = false;
	public float speedRotation = 30;
	public GameObject athmosphere;
	public GameObject athmosphereMinimapPrefab;
	void Awake(){

		GravityBodiesManager.registerNewBody (this.gameObject);

		//We create an athmosphere for this gravity attractor
	

		/*AthmospherePrefab = (GameObject)Resources.Load ("Athmosphere");
		GameObject newAthmosphere = (GameObject)Instantiate (AthmospherePrefab, transform.position, Quaternion.identity);
		newAthmosphere.transform.parent = transform.gameObject.transform;*/

		if (athmosphere != null) {
			//We calculate the size of the athmosphere of the gravityAttractor
			float size = transform.GetComponent<SphereCollider> ().radius * Mathf.Max (transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
			size += Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR;

			//Athmosphere size
			float athmosphereSize = athmosphere.transform.GetComponent<SphereCollider> ().radius * Mathf.Max (athmosphere.transform.lossyScale.x, athmosphere.transform.lossyScale.y, athmosphere.transform.lossyScale.z);

			float factor = size / athmosphereSize;
			athmosphere.transform.localScale = new Vector3 (factor*athmosphere.transform.localScale.x, factor*athmosphere.transform.localScale.y, factor*athmosphere.transform.localScale.z);
			GameObject athmosphereMinimap = (GameObject)GameObject.Instantiate(athmosphereMinimapPrefab);
			athmosphereMinimap.transform.position = new Vector3(transform.position.x,transform.position.y,Constants.MINIMAP_DISTANCE);
			athmosphereMinimap.transform.parent = transform;
			//factor = athmosphereSize / 5f;
			factor = size / (athmosphereMinimap.GetComponent<MeshRenderer>().bounds.size.x / 2f);
			athmosphereMinimap.transform.localScale = new Vector3(factor * athmosphereMinimap.transform.localScale.x,factor * athmosphereMinimap.transform.localScale.y,factor * athmosphereMinimap.transform.localScale.z);
			
			//Debug.Log (athmosphereMinimap.GetComponent<MeshRenderer>().bounds.size.x);
		}
	}

	void FixedUpdate(){
		if (isRotating) {
			//print (Vector3.Distance (transform.parent.position, transform.position));
			transform.position = OrbiteAroundPoint (transform.position, transform.parent.position, Quaternion.Euler (0, 0, speedRotation * Time.deltaTime));
		}
	}

	public bool Attract (Transform objectToAttract,out float distance){
		//Only attract the body to the planet if it is close enough.
		distance = Vector3.Distance (transform.position, objectToAttract.position);
		GravityBody body = objectToAttract.GetComponent<GravityBody> ();

		SphereCollider sphereCollider = (SphereCollider) transform.gameObject.GetComponent (typeof(SphereCollider));
		float sphereRadius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
		distance = distance - sphereRadius;

		if (distance <= Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR) {
			Vector3 targetDir = (objectToAttract.position - transform.position);
			targetDir = new Vector3(targetDir.x,targetDir.y,0f).normalized;

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

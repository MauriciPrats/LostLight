using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public LayerMask objectsToCollide;
	
	GameObject[] planets;
	bool isTouchingPlanet;
	bool usesSpaceGravity;
	float minimumPlanetDistance = float.MaxValue;

	void Start() {
		planets = GravityBodiesManager.getGravityBodies ();
	}

	void OnTriggerEnter (Collider col)
	{
		checkTouchEnter (col.gameObject);
	}

	void OnCollisionEnter (Collision col)
	{
		checkTouchEnter (col.gameObject);
	}

	void OnTriggerExit(Collider col)
	{
		checkTouchExit (col.gameObject);
	}
	
	void OnCollisionExit(Collision col)
	{
		checkTouchExit (col.gameObject);
	}
	

	void checkTouchEnter(GameObject obj){
		if (obj.tag == "Planet") {
			rigidbody.drag = Constants.DRAG_ON_TOUCH_PLANETS;
			isTouchingPlanet = true;
			usesSpaceGravity = false;
		}
	}

	void checkTouchExit(GameObject obj){
		if (obj.tag == "Planet") {
			rigidbody.drag = 0f;
			isTouchingPlanet = false;
		}
	}

	void FixedUpdate() {
		transform.parent = null;
		int closePlanets = 0;
		minimumPlanetDistance = float.MaxValue;
		foreach (GameObject planet in planets) {
			GravityAttractor gravityAttractor = planet.GetComponent<GravityAttractor> ();
			float distance = 0f;
			if(gravityAttractor.Attract (transform,out distance)){
				closePlanets++;
			}
			if(distance<minimumPlanetDistance){
				minimumPlanetDistance = distance;
			}
		}
		if (closePlanets == 0) 
		{
			usesSpaceGravity = true;
			rigidbody.drag = 0f;
		} 
		else 
		{
			rigidbody.drag = Constants.GRAVITY_DRAG_OF_ATHMOSPHERE;
		}
	}

	public bool getIsTouchingPlanet(){
		return isTouchingPlanet;
	}

	public bool getUsesSpaceGravity(){
		return usesSpaceGravity;
	}

	public float getMinimumPlanetDistance(){
		return minimumPlanetDistance;
	}
	/*public bool isGrounded(){
		
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 0.5f + 0.1f, objectsToCollide )) {
			return true;
		}

		return false;
	}*/
}

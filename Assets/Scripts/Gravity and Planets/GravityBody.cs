using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public LayerMask objectsToCollide;
	
	GameObject[] planets;
	bool isTouchingPlanet;
	bool usesSpaceGravity;
	bool isOutsideAthmosphere;
	float minimumPlanetDistance = float.MaxValue;

	int objectsTouching = 0;
	void Start() {
		planets = GravityBodiesManager.getGravityBodies ();
	}

	/*void OnTriggerEnter (Collider col)
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
	}*/
	

	public void checkTouchEnter(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			objectsTouching++;
			rigidbody.drag = Constants.DRAG_ON_TOUCH_PLANETS;
			isTouchingPlanet = true;
			usesSpaceGravity = false;
		}
	}

	public void checkTouchExit(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			objectsTouching--;
			if(objectsTouching==0){
				rigidbody.drag = 0f;
				isTouchingPlanet = false;
			}
		}
	}

	void FixedUpdate() {
		attract ();
	}

	public void attract(){
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
			isOutsideAthmosphere = true;
		} 
		else 
		{
			isOutsideAthmosphere = false;
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

	public bool getIsOutsideAthmosphere(){
		return isOutsideAthmosphere;
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

using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public LayerMask objectsToCollide;
	
	GameObject[] planets;
	bool isTouchingPlanet;
	bool usesSpaceGravity;

	void Awake() {
		planets = GameObject.FindGameObjectsWithTag ("Planet");
	}

	void OnCollisionEnter (Collision col)
	{
		//If we collide with a planet we put the drag of the player to the defined constant
		//if (col.gameObject.tag == "Planet") {
			rigidbody.drag = Constants.DRAG_ON_TOUCH_PLANETS;
			isTouchingPlanet = true;
			usesSpaceGravity = false;
		//}
	}

	void OnCollisionExit(Collision col){

		//If we stop colliding we put the drag back to 0 to give some "space feeling"
		if (col.gameObject.tag == "Planet") {
			rigidbody.drag = 0f;
			isTouchingPlanet = false;
		}
	}

	void FixedUpdate() {
		transform.parent = null;
		int closePlanets = 0;
		foreach (GameObject planet in planets) {
			GravityAttractor gravityAttractor = planet.GetComponent<GravityAttractor> ();
			if(gravityAttractor.Attract (transform)){
				closePlanets++;
			}
		}
		if (closePlanets == 0) 
		{
			usesSpaceGravity = true;
			rigidbody.drag = 0f;
		} 
		else 
		{
			rigidbody.drag = Constants.GRAVITY_FORCE_OF_ATHMOSPHERE;
		}
	}

	public bool getIsTouchingPlanet(){
		return isTouchingPlanet;
	}

	public bool getUsesSpaceGravity(){
		return usesSpaceGravity;
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public LayerMask objectsToCollide;

	GameObject[] planets;
	bool isTouchingPlanet;
	bool usesSpaceGravity;
	bool isOutsideAthmosphere;
	float minimumPlanetDistance = float.MaxValue;
	bool isOrbitingAroundPlanet = false;
	bool isGettingOutOfOrbit = false;

	List<GameObject> collidingObjects = new List<GameObject>(0);


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
			collidingObjects.Add (obj);
			if(usesSpaceGravity){
				//Just landed from spaceJump
				GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
			}
			objectsTouching++;
			GetComponent<Rigidbody>().drag = Constants.DRAG_ON_TOUCH_PLANETS;
			isTouchingPlanet = true;
			usesSpaceGravity = false;
		}
	}

	public void checkTouchExit(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			collidingObjects.Remove(obj);

			objectsTouching--;
			if(objectsTouching==0){
				GetComponent<Rigidbody>().drag = 0f;
				isTouchingPlanet = false;
			}
		}
	}

	public void checkForDestroyedObjects(){
		List<GameObject> newList = new List<GameObject> (0);
		foreach(GameObject go in collidingObjects){
			if(go!=null){
				newList.Add(go);
			}else{
				objectsTouching--;
			}
		}
		collidingObjects = newList;
		if(objectsTouching==0){
			GetComponent<Rigidbody>().drag = 0f;
			isTouchingPlanet = false;
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
			GetComponent<Rigidbody>().drag = 0f;
			isOutsideAthmosphere = true;
			isGettingOutOfOrbit = false;
			setIsOrbitingAroundPlanet(false);
		} 
		else 
		{
			isOutsideAthmosphere = false;
			if(gameObject.tag == "Player"){
				//Ponemos el drag solo para el player
				float dragProportion = minimumPlanetDistance / Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR;
				dragProportion = 1f - dragProportion;
				if(dragProportion>1f){dragProportion = 1f;}
				else if(dragProportion<Constants.PERCENTAGE_DRAG_ATHMOSPHERE){dragProportion = 0.0f;}
				if(usesSpaceGravity){
					dragProportion*=5f;
				}
				GetComponent<Rigidbody>().drag = dragProportion * Constants.GRAVITY_DRAG_OF_ATHMOSPHERE;
			}else{
				GetComponent<Rigidbody>().drag = 0f;
			}
		}

	}

	public bool getIsTouchingPlanet(){
		checkForDestroyedObjects ();
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

	public bool getIsOrbitingAroundPlanet(){
		return isOrbitingAroundPlanet;
	}

	public void setIsOrbitingAroundPlanet(bool orbiting){
		isOrbitingAroundPlanet = orbiting;
		GameManager.gameState.isCameraLockedToPlayer = !orbiting;
	}

	public bool getIsGettingOutOfOrbit(){
		return isGettingOutOfOrbit;
	}

	public void setIsGettingOutOfOrbit(bool orbit){
		isGettingOutOfOrbit = orbit;
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

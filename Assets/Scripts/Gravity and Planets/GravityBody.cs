using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public LayerMask objectsToCollide;
	public float drag;
	protected GameObject[] planets;
	protected bool isTouchingPlanet;
	protected float minimumPlanetDistance = float.MaxValue;

	protected List<GameObject> collidingObjects = new List<GameObject>(0);
	protected int objectsTouching = 0;


	void Start() {
		planets = GravityBodiesManager.getGravityBodies ();
		GetComponent<Rigidbody> ().drag = drag;
	}

	public virtual void checkTouchEnter(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			collidingObjects.Add (obj);
			objectsTouching++;
			isTouchingPlanet = true;
		}
	}

	public virtual void checkTouchExit(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			collidingObjects.Remove(obj);

			objectsTouching--;
			if(objectsTouching==0){
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
			isTouchingPlanet = false;
		}
	}

	void FixedUpdate() {
		attract ();
	}

	public virtual void attract(){

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
	}

	public virtual bool isSpaceGravityBody(){
		return false;
	}

	public bool getIsTouchingPlanet(){
		checkForDestroyedObjects ();
		return isTouchingPlanet;
	}

	public float getMinimumPlanetDistance(){
		return minimumPlanetDistance;
	}
}

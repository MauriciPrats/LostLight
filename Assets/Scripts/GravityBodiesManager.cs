using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GravityBodiesManager {

	private static List<GameObject> gravityBodies;

	public static void registerNewBody(GameObject gameObject){
		if (gravityBodies == null) {
			gravityBodies = new List<GameObject>();
		}
		gravityBodies.Add (gameObject);
	}

	public static GameObject[] getGravityBodies(){
		return gravityBodies.ToArray ();
	}
}

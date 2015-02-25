using UnityEngine;
using System.Collections;

public class HideFrontPlanetFaceOnEnter : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}

	void enableAll(bool enabled){
		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach(Renderer renderer in renderers){
			renderer.enabled = enabled;
		}
	}
	// Update is called once per frame
	void Update () {
		SphereCollider sphereCollider = (SphereCollider)collider;
		//Debug.Log (sphereCollider.radius);

		float sphereRadius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;

		if(Vector3.Distance(transform.position,GameManager.player.transform.position)<sphereRadius){
			renderer.enabled = false;
			enableAll(false);
		}else{
			renderer.enabled = true;
			enableAll(true);
		}
	}
}

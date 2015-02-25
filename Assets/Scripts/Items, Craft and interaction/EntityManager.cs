using UnityEngine;
using System.Collections;

public class EntityManager : MonoBehaviour {

	private static EntityManager singleton;

	public static EntityManager Instance {
		get{ return singleton ?? (singleton = new GameObject("EntityManager").AddComponent<EntityManager>());}
	}	

	/**************************************************
	 * Gets the interactuable entity closest to the GameObject 
	 * passed by parameter.
	 * 
	 * Returns an Interactuable object. 
	 **************************************************/
	public Interactuable GetClosestInteractuableEntity(GameObject player, float maxDistance)
	{
		Interactuable[] closeInteractEntities;
		closeInteractEntities = GameObject.FindObjectsOfType <Interactuable> ();
		if (closeInteractEntities.Length > 0) {
			Interactuable closest = null;
			Vector3 playerPosition = player.transform.position;
			float previousDistance = Mathf.Infinity;
			foreach (Interactuable interactuableEntity in closeInteractEntities) {
				Vector3 currentPosition = interactuableEntity.transform.position;
				float currentDistance = Vector3.Distance(playerPosition,currentPosition);
				if (currentDistance < previousDistance){
					closest = interactuableEntity;
					previousDistance = currentDistance;
				}
			}
			if(previousDistance < maxDistance){
				return closest.GetComponent<Interactuable>();
			} else { 
				return null;
			}
		} else {
			return null;
		}
	}

}

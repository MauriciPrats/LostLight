using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public bool isRotating = false;
	public float speedRotation = 30;
	public GameObject athmospherePrefab;
	private GameObject athmosphere;
	public GameObject athmosphereMinimapPrefab;
	public float gravityDistance = Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR;

	private float sphereRadius;
	void Awake(){
		GravityBodiesManager.registerNewBody (this.gameObject);

		athmosphere = GameObject.Instantiate(athmospherePrefab) as GameObject;
		athmosphere.transform.parent = transform;
		athmosphere.transform.position = transform.position;

		//We calculate the size of the athmosphere of the gravityAttractor
		float size = transform.GetComponent<SphereCollider> ().radius * Mathf.Max (transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
		size += gravityDistance;

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

		SphereCollider sphereCollider = (SphereCollider) transform.gameObject.GetComponent (typeof(SphereCollider));
		sphereRadius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
	}

	void FixedUpdate(){
		if (isRotating && GameManager.gameState.arePlanetsMoving) {
			//print (Vector3.Distance (transform.parent.position, transform.position));
			transform.position = OrbiteAroundPoint (transform.position, transform.parent.position, Quaternion.Euler (0, 0, speedRotation * Time.deltaTime));
		}
	}

	private bool attract(Transform objectToAttract,out float distance){
		//Only attract the body to the planet if it is close enough.
		distance = Vector3.Distance (transform.position, objectToAttract.position);
		GravityBody body = objectToAttract.GetComponent<GravityBody> ();
		
		distance = distance - sphereRadius;
		
		if (distance <= gravityDistance) {
			Vector3 targetDir = (objectToAttract.position - transform.position);
			targetDir = new Vector3(targetDir.x,targetDir.y,0f).normalized;
			
			Vector3 objectUp = objectToAttract.up;
			
			objectToAttract.rotation = Quaternion.FromToRotation (objectUp, targetDir) * objectToAttract.rotation;
			float forceToAdd = -Constants.GRAVITY_FORCE_OF_PLANETS;
			objectToAttract.GetComponent<Rigidbody>().AddForce (targetDir * forceToAdd ,ForceMode.Force);
			return true;
		}
		return false;
	}

	private bool spaceAttract(Transform objectToAttract,out float distance){
		//Only attract the body to the planet if it is close enough.
		distance = Vector3.Distance (transform.position, objectToAttract.position);
		SpaceGravityBody body = objectToAttract.GetComponent<SpaceGravityBody> ();

		distance = distance - sphereRadius;
		
		if (distance <= gravityDistance) {
			Vector3 targetDir = (objectToAttract.position - transform.position);
			targetDir = new Vector3(targetDir.x,targetDir.y,0f).normalized;
			
			Vector3 objectUp = objectToAttract.up;
			
			objectToAttract.rotation = Quaternion.FromToRotation (objectUp, targetDir) * objectToAttract.rotation;
			float forceToAdd = -Constants.GRAVITY_FORCE_OF_PLANETS;
			
			bool hasToAddForce = true;

			float ratioDistance = distance/gravityDistance;
			if(body.getUsesSpaceGravity()){
				
				bool isOrbiting = body.getIsOrbitingAroundPlanet();
				if(!isOrbiting){
					
					float angle = Vector3.Angle(body.GetComponent<Rigidbody>().velocity,targetDir);
					angle = Mathf.Abs(angle);
					if(angle>= Constants.ANGLE_CAN_ENTER_ORBIT_START && angle<= Constants.ANGLE_CAN_ENTER_ORBIT_END){
						if(ratioDistance>= Constants.PERCENTAGE_ATHMOSPHERE_CAN_ENTER_ORBIT_START && ratioDistance<= Constants.PERCENTAGE_ATHMOSPHERE_CAN_ENTER_ORBIT_END){
							isOrbiting = true;
							body.setIsOrbitingAroundPlanet(true);
						}
					}
				}

				if(ratioDistance<=Constants.PERCENTAGE_ATHMOSPHERE_CAN_ENTER_ORBIT_START){
					isOrbiting = false;
					body.setIsOrbitingAroundPlanet(false);
				}
				
				if(isOrbiting && !body.getIsGettingOutOfOrbit()){
					hasToAddForce = false;
					//Meeec, no funciona be, sempre va a la dreta
					//bool goesRight = Vector3.Angle(targetDir,-body.rigidbody.velocity) < Vector3.Angle(targetDir,body.rigidbody.velocity);
					bool goesRight = GameManager.player.GetComponent<PlayerController>().getIsLookingRight();
					float angle2 = Util.getAngleFromVectorAToB(body.GetComponent<Rigidbody>().velocity,transform.position - body.transform.position);
					goesRight = angle2<0f;
					float forceMagnitude = body.GetComponent<Rigidbody>().velocity.magnitude;
					
					if(goesRight){
						forceMagnitude *= -1;
					}else{
						targetDir *= -1f;
					}
					float forceRatio = Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(body.GetComponent<Rigidbody>().velocity,targetDir));
					
					Vector3 forwardDirection = body.GetComponent<Rigidbody>().transform.forward.normalized;
					if(!GameManager.player.GetComponent<PlayerController>().getIsLookingRight()){
						forwardDirection *= -1f;
					}
					body.GetComponent<Rigidbody>().velocity = ((forwardDirection) + (((targetDir.normalized) * Mathf.Abs(forceRatio)))).normalized * forceMagnitude;
					objectToAttract.parent = transform;
				}
				
				if(body.getIsGettingOutOfOrbit()){
					hasToAddForce = false;
				}
				
				float ratio = 1f-(distance / gravityDistance);
				forceToAdd *=Constants.GRAVITY_MULTIPLYIER_ON_SPACE_JUMPS * ratio;
			}
			if(hasToAddForce){
				objectToAttract.GetComponent<Rigidbody>().AddForce (targetDir * forceToAdd ,ForceMode.Force);
			}
			//We only put the body in the hierarchy if it has touched a planet after doing "Space travel".
			if(!body.getUsesSpaceGravity()){
				objectToAttract.parent = transform;
			}
			
			return true;
		}
		return false;
	}

	public bool Attract (Transform objectToAttract,out float distance){

		if(objectToAttract.GetComponent<GravityBody>().isSpaceGravityBody()){
			return spaceAttract(objectToAttract,out distance);
		}else{
			return attract(objectToAttract,out distance);
		}

	}

	private Vector3 OrbiteAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * ( point - pivot) + pivot;
	}
}

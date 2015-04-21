using UnityEngine;
using System.Collections;

public class SpaceGravityBody : GravityBody {

	protected bool usesSpaceGravity;
	protected bool isOutsideAthmosphere;
	protected bool isOrbitingAroundPlanet = false;
	protected bool isGettingOutOfOrbit = false;
	public float dragMultiplyierOnCloseOrbit = 5f;

	public override void checkTouchEnter(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			collidingObjects.Add (obj);
			if(usesSpaceGravity){
				//Just landed from spaceJump
				GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
			}
			objectsTouching++;
			isTouchingPlanet = true;
			usesSpaceGravity = false;
		}
	}

	public override void checkTouchExit(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			collidingObjects.Remove(obj);
			
			objectsTouching--;
			if(objectsTouching==0){
				isTouchingPlanet = false;
			}
		}
	}

	public override void attract(){
		
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
			isOutsideAthmosphere = true;
			GetComponent<Rigidbody>().drag = 0f;
			isGettingOutOfOrbit = false;
			setIsOrbitingAroundPlanet(false);
		} 
		else 
		{
			isOutsideAthmosphere = false;
			if(usesSpaceGravity){
				float dragProportion = minimumPlanetDistance / Constants.GRAVITY_DISTANCE_FROM_PLANET_FLOOR;
				float invertDragProportion = 1f - dragProportion;
				if(invertDragProportion>1f){invertDragProportion = 1f;}
				else if(invertDragProportion<Constants.PERCENTAGE_DRAG_ATHMOSPHERE){invertDragProportion = 0.0f;}
				if(usesSpaceGravity){
					invertDragProportion*=dragMultiplyierOnCloseOrbit;
				}else{
					invertDragProportion = 0f;
				}
				GetComponent<Rigidbody>().drag = invertDragProportion * Constants.GRAVITY_DRAG_OF_ATHMOSPHERE;
			}else{
				GetComponent<Rigidbody>().drag = drag;
			}
		}
		
	}

	public override bool isSpaceGravityBody(){
		return true;
	}

	public bool getUsesSpaceGravity(){
		return usesSpaceGravity;
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
}

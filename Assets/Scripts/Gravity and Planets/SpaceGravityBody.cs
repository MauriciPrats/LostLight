using UnityEngine;
using System.Collections;

public class SpaceGravityBody : GravityBody {

	protected bool usesSpaceGravity;
	protected bool isOutsideAthmosphere;
	protected bool isOrbitingAroundPlanet = false;
	protected bool isGettingOutOfOrbit = false;
	protected bool isFallingIntoPlanet = false;
	public float dragMultiplyierOnCloseOrbit = 5f;
	public float massOnJump;
	public bool isPlayer = false;
	private float originalMass;
	private bool isStatic = false;
	private GameObject closestPlanet;
	private bool canEnterSpaceSlide = true;

	protected override void onStart(){
		originalMass = GetComponent<Rigidbody>().mass;
	}

	public override void checkTouchEnter(GameObject obj){
		if (obj.tag == "Planet" || obj.tag == "Enemy") {
			collidingObjects.Add (obj);
			if(isPlayer){
				GUIManager.deactivateSpaceJumpGUI();
			}
			if(usesSpaceGravity){
				//Just landed from spaceJump
				GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
			}
			objectsTouching++;
			isTouchingPlanet = true;
			usesSpaceGravity = false;
			isFallingIntoPlanet = false;
			if(obj.tag == "Planet"){
				GetComponent<Rigidbody>().mass = originalMass;
			}
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

	public void applySpaceBodyChangesOnJump(){
		GetComponent<Rigidbody>().mass = massOnJump;
	}

	public override void attract(bool applyForce){
		if(!isStatic){
			transform.parent = null;
			int closePlanets = 0;
			minimumPlanetDistance = float.MaxValue;
			GameObject closestTMP = null;
			foreach (GameObject planet in planets) {
				GravityAttractor gravityAttractor = planet.GetComponent<GravityAttractor> ();
				float distance = 0f;
				if(gravityAttractor.Attract (transform,out distance,applyForce)){
					closePlanets++;
				}
				if(distance<minimumPlanetDistance){
					minimumPlanetDistance = distance;
					closestTMP = planet;
				}
			}
			
			if (closePlanets == 0) 
			{
				if(isPlayer){
					setClosestPlanet(null);
				}
				usesSpaceGravity = true;
				isOutsideAthmosphere = true;
				GetComponent<Rigidbody>().drag = 0f;
				isGettingOutOfOrbit = false;
				setIsOrbitingAroundPlanet(false);
			}else{
				canEnterSpaceSlide = true;
				isOutsideAthmosphere = false;
				if(isPlayer){
					setClosestPlanet(closestTMP);
				}
				if(isGettingOutOfOrbit){
					GetComponent<Rigidbody>().drag = 0f;
				}else if(usesSpaceGravity){
					float dragProportion = minimumPlanetDistance / closestPlanet.GetComponent<GravityAttractor>().gravityDistance;
					float invertDragProportion = 1f - dragProportion;
					if(invertDragProportion>1f){invertDragProportion = 1f;}
					else if(invertDragProportion<Constants.PERCENTAGE_DRAG_ATHMOSPHERE){invertDragProportion = 0.0f;}
					invertDragProportion*=dragMultiplyierOnCloseOrbit;
					GetComponent<Rigidbody>().drag = invertDragProportion * Constants.GRAVITY_DRAG_OF_ATHMOSPHERE;
				}else{
					GetComponent<Rigidbody>().drag = drag;
				}
			}
		}
	}

	//Sets the character to the closest planet (Without need to touch it)
	public void bindToClosestPlanet(){
		attract (false);
		usesSpaceGravity = false;
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
		if (!orbiting) {
			if(isPlayer){
				GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().unfollowObjective();
			}
		}
	}
	
	public bool getIsGettingOutOfOrbit(){
		return isGettingOutOfOrbit;
	}
	
	public void setIsGettingOutOfOrbit(bool orbit){
		if(!isFallingIntoPlanet){
			isGettingOutOfOrbit = orbit;
		}
	}

	public void setIsFallingIntoPlanet(bool falling){
		if(!isGettingOutOfOrbit){
			isFallingIntoPlanet = falling;
		}
	}

	public bool getIsFallingIntoPlanet(){
		return isFallingIntoPlanet;
	}

	public Planet getClosestPlanet(){
		if (closestPlanet != null) {
			return closestPlanet.GetComponent<Planet>();
		}else{
			return null;
		}
	}

	//Sets the gameObject as the closest planet, and activates or deactivates the planets accordingly
	private void setClosestPlanet(GameObject closestPlanet){
		if(closestPlanet!=null && closestPlanet!=this.closestPlanet){
			if(this.closestPlanet!=null){
				this.closestPlanet.GetComponent<Planet> ().deactivate();
			}
			this.closestPlanet = closestPlanet;
			this.closestPlanet.GetComponent<Planet> ().activate ();
		}else if(closestPlanet!=this.closestPlanet){
			this.closestPlanet.GetComponent<Planet> ().deactivate();
			this.closestPlanet = closestPlanet;
		}
	}

	//If it is static, it won't move, nor be attracted by the planet
	public void setStatic(bool st){
		isStatic = st;
	}

	
	public void setCanEnterDragonSlide(bool canEnter){
		canEnterSpaceSlide = canEnter;
	}
	
	public bool getCanEnterDragonSlide(){
		return canEnterSpaceSlide;
	}
}

using UnityEngine;
using System.Collections;

abstract public class PlanetEventsManager : MonoBehaviour {
	
	public bool isEnabled;
	private bool active;
	private bool initialized = false;
	public abstract void informEventActivated (CutsceneIdentifyier identifyier);
	public abstract void initialize ();
	public abstract void isActivated ();
	public abstract void startButtonPressed ();
	void Start(){
		if(!initialized){
			initialized = true;
			initialize ();
		}
	}

	public void activate(){
		active = true;
		isActivated ();
	}

	public void deactivate(){
		active = false;
	}
}

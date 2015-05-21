using UnityEngine;
using System.Collections;

abstract public class PlanetEventsManager : MonoBehaviour {

	protected bool isEnabled;
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
		isEnabled = true;
		isActivated ();
	}

	public void deactivate(){
		isEnabled = false;
	}
}

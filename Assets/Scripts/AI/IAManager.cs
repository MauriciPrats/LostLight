using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAManager : MonoBehaviour {

	List<IAController> actualIAs;

	void Awake(){
		GameManager.registerIAManager (this);
		actualIAs = new List<IAController> (0);
	}

	void Update(){
		if(Input.GetKeyUp(KeyCode.V)){
			disableIAs();
		}
		if(Input.GetKeyUp(KeyCode.B)){
			enableIAs();
		}
	}

	public void registerIA(IAController iaC){
		actualIAs.Add (iaC);
	}

	public void removeIA(IAController iaC){
		actualIAs.Remove (iaC);
	}

	public void disableIAs(){
		foreach(IAController iaController in actualIAs){
			iaController.deactivate();
		}
	}

	public void enableIAs(){
		foreach(IAController iaController in actualIAs){
			iaController.activate();
		}
	}
}

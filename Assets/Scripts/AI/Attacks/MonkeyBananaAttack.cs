using UnityEngine;
using System.Collections;

public class MonkeyBananaAttack : Attack {

	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;

	public override void initialize(){
		attackType = AttackType.MonkeyBananaAttack;
	}
	
	void OnTriggerEnter(Collider col) {

	}
	
	void OnTriggerExit(Collider col){

	}
	
	public override void startAttack(){

	}

	
	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		transform.position = parentObject.GetComponent<Rigidbody>().worldCenterOfMass + (parentObject.transform.forward*parentObject.GetComponent<WalkOnMultiplePaths>().centerToExtremesDistance*1.3f);
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
		outlineChanger = parent.GetComponent<OutlineChanging> ();
	}
}

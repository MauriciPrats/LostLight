using UnityEngine;
using System.Collections;

public class CraneMeleeAttack : Attack {

	//NOT IMPLEMENTED

	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	
	private Vector3 playerOriginalPosition;
	
	public override void initialize(){
		attackType = AttackType.CraneMeleeAttack;
	}
	
	public override void enemyCollisionEnter(GameObject enemy){
		GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
	}
	
	public override void startAttack(){
		isFinished = false;
		StartCoroutine("doAttack");
	}
	
	private IEnumerator doAttack(){
		yield return null;
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

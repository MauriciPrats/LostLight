using UnityEngine;
using System.Collections;

public class CraneMeleeAttack : Attack {

	//NOT IMPLEMENTED YET

	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	public float timeToChargeAttack = 1f;
	private bool isPlayerInRange = false;
	
	private Vector3 playerOriginalPosition;
	
	public override void initialize(){
		attackType = AttackType.CraneMeleeAttack;
	}


	private void doDamage(){
		GameManager.playerController.getHurt (damage);
	}

	public override void otherCollisionEnter(GameObject element){
		if( element.tag.Equals("Player")){
			isPlayerInRange = true;
		}
	}

	public override void otherCollisionExit(GameObject element){
		if( element.tag.Equals("Player")){
			isPlayerInRange = false;
		}
	}
	
	public override void startAttack(){
		isFinished = false;
		StartCoroutine("doAttack");
	}
	
	private IEnumerator doAttack(){
		iaAnimator.SetTrigger ("isDoingMeleeAttack");
		float timer = 0f;
		while(timer<timeToChargeAttack){
			timer+=Time.deltaTime;
			float ratio = timer/timeToChargeAttack;
			outlineChanger.setOutlineColor(Color.Lerp(Color.black,Color.red,ratio));
			yield return null;
		}
		if(isPlayerInRange){
			doDamage();
		}
		outlineChanger.setOutlineColor (Color.black);
		isFinished = true;
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

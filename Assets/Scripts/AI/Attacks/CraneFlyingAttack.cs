using UnityEngine;
using System.Collections;

public class CraneFlyingAttack : Attack {

	//NOT IMPLEMENTED YET!
	
	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	private bool isActive = false;
	public float timeToChargeAttack = 1f;

	private Vector3 playerOriginalPosition;
	private bool interrupted = false;
	
	public override void initialize(){
		attackType = AttackType.CraneFlyingAttack;
	}
	
	public override void otherCollisionEnter(GameObject enemy){
		if(isActive && enemy.tag.Equals("Player") && !isFinished && !interrupted){
			isActive = false;
			GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
		}
	}

	public void onHitGround(){
		if(parent!=null){
			parent.GetComponent<IAControllerCrane> ().resetActionToCallOnTouchGround ();
			outlineChanger.setOutlineColor (Color.black);
			iaAnimator.SetBool ("isFlyingAttacking", false);
			isFinished = true;
			parent.GetComponent<IAControllerCrane> ().resetOriginalPreferredHeight ();
		}
	}
	
	public override void startAttack(){
		isFinished = false;
		StartCoroutine("doAttack");
	}
	
	private IEnumerator doAttack(){
		interrupted = false;
		iaAnimator.SetBool ("isFlyingAttacking", true);
		parent.GetComponent<IAControllerCrane> ().preferredHeight = 0f;
		parent.GetComponent<IAControllerCrane> ().upSpeed = 2f;

		float timer = 0f;
		while(timer<timeToChargeAttack && !interrupted){
			timer+=Time.deltaTime;
			float ratio = timer/timeToChargeAttack;
			outlineChanger.setOutlineColor(Color.Lerp(Color.black,Color.red,ratio));
			yield return null;
		}
		if(!interrupted){
			parent.GetComponent<IAControllerCrane> ().setActionToCallOnTouchGround (onHitGround);
			isActive = true;
		}
	}

	public override void interruptAttack(){
		onHitGround ();
		interrupted = true;
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

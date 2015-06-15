using UnityEngine;
using System.Collections;

public class PenguinSlideMove : Attack {

	public float timeItLasts = 2f;
	public float speedMultiplyier = 2f;
	public float timeToCharge = 1f;
	public float timeToStand = 0.5f;
	public float distanceStandUpBehindEnemy = 0.3f;

	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	private float direction;
	private float originalPlayerDirection;

	public override void initialize(){
		attackType = AttackType.PenguinSlideMove;
	}
	
	public override void enemyCollisionEnter(GameObject enemy){
		//GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
	}
	
	public override void startAttack(){
		if(isFinished){
			StartCoroutine("doAttack");
			isFinished = false;
			originalPlayerDirection = parent.GetComponent<IAController> ().getPlayerDirection ();
		}
	}

	private bool isInOtherSideOfEnemy(){
		if(parent.GetComponent<IAController> ().getPlayerDirection () != originalPlayerDirection && parent.GetComponent<IAController> ().getPlayerDistance()>=distanceStandUpBehindEnemy){
			return true;
		}
		return false;
	}
	IEnumerator doAttack(){
		iaAnimator.SetTrigger("isChargingSlide");
		float timer = 0f;
		while(timer<timeToCharge){
			timer+=Time.deltaTime;
			float ratio = timer/timeToCharge;
			yield return null;
		}

		iaAnimator.SetBool("isSliding",true);
		timer = 0f;
		direction = parent.GetComponent<IAController> ().getPlayerDirection ();
		parent.layer = LayerMask.NameToLayer("OnlyFloor");

		while(timer<timeItLasts && !isInOtherSideOfEnemy()){
			timer+=Time.deltaTime;
			float ratio = timer/timeItLasts;
			parent.GetComponent<CharacterController>().Move(direction * speedMultiplyier);
			yield return null;
		}
		parent.GetComponent<CharacterController> ().StopMoving ();
		iaAnimator.SetBool("isSliding",false);

		timer = 0f;
		while(timer<timeToStand){
			timer+=Time.deltaTime;
			yield return null;
		}

		isFinished = true;
		parent.layer = LayerMask.NameToLayer("Enemy");
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

using UnityEngine;
using System.Collections;

public class PenguinSlideMove : Attack {

	public float timeItLasts = 1f;
	public float speedMultiplyier = 2f;
	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	private float direction;

	public override void initialize(){
		attackType = AttackType.PenguinSlideMove;
	}
	
	public override void enemyCollisionEnter(GameObject enemy){
		//GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
	}
	
	public override void startAttack(){
		Debug.Log("Penguin Slide Move");
		StartCoroutine("doAttack");
		isFinished = false;
	}

	IEnumerator doAttack(){
		
		float timer = 0f;
		direction = parent.GetComponent<IAController> ().getPlayerDirection ();
		parent.layer = LayerMask.NameToLayer("OnlyFloor");
		while(timer<timeItLasts){
			timer+=Time.deltaTime;
			float ratio = timer/timeItLasts;
			parent.GetComponent<IAController>().Move(direction * speedMultiplyier);
			yield return null;
		}
		outlineChanger.setOutlineColor(Color.black);
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

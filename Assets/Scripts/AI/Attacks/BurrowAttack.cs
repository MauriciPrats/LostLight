using UnityEngine;
using System.Collections;

public class BurrowAttack : Attack {

	public GameObject burrowParticles;
	public float timeUnderground = 1.5f;

	private GameObject parent;
	private float direction = 0f;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	private float attackTimer = 0f;

	public override void initialize(){
		attackType = AttackType.BurrowAttack;
	}

	public override void startAttack(){
		bool isRight = parent.GetComponent<IAController>().getIsLookingRight();
		if(isRight){
			direction = 1f;
		}else{
			direction = -1f;
		}
		isFinished = false;

		StartCoroutine ("burrow");
	}

	private IEnumerator burrow() {
		isFinished = false;
		burrowParticles.SetActive (true);
		parent.GetComponent<IAController> ().Move(direction);
		while(attackTimer<=timeUnderground){
			attackTimer+=Time.deltaTime;
			if(!parent.GetComponent<IAController>().isDead){
				parent.GetComponent<IAController> ().Move(direction);
			}
			yield return null;
		}
		//burrowParticles.SetActive (false);
		isFinished = true;
		yield return null;

	}

	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		transform.position = parentObject.GetComponent<Rigidbody>().worldCenterOfMass + (parentObject.transform.forward*parentObject.GetComponent<WalkOnMultiplePaths>().centerToExtremesDistance*1.5f);
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
		outlineChanger = parent.GetComponent<OutlineChanging> ();
	}


}

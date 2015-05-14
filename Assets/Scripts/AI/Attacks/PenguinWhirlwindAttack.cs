using UnityEngine;
using System.Collections;

public class PenguinWhirlwindAttack : Attack {

	public float timeToCharge = 0.3f;

	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	
	public override void initialize(){
		attackType = AttackType.PenguinWhirlwindAttack;
	}
	
	public override void enemyCollisionEnter(GameObject enemy){
		//GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
	}
	
	public override void startAttack(){
		Debug.Log("Penguin Whirlwind Attack");
		StartCoroutine("doAttack");
		isFinished = false;
	}

	IEnumerator doAttack(){

		float timer = 0f;

		while(timer<timeToCharge){
			timer+=Time.deltaTime;
			float ratio = timer/timeToCharge;
			outlineChanger.setOutlineColor(Color.Lerp(Color.black,Color.red,ratio));
			yield return null;
		}
		outlineChanger.setOutlineColor(Color.black);
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

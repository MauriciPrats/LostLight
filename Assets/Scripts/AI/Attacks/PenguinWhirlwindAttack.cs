using UnityEngine;
using System.Collections;

public class PenguinWhirlwindAttack : Attack {

	public float timeToCharge = 0.8f;

	public float timeItLasts = 1f;
	public float rotationSpeed = 360f;
	public float speedToAddV = 4f;

	public GameObject colliderO;

	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	
	public override void initialize(){
		attackType = AttackType.PenguinWhirlwindAttack;
		colliderO.SetActive (false);
	}

	public override void otherCollisionEnter(GameObject enemy){
		Debug.Log ("Attaack");
		if(enemy.tag.Equals("Player")){
			GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
			Vector3 speedToAdd = GameManager.player.GetComponent<Rigidbody>().worldCenterOfMass - parent.GetComponent<Rigidbody>().worldCenterOfMass;
			GameManager.player.GetComponent<Rigidbody>().velocity +=(speedToAdd.normalized * speedToAddV);
		}
	}
	
	public override void startAttack(){
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
		timer = 0f;
		colliderO.SetActive (true);
		while (timer<timeItLasts) {
			timer+=Time.deltaTime;
			transform.RotateAround(parent.GetComponent<Rigidbody>().worldCenterOfMass,parent.transform.up,rotationSpeed*Time.deltaTime);
			yield return null;
		}
		colliderO.SetActive (false);
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

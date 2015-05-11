using UnityEngine;
using System.Collections;

public class BurrowAttack : Attack {

	public GameObject burrowParticles;

	private float direction = 0f;

	public override void startAttack(){
		bool isRight = gameObject.GetComponent<IAController>().getIsLookingRight();
		if(isRight){
			direction = 1f;
		}else{
			direction = -1f;
		}
		isFinished = false;
		StartCoroutine ("burrow");
	}

	private IEnumerator burrow() {

		burrowParticles.SetActive (true);
		yield return null;

	}

}

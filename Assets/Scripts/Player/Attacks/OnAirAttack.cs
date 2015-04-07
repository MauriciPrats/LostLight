using UnityEngine;
using System.Collections;

public class OnAirAttack : Attack {
	
	public GameObject triggerBox;
	public int ammountOfDamage = 1;
	public float positionInFrontPlayer = 0.5f;
	// Use this for initialization
	void Start () {
	
	}

	public override void enemyCollisionEnter(GameObject enemy){
		//If it's an enemy we damage him
		enemy.GetComponent<IAController>().getHurt(ammountOfDamage,enemy.transform.position);
		GameManager.comboManager.addCombo ();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.player.GetComponent<GravityBody>().getIsTouchingPlanet()){
			isFinished = true;
			triggerBox.SetActive(false);
		}
	}

	private IEnumerator airAttack(){
		triggerBox.transform.position = GameManager.player.GetComponent<Rigidbody> ().worldCenterOfMass + GameManager.player.transform.forward.normalized * positionInFrontPlayer;
		triggerBox.SetActive (true);
		triggerBox.transform.parent = GameManager.player.transform;
		triggerBox.transform.rotation = GameManager.player.transform.rotation;
		yield return true;
	}

	public override void startAttack(){
		GameManager.playerAnimator.SetTrigger("isDoingAirAttack");
		StartCoroutine ("airAttack");
	}
}

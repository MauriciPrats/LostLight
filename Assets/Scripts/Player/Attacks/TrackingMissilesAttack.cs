using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackingMissilesAttack : Attack,AnimationSubscriber {

	public GameObject attackCharge;
	public GameObject missilePrefab;
	public float timeToChargeAttack = 0.7f;
	public int maxAmmountOfMissilesSpawned = 4;
	private AnimationEventBroadcast eventHandler;

	private List<GameObject> enemiesHit;

	public float range = 8f;

	float timer = 0f;

	public override void initialize(){
		attackType = AttackType.Missiles;
		eventHandler = GameManager.playerAnimator.gameObject.GetComponent<AnimationEventBroadcast>();
		eventHandler.subscribe(this);
	}

	public void enemyHit(GameObject enemy){
		if(!enemiesHit.Contains(enemy)){
			enemiesHit.Add(enemy);
			enemy.GetComponent<IAController>().getHurt(damage,(enemy.transform.position));
			GameManager.comboManager.addCombo ();
			if(!elementAttack.Equals(ElementType.None)){
				AttackElementsManager.getElement(elementAttack).doEffect(enemy);
			}
		}
	}

	private void spawnMissiles(){

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		List<GameObject> enemiesInRange = new List<GameObject> (0);
		foreach(GameObject enemy in enemies){
			if(Vector3.Distance(enemy.transform.position,attackCharge.transform.position)<=range){
				enemiesInRange.Add(enemy);
			}
		}

		int ammountOfMissiles = maxAmmountOfMissilesSpawned;
		if(enemiesInRange.Count<ammountOfMissiles){
			ammountOfMissiles = enemiesInRange.Count;
		}

		for(int i = 0;i<ammountOfMissiles;++i){
			GameObject newMissile = Instantiate(missilePrefab);
			newMissile.transform.position = attackCharge.transform.position;
			newMissile.GetComponent<TrackingMissile>().initialize(GameManager.player.transform.up,enemiesInRange[i],this,elementAttack);
		}
	}
	
	protected override void update(){
		/*if(!isFinished){
			timer+=Time.deltaTime;
			attackCharge.transform.position = GameManager.lightGemObject.transform.position ;
			if(timer>=timeToChargeAttack){
				spawnMissiles();
				attackCharge.SetActive (false);
				isFinished = true;
				GameManager.playerAnimator.SetBool("isDoingMissiles",false);
			}
		}*/
	}

	IEnumerator putTrackingMissilesInPosition(){
		while(timer<=timeToChargeAttack){
			timer+=Time.deltaTime;
			attackCharge.transform.position = GameManager.lightGemObject.transform.position ;
			yield return null;
		}
	}

	IEnumerator doTrackingMissilesAttack(){
		enemiesHit = new List<GameObject> (0);
		attackCharge.SetActive (true);
		attackCharge.transform.position = GameManager.lightGemObject.transform.position ;
		timer = 0f;
		StartCoroutine ("putTrackingMissilesInPosition");
		yield return new WaitForSeconds (timeToChargeAttack);
		spawnMissiles();
		attackCharge.SetActive (false);
		isFinished = true;
		GameManager.playerAnimator.SetBool("isDoingMissiles",false);
	}

	public override void startAttack(){
		GameManager.playerAnimator.SetBool("isDoingMissiles",true);
		isFinished = false;
	}

	void AnimationSubscriber.handleEvent(string idEvent) {
		Debug.Log (idEvent);
		switch (idEvent) {
		case "start": 
			StartCoroutine ("doTrackingMissilesAttack");
			break;
		case "end":
			break;
		default: 
			break;
		}
		
	}
	
	
	string AnimationSubscriber.subscriberName() {
		return  "TrackingMissiles";	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackingMissilesSpecialAttack : SpecialAttack {

	public GameObject attackCharge;
	public GameObject missilePrefab;
	public float timeToChargeAttack = 0.7f;
	public int maxAmmountOfMissilesSpawned = 4;
	public int damagePerMissile = 2;

	public float range = 8f;

	float timer = 0f;

	public void enemyHit(GameObject enemy){
		enemy.GetComponent<IAController>().getHurt(damagePerMissile,(enemy.transform.position));
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
			newMissile.GetComponent<TrackingMissile>().initialize(GameManager.player.transform.up,enemiesInRange[i],this);
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

	IEnumerator putTrackingMisssilesInPosition(){
		while(timer<=timeToChargeAttack){
			timer+=Time.deltaTime;
			attackCharge.transform.position = GameManager.lightGemObject.transform.position ;
			yield return null;
		}
	}

	IEnumerator doTrackingMissilesAttack(){
		attackCharge.SetActive (true);
		attackCharge.transform.position = GameManager.lightGemObject.transform.position ;
		isFinished = false;
		GameManager.playerAnimator.SetBool("isDoingMissiles",true);
		timer = 0f;
		StartCoroutine ("putTrackingMisssilesInPosition");
		yield return new WaitForSeconds (timeToChargeAttack);

		spawnMissiles();
		attackCharge.SetActive (false);
		isFinished = true;
		GameManager.playerAnimator.SetBool("isDoingMissiles",false);
	}

	public override void startAttack(){
		StartCoroutine ("doTrackingMissilesAttack");
	}
}

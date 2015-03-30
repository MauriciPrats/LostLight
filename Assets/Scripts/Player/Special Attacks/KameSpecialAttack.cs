using UnityEngine;
using System.Collections;

public class KameSpecialAttack : SpecialAttack {

	public GameObject kameEffect;
	public GameObject kameCore;
	public GameObject enemyHitEffectPrefab;
	public float totalTimeLasts = 0.9f;
	public float timeToDisappear = 0.2f;
	public float chargeTime = 0.6f;
	public float distance = 5f;
	public float forceExplosion = 200f;
	public int damageAmmount = 1;

	public float maxWidthKame = 0.2f;


	private bool isCleaningEffect;



	float timer = 0f;
	private Vector3 startPosition;
	private Vector3 closestPlanetCenter;

	private int objectsCollided = 0;
	public override void initialize(){
		
	}

	public override void enemyCollisionEnter(GameObject enemy){
		//If it's an enemy we damage him
		enemy.GetComponent<IAController>().getHurt(damageAmmount,(kameEffect.transform.position+enemy.transform.position)/2f);
		//We find the radius of areaEffect
		enemy.GetComponent<Rigidbody>().AddExplosionForce(forceExplosion,transform.position,1f);
		GameObject newEffect = GameObject.Instantiate (enemyHitEffectPrefab) as GameObject;
		newEffect.transform.position = enemy.GetComponent<Rigidbody> ().worldCenterOfMass - (kameEffect.transform.forward * 0.15f);
		Vector3 direction = (enemy.transform.position - kameCore.transform.position).normalized + (enemy.transform.up * 2f);
		enemy.GetComponent<Rigidbody> ().AddForce (direction.normalized * 15f,ForceMode.Impulse);
		enemy.GetComponent<IAController> ().stun (1f);
	}

	protected override void update(){
		timer += Time.deltaTime;
		if(!isFinished || isCleaningEffect){
			if(timer>=chargeTime){
				GameManager.playerAnimator.SetBool("isDoingKame",true);
				float extraTime1 = timer - chargeTime;
				if(extraTime1>= totalTimeLasts){
					GameManager.playerAnimator.SetBool("isDoingKame",false);
					float extraTime2 = (extraTime1 - totalTimeLasts);
					if(extraTime2<timeToDisappear){
						isCleaningEffect = true;
						isFinished = true;
						kameCore.GetComponent<ParticleSystem>().Stop();
						float ratio = extraTime2/timeToDisappear;
						float invertRatio = 1f - ratio;
						kameEffect.GetComponent<TrailRenderer>().startWidth = maxWidthKame * invertRatio;
						kameEffect.GetComponent<TrailRenderer>().endWidth = maxWidthKame * invertRatio;
					}else{
						kameEffect.SetActive (false);
						kameCore.SetActive(false);
					}
				}else{
					float ratio = (timer - chargeTime)/totalTimeLasts;
					float magnitude = ratio * distance;

					Vector3 newPostion =kameEffect.transform.position + (kameEffect.transform.forward * distance * Time.deltaTime) ;
					kameEffect.transform.position = newPostion;
					
					//We rotate the object
					Vector3 objectiveUp = (kameEffect.transform.position - closestPlanetCenter);
					objectiveUp = new Vector3(objectiveUp.x,objectiveUp.y,0f).normalized;
					Vector3 objectUp = new Vector3(kameEffect.transform.up.x,kameEffect.transform.up.y,0f).normalized;

					kameEffect.transform.rotation = Quaternion.FromToRotation (objectUp, objectiveUp) *kameEffect.transform.rotation;
				}
			}
		}
	}

	private IEnumerator resetTrail(){

		TrailRenderer tr = kameEffect.GetComponent<TrailRenderer>();
		TrailRenderer[] renderers = GetComponentsInChildren<TrailRenderer> ();
		float[] tempoTimes = new float[renderers.Length];
		float tmp = tr.time;
		tr.time = -1;
		for(int i = 0;i<renderers.Length;++i){
			tempoTimes[i] = renderers[i].time;
			renderers[i].time = -1;
		}
		yield return true;

		for(int i = 0;i<renderers.Length;++i){
			renderers[i].time = tempoTimes[i];
		}
		tr.time = tmp;
		tr.startWidth = maxWidthKame;
		tr.endWidth = maxWidthKame;

	}



	public override void startAttack(){


		kameEffect.transform.up = GameManager.player.transform.up;

		kameEffect.transform.rotation = Quaternion.LookRotation (GameManager.player.transform.forward, GameManager.player.transform.up);
		//Debug.Log (GameManager.player.transform.eulerAngles);
		/*if(GameManager.player.transform.eulerAngles.z<=90){
			kameEffect.transform.Rotate (0f, GameManager.player.transform.eulerAngles.y, 0f);
		}else{
			kameEffect.transform.Rotate (0f,-1f * GameManager.player.transform.eulerAngles.y, 0f);
		}*/
		//kameEffect.transform.eulerAngles = new Vector3 (GameManager.player.transform.eulerAngles.x, GameManager.player.transform.eulerAngles.y, 0f);
		GameObject[] planets = GravityBodiesManager.getGravityBodies ();
		float smallestDistance = float.PositiveInfinity;
		foreach(GameObject planet in planets){
			if(Vector3.Distance(planet.transform.position,GameManager.player.transform.position)<smallestDistance){
				closestPlanetCenter = planet.transform.position;
				smallestDistance = Vector3.Distance(planet.transform.position,GameManager.player.transform.position);
			}
		}
		StartCoroutine ("resetTrail");
		//resetTrail ();
		isFinished = false; 
		kameEffect.SetActive (true);
		kameCore.SetActive (true);
		kameCore.GetComponent<ParticleSystem>().Play();
		startPosition = ((GameManager.player.GetComponent<Rigidbody> ().worldCenterOfMass + GameManager.player.transform.position)/2) + (kameEffect.transform.forward * 0.4f);
		kameEffect.transform.position = startPosition;
		kameCore.transform.position = startPosition;
		kameCore.transform.forward = kameEffect.transform.forward;
		timer = 0f;
		isCleaningEffect = false;
		objectsCollided = 0;

		GameManager.playerAnimator.SetTrigger("isChargingKame");
	}
}

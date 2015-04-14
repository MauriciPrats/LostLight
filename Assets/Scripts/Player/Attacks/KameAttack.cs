using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class KameAttack : Attack {

	public GameObject kameEffect;
	public GameObject kameCore;
	public GameObject elementalParticleSystem;
	public GameObject enemyHitEffectPrefab;
	public float totalTimeLasts = 0.9f;
	public float timeToDisappear = 0.2f;
	public float chargeTime = 0.6f;
	public float distance = 5f;
	public float forceExplosion = 10f;

	public float maxWidthKame = 0.2f;

	float timer = 0f;
	float timerCleanTrail = 0f;
	private Vector3 startPosition;
	private Vector3 closestPlanetCenter;
	private bool isCharged = false;

	private List<GameObject> enemiesHit;

	private int objectsCollided = 0;
	public override void initialize(){
		attackType = AttackType.Kame;
	}

	public override void enemyCollisionEnter(GameObject enemy){
		if(!enemiesHit.Contains(enemy)){
			enemiesHit.Add(enemy);
			//If it's an enemy we damage him
			enemy.GetComponent<IAController>().getHurt(damage,(kameEffect.transform.position+enemy.transform.position)/2f);
			//We find the radius of areaEffect
			enemy.GetComponent<Rigidbody>().AddExplosionForce(forceExplosion,transform.position,1f);
			GameObject newEffect = GameObject.Instantiate (enemyHitEffectPrefab) as GameObject;
			newEffect.transform.position = enemy.GetComponent<Rigidbody> ().worldCenterOfMass - (kameEffect.transform.forward * 0.15f);
			Vector3 direction = (enemy.transform.position - kameCore.transform.position).normalized + (enemy.transform.up * 2f);
			enemy.GetComponent<Rigidbody> ().AddForce (direction.normalized * forceExplosion,ForceMode.Impulse);
			GameManager.comboManager.addCombo ();
			if(!elementAttack.Equals(ElementType.None)){
				AttackElementsManager.getElement(elementAttack).doEffect(enemy);
			}
		}
	}

	protected override void update(){

	}

	private IEnumerator resetTrails(){
		TrailRenderer tr = kameEffect.GetComponent<TrailRenderer>();
		TrailRenderer[] renderers = kameEffect.GetComponentsInChildren<TrailRenderer> ();
		float[] tempoTimes = new float[renderers.Length];
		float tmp = tr.time;
		tr.time = -1;
		for(int i = 0;i<renderers.Length;++i){
			tempoTimes[i] = renderers[i].time;
			renderers[i].time = -1;
		}
		yield return new WaitForEndOfFrame();
		
		for(int i = 0;i<renderers.Length;++i){
			renderers[i].time = tempoTimes[i];
		}
		tr.time = tmp;
		kameEffect.GetComponent<TrailRenderer>().startWidth = maxWidthKame;
		kameEffect.GetComponent<TrailRenderer>().endWidth = maxWidthKame ;
		yield return null;
	}

	private IEnumerator makeKameTrail(){
		timer = 0f;
		StartCoroutine ("resetTrails");
		while(timer<totalTimeLasts){
			timer+=Time.deltaTime;
			float ratio = (timer)/totalTimeLasts;
			float magnitude = ratio * distance;
			
			Vector3 newPostion = kameEffect.transform.position + (kameEffect.transform.forward * distance * Time.deltaTime) ;
			kameEffect.transform.position = newPostion;
			
			//We rotate the object
			Vector3 objectiveUp = (kameEffect.transform.position - closestPlanetCenter);
			objectiveUp = new Vector3(objectiveUp.x,objectiveUp.y,0f).normalized;
			Vector3 objectUp = new Vector3(kameEffect.transform.up.x,kameEffect.transform.up.y,0f).normalized;
			kameEffect.transform.rotation = Quaternion.FromToRotation (objectUp, objectiveUp) *kameEffect.transform.rotation;

			elementalParticleSystem.transform.position = kameEffect.transform.position;

			yield return null;
		}
	}

	private IEnumerator chargeKame(){
		timer = 0f;
		while(timer<chargeTime && !isCharged){
			timer+=Time.deltaTime;
			kameCore.transform.position = GameManager.lightGemObject.transform.position;
			kameEffect.transform.position = GameManager.lightGemObject.transform.position;
			elementalParticleSystem.transform.position = kameEffect.transform.position;
			yield return null;
		}
	}

	private IEnumerator cleanKameTrail(){
		timerCleanTrail = 0f;
		while(timerCleanTrail<timeToDisappear){
			timerCleanTrail+=Time.deltaTime;
			float ratio = timerCleanTrail/timeToDisappear;
			float invertRatio = 1f - ratio;
			kameEffect.GetComponent<TrailRenderer>().startWidth = maxWidthKame * invertRatio;
			kameEffect.GetComponent<TrailRenderer>().endWidth = maxWidthKame * invertRatio;
			yield return null;
		}
	}

	private void initializeVariables(){
		kameEffect.transform.rotation = Quaternion.LookRotation (GameManager.player.transform.forward, GameManager.player.transform.up);
		GameObject[] planets = GravityBodiesManager.getGravityBodies ();
		float smallestDistance = float.PositiveInfinity;
		foreach(GameObject planet in planets){
			if(Vector3.Distance(planet.transform.position,GameManager.player.transform.position)<smallestDistance){
				closestPlanetCenter = planet.transform.position;
				smallestDistance = Vector3.Distance(planet.transform.position,GameManager.player.transform.position);
			}
		}
		isFinished = false; 
		//kameEffect.SetActive (true);
		kameEffect.SetActive (false);
		kameCore.SetActive (true);
		kameCore.GetComponent<ParticleSystem>().Play();
		startPosition = ((GameManager.player.GetComponent<Rigidbody> ().worldCenterOfMass + (GameManager.player.transform.up * 0.15f))) + (kameEffect.transform.forward * 0.4f);
		kameEffect.transform.position = startPosition;
		kameCore.transform.position = startPosition;
		kameCore.transform.forward = kameEffect.transform.forward;
		timer = 0f;
		objectsCollided = 0;
		GameManager.playerAnimator.SetTrigger("isChargingKame");
		isCharged = false;

	}



	IEnumerator doKame(){
		enemiesHit = new List<GameObject> (0);
		initializeVariables ();
		StartCoroutine ("chargeKame");
		if(!elementAttack.Equals(ElementType.None)){
			elementalParticleSystem.SetActive(true);
			elementalParticleSystem.GetComponent<ParticleSystem>().Play();
			Material material = AttackElementsManager.getElement(elementAttack).material;
			if(material!=null){
				elementalParticleSystem.GetComponent<ParticleSystemRenderer >().material = material;
			}
		}
		yield return new WaitForSeconds (chargeTime);
		isCharged = true;
		GameManager.playerAnimator.SetBool("isDoingKame",true);
		kameEffect.SetActive (true);

		//We set the trail farther away because a few fotograms will be cut when we clean the trail
		kameEffect.transform.position = GameManager.lightGemObject.transform.position - (kameEffect.transform.forward * distance * Time.deltaTime * 2f);
		StartCoroutine ("makeKameTrail");

		yield return new WaitForSeconds (totalTimeLasts);

		GameManager.playerAnimator.SetBool("isDoingKame",false);
		kameCore.GetComponent<ParticleSystem>().Stop();
		if(!elementAttack.Equals(ElementType.None)){
			elementalParticleSystem.GetComponent<ParticleSystem>().Stop();
		}
		isFinished = true;
		StartCoroutine ("cleanKameTrail");
		yield return new WaitForSeconds (timeToDisappear);
		if(isFinished){
			kameEffect.SetActive (false);
			kameCore.SetActive (false);
			elementalParticleSystem.SetActive(false);
		}
	}



	public override void startAttack(){

		StartCoroutine("doKame");
	}
}

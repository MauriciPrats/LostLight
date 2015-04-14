using UnityEngine;
using System.Collections;

public class TrackingMissile : MonoBehaviour {
	
	Vector3 speed;
	float totalspeed = 2.5f;

	public float basStartSpeed = 5.5f;
	public float speedTimeMultiplier = 1.2f;
	public float angleTimeIncrement = 0.05f;
	public float explosionMaxScale = 1f;
	public float timeExplode = 0.8f;

	public GameObject elementalParticleEffect;
	
	float timeHasBeenAlive;
	
	bool hasArrived = false;
	float timerWhenArrived;

	TrackingMissilesAttack originalAttack;
	GameObject objective;
	bool isDying = false;

	void OnTriggerEnter(Collider other) {

			if(other.tag.Equals("Enemy")){
				originalAttack.enemyHit(other.gameObject);
				//Destroy(gameObject);
				isDying = true;
			}else if(other.tag.Equals("Planet")){
				//Destroy(gameObject);
				isDying = true;
			}

	}

	public void initialize(Vector3 vectorUp,GameObject objective,TrackingMissilesAttack originalAttack,ElementType elementType){
		this.objective = objective;
		this.originalAttack = originalAttack;
		speed = new Vector3 (Random.value, Random.value,0f).normalized;
		speed = ((speed * 0.8f) + vectorUp).normalized;

		if(!elementType.Equals(ElementType.None)){
			Debug.Log("aaaa");
			elementalParticleEffect.SetActive(true);
			elementalParticleEffect.GetComponent<ParticleSystem>().Play();
			Material material = AttackElementsManager.getElement(elementType).material;
			if(material!=null){
				elementalParticleEffect.GetComponent<ParticleSystemRenderer >().material = material;
			}
		}
	}
	// Use this for initialization
	void Start () {
		timeHasBeenAlive = 0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		timeHasBeenAlive += Time.deltaTime;
		if(isDying){
			if(!hasArrived){
				hasArrived = true;
				timerWhenArrived = 0f;
			}
			timerWhenArrived+=Time.deltaTime;
			float ratio = timerWhenArrived/timeExplode;
			ratio = explosionMaxScale * ratio;
			transform.localScale = new Vector3(ratio,ratio,ratio);
			if(timerWhenArrived>=timeExplode){
				Destroy(gameObject);
			}
			
		}else{
			totalspeed = basStartSpeed + (timeHasBeenAlive * speedTimeMultiplier);
			transform.position = transform.position+(speed * Time.deltaTime * totalspeed);
			if(objective!=null){
				Vector3 objectiveDirection = objective.transform.position -  transform.position;
				int angleRotation =(int) (timeHasBeenAlive/angleTimeIncrement);
				speed = Vector3.RotateTowards(speed,objectiveDirection,Mathf.Deg2Rad * angleRotation,1f).normalized;
			}
		
		}
	//}
	}
}

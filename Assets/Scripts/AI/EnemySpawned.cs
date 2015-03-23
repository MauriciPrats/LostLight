using UnityEngine;
using System.Collections;
using System;
public class EnemySpawned : MonoBehaviour {

	public int pointsCost;
	public GameObject lightsOnDeathPrefab;
	public int numLights;
	public Action<GameObject> actionToCallOnDie;
	public Action<GameObject> actionToCallOnDespawn;

	private bool isDespawned = false;
	private float timerCheckPlayerDistance = 0f;


	Killable killable;
	// Use this for initialization
	void Start () {
		killable = GetComponent<Killable> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(killable.isDead() && !isDespawned){
			isDespawned = true;
			OnEnemyDead();
		}
		timerCheckPlayerDistance += Time.deltaTime;

		if(timerCheckPlayerDistance>Constants.TIME_BETWEEN_CHECK_PLAYER_DISTANCE_FOR_DESPAWN){
			timerCheckPlayerDistance =  0f;
			if(Vector3.Distance(GameManager.player.transform.position,transform.position)> Constants.SPAWNING_MAXIMUM_DISTANCE_OF_PLAYER){
				OnDespawn();
			}
		}
	}

	public void OnEnemyDead(){
		//GetComponent<Collider>().enabled = false;
		Vector3 centerBoar = GetComponent<Rigidbody> ().worldCenterOfMass;
		int numberLights = numLights +UnityEngine.Random.Range (-1, 1);
		for(int i = 0;i<numberLights;i++){
			GameObject newLight = GameObject.Instantiate(lightsOnDeathPrefab) as GameObject;
			newLight.transform.position = (centerBoar);
			newLight.GetComponent<LightOnDeath>().setVectorUp(transform.up);
			int randRGB = UnityEngine.Random.Range(0,3);
			Color color = new Color(1f,1f,1f);
			float complementary = 1f;
			float mainColor = 0.85f;
			if(randRGB==0){
				color = new Color(mainColor,complementary,complementary);
			}else if(randRGB==1){
				color = new Color(complementary,mainColor,complementary);
			}else{
				color = new Color(complementary,complementary,mainColor);
			}
			newLight.GetComponent<TrailRenderer>().material.color = color;
		}
		//Create light spheres

		Destroy (gameObject);
		actionToCallOnDie (gameObject);
	}

	public void OnDespawn(){
		actionToCallOnDespawn (gameObject);
	}
}

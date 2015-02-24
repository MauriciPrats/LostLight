using UnityEngine;
using System.Collections;

public class QuickTimeEventManager : MonoBehaviour {

	public GameObject asteroidAttackPrefab;
	public GameObject player;

	private GravityBody playerGravityBody;

	private float quickTimeEventTimer = 0f;
	private float quickTimeEventActualCooldown = 0f;

	// Use this for initialization
	void Start () {
		playerGravityBody = player.GetComponent<GravityBody> ();
		putRandomCooldown ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		quickTimeEventTimer += Time.deltaTime;

		if(playerGravityBody.getMinimumPlanetDistance()>Constants.MINIMUM_PLANET_DISTANCE_FOR_QUICK_TIME_EVENT
		   && quickTimeEventTimer>quickTimeEventActualCooldown){

			putRandomCooldown();

			Vector3 randomDirection =Vector3.Cross(player.rigidbody.velocity.normalized,Vector3.back).normalized;
			
			if(Random.value>0.5f){
				randomDirection = randomDirection * -1f;
			}
			randomDirection -= player.rigidbody.velocity.normalized*Random.value;

			Vector3 asteroidPosition = player.transform.position - (randomDirection * Constants.ASTEROID_SPEED * 2f);
			//Asteroid position

			//Spawn the asteroid
			GameObject newAsteroid = (GameObject) GameObject.Instantiate(asteroidAttackPrefab);
			newAsteroid.transform.position = asteroidPosition;
			newAsteroid.rigidbody.velocity = (randomDirection * Constants.ASTEROID_SPEED) + player.rigidbody.velocity;

			QuickTimeAsteroid qta = newAsteroid.GetComponent<QuickTimeAsteroid>();
			qta.Initialize(player);


		}
	}

	void putRandomCooldown(){
		quickTimeEventTimer = 0f;
		float randomness = (Random.value * 2f) - 1f; // -1 to 1
		quickTimeEventActualCooldown = Constants.TIME_BETWEEN_QUICK_TIME_EVENTS + (randomness * Constants.RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS);
	}
}

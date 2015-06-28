using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MundusPlanetEventsManager : PlanetEventsManager {

	public GameObject mundusPrefab;
	public GameObject mundusSpawnPosition;
	public GameObject positionOnPlanetExplode;
	public GameObject insideLastPlanetPrefab;
	public GameObject platformPrefab;


	bool firstCinematicPlayed = false;
	bool hasBeenActivated = false;

	private GameObject mundus;
	private GameObject bigPappadaDialogue;
	private GameObject littleGDialogue;
	private List<GameObject> fisures;
	private GameObject athmosphere;
	private GameObject insideLastPlanet;
	private bool isInSecondPhase = false;

	
	//Is called when the class is activated by the GameTimelineManager
	public override void isActivated(){
		if(!hasBeenActivated){
			if(isEnabled){

				//setFase2();
				//We instantiate the inside of the planet 
				insideLastPlanet = GameObject.Instantiate(insideLastPlanetPrefab) as GameObject;
				insideLastPlanet.transform.position = transform.position;

				//We create the new mundus
				mundus = GameObject.Instantiate(mundusPrefab) as GameObject;
				mundus.GetComponent<IAControllerMundus>().informPlanetEventManager(this);
				mundus.transform.position = mundusSpawnPosition.transform.position;

			}else{

			}
		}
	}

	private void setFase2(){
		//We instantiate the inside of the planet 
		insideLastPlanet = GameObject.Instantiate(insideLastPlanetPrefab) as GameObject;
		insideLastPlanet.transform.position = transform.position;
		
		mundus = GameObject.Instantiate(mundusPrefab) as GameObject;
		mundus.GetComponent<IAControllerMundus>().informPlanetEventManager(this);
		mundus.transform.position = mundusSpawnPosition.transform.position;

		mundus.GetComponent<IAControllerMundus> ().setPhase (2);
		StartCoroutine(CinematicChangeToPhase2());
	}

	private IEnumerator spawnPlatforms(){
		float timer = 0f;
		while(isInSecondPhase){
			timer+=Time.deltaTime;
			if(timer>=0.1f){
				timer = 0f;
				GameObject platform = GameObject.Instantiate(platformPrefab) as GameObject;
				float distance = 20f;
				Vector3 position = Vector3.up * 50f;
				position = Quaternion.Euler(new Vector3(0f,0f,Random.value*360f))*position;
				position += transform.position;
				platform.transform.position = position;
			}
			yield return null;
		}
	}

	private IEnumerator CinematicChangeToPhase2(){

		isInSecondPhase = true;
		GameObject staticPlatforms = insideLastPlanet.GetComponent<InsideLastPlanetContainer>().staticPlatforms;
		GameObject dynamicPlatforms = insideLastPlanet.GetComponent<InsideLastPlanetContainer>().dynamicPlatforms;

		mundus.GetComponent<GravityBody> ().setHasToApplyForce (false);
		yield return new WaitForSeconds (3f);
		//Deactivate player input and move camera
		GameManager.inputController.disableInputController ();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().followObjective (positionOnPlanetExplode);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZ (50f);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setCameraShaking ();
		yield return new WaitForSeconds (3f);
		//Explosion of the planet
		foreach(GameObject fisure in fisures){
			foreach(ParticleSystem ps in fisure.GetComponentsInChildren<ParticleSystem>()){
				ps.Stop();
			}
		}
		yield return new WaitForSeconds (1f);
		//Jump so the player won't stay in a strange state when the plane disappears
		GameManager.playerController.Jump ();

		yield return new WaitForSeconds (0.2f);
		GetComponent<Collider> ().enabled = false;
		GetComponent<Renderer> ().enabled = false;
		athmosphere.GetComponent<Renderer> ().enabled = false;
		float timer = 0f;
		foreach(Renderer child in dynamicPlatforms.GetComponentsInChildren<Renderer>()){
			child.GetComponent<Collider>().enabled = true;
		}
		insideLastPlanet.GetComponent<InsideLastPlanetContainer> ().coreParticlesExplosion.GetComponent<ParticleSystem> ().Play ();
		while(timer<1f){
			timer+=Time.deltaTime;
			foreach(Renderer child in dynamicPlatforms.GetComponentsInChildren<Renderer>()){
				Vector3 direction = child.transform.position - transform.position;
				child.transform.position +=direction.normalized*30f * Time.deltaTime;
			}
			mundus.transform.position += mundus.transform.up * 15f * Time.deltaTime;
			foreach(Renderer child in staticPlatforms.GetComponentsInChildren<Renderer>()){
				child.transform.position +=child.transform.up*15f * Time.deltaTime;
			}
			yield return null;
		}
		insideLastPlanet.GetComponent<InsideLastPlanetContainer> ().coreParticlesImplosion.GetComponent<ParticleSystem> ().Play ();
		foreach(Renderer child in dynamicPlatforms.GetComponentsInChildren<Renderer>()){
			child.GetComponent<Collider>().enabled = false;
		}

		StartCoroutine (spawnPlatforms ());
		
		GameManager.inputController.enableInputController ();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().stopCameraShaking ();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZ (20f);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().resetObjective();
	}

	public override void isDeactivated(){
		isInSecondPhase = false;
		Destroy(mundus);
		Destroy (insideLastPlanet);
		GetComponent<Renderer> ().enabled = true;
		GetComponent<Collider> ().enabled = true;
		athmosphere.GetComponent<Renderer> ().enabled = true;
		foreach(GameObject fisure in fisures){
			Destroy(fisure);
		}
		fisures.Clear ();
	}

	public void informFisure(GameObject fisure){
		fisures.Add (fisure);
	}

	public override void startButtonPressed(){

	}
	
	public override void planetCleansed(){

	}
	
	public override void informEventActivated (CutsceneIdentifyier identifyier){
		if(isEnabled){
			if(identifyier.Equals(CutsceneIdentifyier.LastPlanetMundusSecondPhase)){
				StartCoroutine(CinematicChangeToPhase2());
			}
		}
	}
	
	public override void initialize(){
		if(isEnabled){
			athmosphere = GetComponent<GravityAttractor>().getAthmosphere();
			fisures = new List<GameObject>(0);
			//GetComponent<PlanetCorruption> ().setCorruptionToClean ();
		}
	}
}

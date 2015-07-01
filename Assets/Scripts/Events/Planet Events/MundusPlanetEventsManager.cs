using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MundusPlanetEventsManager : PlanetEventsManager {

	public GameObject mundusPrefab;
	public GameObject mundusSpawnPosition;
	public GameObject positionOnPlanetExplode;
	public GameObject lastPlanetPrefab;
	public GameObject platformPrefab;
	public GameObject positionEndingBigP;
	public LayerMask layersToCollideRaycastPlatforms;


	bool firstCinematicPlayed = false;
	bool hasBeenActivated = false;

	private GameObject mundus;
	private GameObject bigPappadaDialogue;
	private GameObject littleGDialogue;
	private List<GameObject> fisures;
	private GameObject athmosphere;
	private GameObject lastPlanet;
	private bool isInSecondPhase = false;
	private bool isFinishedTransition = false;

	
	//Is called when the class is activated by the GameTimelineManager
	public override void isActivated(){
		if(!hasBeenActivated){
			if(isEnabled){

				//setFase2();

				//We instantiate the inside of the planet 

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
			if(timer>=0.05f){
				timer = 0f;

				float distance = 45f;
				Vector3 position = Vector3.up * 35f;
				position = Quaternion.Euler(new Vector3(0f,0f,Random.value*360f))*position;
				position += transform.position;
				position.z = GameManager.player.transform.position.z;
				bool isGoodPosition = true;
				Vector3 direction = transform.position - position;
				RaycastHit hit;
				if(Physics.SphereCast(position,1f,direction,out hit,50f,layersToCollideRaycastPlatforms)){
					if(hit.collider.gameObject.tag.Equals("MundusPlanetFragment") || hit.distance<1f){
						isGoodPosition = false;
					}
				}
				Collider[] colliders = Physics.OverlapSphere(position,3f);
				if(colliders.Length>1){
					isGoodPosition = false;
				}
				
				if(isGoodPosition){
					GameObject platform = GameObject.Instantiate(platformPrefab) as GameObject;
					platform.transform.parent = lastPlanet.transform;
					platform.transform.position = position;
				}
			}
			yield return null;
		}
	}

	private IEnumerator CinematicEndGame(){
		GameObject closestPlatform = getClosestPlatformTop (GameManager.player.transform.position);
		GUIManager.fadeIn (Menu.BlackMenu);
		GameManager.playerController.isInvulnerable = true;
		GameManager.inputController.disableInputController ();
		yield return new WaitForSeconds (1f);
		isInSecondPhase = false;
		GetComponent<PlanetCorruption> ().setCorruptionToClean ();

		GameManager.player.transform.position = positionEndingBigP.transform.position;
		yield return new WaitForSeconds (0.5f);
		GUIManager.fadeOut (null);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZ (10f);
	}

	private IEnumerator CinematicChangeToPhase2(){
		GameObject staticPlatforms = lastPlanet.GetComponent<MundusFightPlanet>().staticPlatforms;
		mundus.GetComponent<IAControllerMundus> ().setPhase (2);
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

		GameObject closestPlayerSafePlace = getClosestPlatformTop (GameManager.player.transform.position);

		yield return new WaitForSeconds (1f);

		//Jump so the player won't stay in a strange state when the plane disappears
		GameManager.playerController.Jump ();
		if (Vector3.Distance (closestPlayerSafePlace.transform.position, GameManager.player.transform.position) > 1f) {
			GameManager.playerController.Move (Util.getPlanetaryDirectionFromAToB(GameManager.player,closestPlayerSafePlace));
		}
		yield return new WaitForSeconds (0.4f);
		GetComponent<Collider> ().enabled = false;
		athmosphere.GetComponent<Renderer> ().enabled = false;
		float timer = 0f;
		lastPlanet.GetComponent<MundusFightPlanet> ().coreParticlesExplosion.GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (spawnPlatforms());
		while(timer<2f){
			timer+=Time.deltaTime;
			mundus.transform.position += mundus.transform.up * 8f * Time.deltaTime;
			foreach(Collider child in staticPlatforms.GetComponentsInChildren<Collider>()){
				Vector3 direccion = child.GetComponent<Rigidbody>().worldCenterOfMass -transform.position;
				direccion.z = 0f;
				child.transform.position +=direccion.normalized*8f * Time.deltaTime;
			}

			Vector3 position = closestPlayerSafePlace.transform.position;
			position.z = GameManager.player.transform.position.z;
			GameManager.player.transform.position  = position;
			yield return null;
		}
		lastPlanet.GetComponent<MundusFightPlanet> ().coreParticlesImplosion.GetComponent<ParticleSystem> ().Play ();



		mundus.GetComponent<IAControllerMundus> ().setPhase (2);
		GameManager.inputController.enableInputController ();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().stopCameraShaking ();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZ (20f);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().resetObjective();
		isFinishedTransition = true;
	}

	public bool getIsFinishedTransition(){
		return isFinishedTransition;
	}

	public Vector3 getInsidePlanetPosition(){
		return lastPlanet.transform.position;
	}
	public GameObject getClosestPlatformTop(Vector3 position){
		GameObject closestGO = null;
		float minimumDistance = float.MaxValue;
		foreach (MundusStaticPlatform pa in lastPlanet.GetComponentsInChildren<MundusStaticPlatform>()) {
			if(pa.positionToHoldOver!=null){
				if(Vector3.Distance(pa.positionToHoldOver.transform.position,position)<minimumDistance){
					minimumDistance = Vector3.Distance(pa.positionToHoldOver.transform.position,position);
					closestGO = pa.positionToHoldOver;
				}
			}
		}
		return closestGO;
	}

	public override void isDeactivated(){
		isInSecondPhase = false;
		Destroy(mundus);
		DestroyImmediate(lastPlanet);
		lastPlanet = GameObject.Instantiate(lastPlanetPrefab) as GameObject;
		lastPlanet.transform.position = transform.position;
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
				isFinishedTransition = false;
				isInSecondPhase = true;
				StartCoroutine(CinematicChangeToPhase2());
			}if(identifyier.Equals(CutsceneIdentifyier.MundusDies)){
				StartCoroutine(CinematicEndGame());
			}
		}
	}
	
	public override void initialize(){
		if(isEnabled){
			athmosphere = GetComponent<GravityAttractor>().getAthmosphere();
			fisures = new List<GameObject>(0);

			lastPlanet = GameObject.Instantiate(lastPlanetPrefab) as GameObject;
			lastPlanet.transform.position = transform.position;
			//GetComponent<PlanetCorruption> ().setCorruptionToClean ();
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanetCorruption : MonoBehaviour {

	public GameObject shintoDoor;
	public GameObject corruptionOrigin;
	private ShintoDoor shinto;
	public Material material;
	public float yMin = -3f;
	public float yMax = 30f;

	public float speed = 5f;
	public float particlesOnPurificationSpeed = 5f;
	private float direction = -1f;
	private float yValue = 0f;
	private float offset = 0f;
	private bool cleaningCorruption = false;

	private List<Material> materials;
	private Vector4 originPosition;
	// Use this for initialization
	void Awake () {
		yValue = yMin;
	}

	void Start(){
		if(shintoDoor!=null){
			shinto = shintoDoor.GetComponent<ShintoDoor> ();
		}
		originPosition = new Vector4 (corruptionOrigin.transform.position.x, corruptionOrigin.transform.position.y, corruptionOrigin.transform.position.z, 0f);
		ParticleSystem particlesOnDescorrupt = corruptionOrigin.GetComponent<ParticleSystem> ();
		if(particlesOnDescorrupt!=null){
			particlesOnDescorrupt.startSpeed = particlesOnPurificationSpeed;
			particlesOnDescorrupt.startLifetime = ((float)(yMax-yMin))/Mathf.Abs (particlesOnPurificationSpeed);
		}
	}

	public void initialize(){
		materials = new List<Material> (0);

		foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
			if(renderer.enabled && renderer.gameObject.activeSelf){
				foreach(Material material in renderer.materials){
					if(material.HasProperty("_YCutOut")){
						materials.Add(material);
					}
				}
			}
		}

	}

	public void cleanCorruption(){
		if(!cleaningCorruption){
			cleaningCorruption = true;
			//direction=1f;
			//shinto.activateKanjis ();
			//corruptionOrigin.GetComponent<ParticleSystem> ().Play ();
			//cleaningCorruption = true;
			//GameManager.player.GetComponent<InputController> ().enabled = false;
			GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZCameraCleansePlanet ();
			GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().followObjective(corruptionOrigin);
			StartCoroutine("startCleaningWithDelay");
		}
	}

	private IEnumerator startCleaningWithDelay(){
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().lerpMultiplyierUp = 1f;
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().lerpMultiplyierPos = 1.5f;
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().xAngle = 13f;
		float timer = 0f;
		while(timer<2f){
			timer+=Time.deltaTime;
			yield return null;
		}
		direction=1f;
		shinto.activateKanjis ();
		corruptionOrigin.GetComponent<ParticleSystem> ().Play ();

		//GameManager.player.GetComponent<InputController> ().enabled = false;
	}

	private IEnumerator resetCameraValuesWithDelay(){
		float timer = 0f;
		while(timer<1f){
			timer+=Time.deltaTime;
			float ratio = timer/1f;
			GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().lerpMultiplyierUp = 1f+(3.5f*ratio);
			GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().lerpMultiplyierPos = 1.5f+(3.5f*ratio);
			yield return null;
		}
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().lerpMultiplyierUp = 4.5f;
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().lerpMultiplyierPos = 5f;
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().xAngle = 17f;
		
		//GameManager.player.GetComponent<InputController> ().enabled = false;
	}

	public void isFinishedCleaning(){
		cleaningCorruption = false;
		//GameManager.player.GetComponent<InputController> ().enabled = true;
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().returnOriginalZ ();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().resetObjective ();
		StartCoroutine ("resetCameraValuesWithDelay");
	}

	public void corrupt(){
		direction=-1f;
	}
	// Update is called once per frame
	void Update () {
		offset += Time.deltaTime/100f;
		yValue += (direction * speed * Time.deltaTime);
		if(yValue>yMax){
			if(cleaningCorruption){
				isFinishedCleaning();
			}
			yValue = yMax;
		}
		if(yValue<yMin){
			yValue = yMin;
		}

		if(Input.GetKeyUp(KeyCode.C)){
			cleanCorruption();
		}
			foreach(Material material in materials){
				if(material.HasProperty("_YCutOut")){
					material.SetFloat("_YCutOut",yValue);
					material.SetTextureOffset("_CorruptionTexture",new Vector2(offset,0));
					material.SetVector("_OriginCorruption",originPosition);
				}
			}

		Renderer athmosphereRenderer = GetComponent<GravityAttractor> ().getAthmosphere ().GetComponent<Renderer> ();
		foreach(Material material in athmosphereRenderer.materials){
			if(material.HasProperty("_YCutOut")){
				material.SetFloat("_YCutOut",yValue);
				material.SetVector("_OriginCorruption",originPosition);
			}
		}

	}
}

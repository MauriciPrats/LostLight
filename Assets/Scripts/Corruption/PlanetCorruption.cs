using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlanetCorruption : MonoBehaviour {

	public GameObject container;
	public Material material;
	public float yMin = -3f;
	public float yMax = 30f;

	private float direction = 3f;
	private float yValue = 0f;
	private float offset = 0f;
	// Use this for initialization
	void Awake () {
		yValue = yMax;
	}


	public void initialize(){
		foreach(MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>()){
			if(meshRenderer.enabled && meshRenderer.gameObject.activeSelf){
				MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();
				Mesh mesh = meshFilter.mesh;
				if(mesh!=null){
					Mesh newMesh = Mesh.Instantiate(mesh);
					GameObject newObject = new GameObject();
					newObject.name = "CorruptCloneObject";
					newObject.AddComponent<MeshFilter>();
					newObject.AddComponent<MeshRenderer>();
					Material[] materials = new Material[meshRenderer.materials.Length];
					for(int i = 0;i<materials.Length;i++){
						materials[i] = material;
					}
					newObject.GetComponent<MeshRenderer>().materials = materials;
					newObject.GetComponent<MeshFilter>().mesh = newMesh;
					
					
					
					newObject.transform.localScale = meshRenderer.gameObject.transform.lossyScale;
					//If they are spheres, scale them a bit more
					
					if(mesh.name.Equals("Sphere Instance")){newObject.transform.localScale*=1.001f;}
					newObject.transform.position = meshRenderer.gameObject.transform.position;
					newObject.transform.rotation = meshRenderer.gameObject.transform.rotation;
					newObject.transform.parent = container.transform;
					newObject.layer = LayerMask.NameToLayer("Planets");
					
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		offset += Time.deltaTime/100f;
		yValue += (direction * Time.deltaTime);
		if(yValue>yMax){
			yValue = yMax;
		}
		if(yValue<yMin){
			yValue = yMin;
		}

		if(Input.GetKeyUp(KeyCode.C)){
			direction*=-1f;
		}

		foreach(Renderer renderer in container.GetComponentsInChildren<Renderer>()){
			foreach(Material material in renderer.materials){
				if(material.HasProperty("_YCutOut")){
					material.SetFloat("_YCutOut",yValue);
					material.SetTextureOffset("_MainTex",new Vector2(offset,0));
				}
			}
		}

		Renderer athmosphereRenderer = GetComponent<GravityAttractor> ().getAthmosphere ().GetComponent<Renderer> ();
		foreach(Material material in athmosphereRenderer.materials){
			if(material.HasProperty("_YCutOut")){
				material.SetFloat("_YCutOut",yValue);
			}
		}

		foreach(ParticleSystem particles in container.GetComponentsInChildren<ParticleSystem>()){
			if(particles.gameObject.transform.position.y<yValue){
				if(!particles.isPlaying){
					particles.Play();
				}
			}else{
				particles.Stop();
			}
		}
	}
}

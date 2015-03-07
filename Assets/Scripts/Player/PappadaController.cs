using UnityEngine;
using System.Collections;

public class PappadaController : MonoBehaviour {

	public float maxPappadaScale;
	public float minPappadaScale;


	// Use this for initialization
	void Start () {
		newProportionOfLife (1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void newProportionOfLife(float proportion){
		float scale = (maxPappadaScale - minPappadaScale) * proportion;
		scale += minPappadaScale;
		if(scale<minPappadaScale){scale = minPappadaScale;}
		transform.localScale = new Vector3 (scale, scale, scale);


		//transform.renderer.material.color = new Color (1f - proportion, 0f, 0f);
	}
}

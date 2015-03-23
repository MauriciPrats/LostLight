using UnityEngine;
using System.Collections;

public class KameOnHitExplosion : MonoBehaviour {

	public float timeToGrow = 0.3f;
	public float maxScaleToGrow = 0.5f;
	float timer;
	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if(timer>=timeToGrow){
			Destroy(gameObject);
		}else{
			float ratio = (timer/timeToGrow) * maxScaleToGrow;
			transform.localScale = new Vector3(ratio,ratio,ratio);
		}
	}
}

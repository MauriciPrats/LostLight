﻿using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour {

	public int HP = 3;

	// Use this for initialization
	void Start () {
		renderer.material.color = new Color (0.0f, 1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Damage (int AttackPoints) {
		
		HP -= AttackPoints;
		if (HP == 2) {
			renderer.material.color = new Color (1.0f, 1.0f, 0.0f);
		}
		if (HP == 1) {
			renderer.material.color = new Color (1.0f, 0.0f, 0.0f);
		}
		if (HP == 0) {
			renderer.material.color = new Color (0.0f, 0.0f, 0.0f);
			
			//TODO: Deactivate the enemy
			this.GetComponent<IAController>().enabled = false;
			
			//transform.position -= new Vector3(0.0f,0.02f,0.0f);
			StartCoroutine("Fade");
		}
		
	}
	
	IEnumerator Fade() {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			yield return null;
		}
	}
	
}

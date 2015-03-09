using UnityEngine;
using System.Collections;
using System;

public class OnAnyKeyPressed : MonoBehaviour {

	public Action<string> action;

	public string scene;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey){
			GameManager.actualSceneManager.ChangeScene(scene);
		}
	}
}

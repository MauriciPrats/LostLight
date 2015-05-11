using UnityEngine;
using System.Collections;

public class TongueController : MonoBehaviour {

	void Awake(){
		GameManager.registerPlayerTongue (gameObject);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class LightGem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.registerLightGemObject (gameObject);
	}
}

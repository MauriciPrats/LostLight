using UnityEngine;
using System.Collections;

public class QuickTimeAsteroid : MonoBehaviour {
	

	public GameObject quickTimePopupPrefab;

	private GameObject player;
	private GameObject quickTimePopup;
	// Use this for initialization
	public void Initialize (GameObject playerGO) {
		player = playerGO;

		quickTimePopup = (GameObject)GameObject.Instantiate (quickTimePopupPrefab);
		quickTimePopup.transform.position = transform.position + Vector3.up * 1f;
		quickTimePopup.transform.parent = transform;
		quickTimePopup.GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		quickTimePopup.GetComponent<Renderer>().enabled = false;
		if(Vector3.Distance(player.transform.position,transform.position)<Constants.DISTANCE_SHOW_POPUP){
			quickTimePopup.GetComponent<Renderer>().enabled = true;

			if(Input.GetKey(KeyCode.X)){
				Destroy(quickTimePopup);
				Destroy(this.gameObject);
			}
		}

	}
}

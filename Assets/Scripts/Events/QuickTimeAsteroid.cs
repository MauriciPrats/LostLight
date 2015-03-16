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
		quickTimePopup.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		quickTimePopup.renderer.enabled = false;
		if(Vector3.Distance(player.transform.position,transform.position)<Constants.Instance.DISTANCE_SHOW_POPUP){
			quickTimePopup.renderer.enabled = true;

			if(Input.GetKey(KeyCode.X)){
				Destroy(quickTimePopup);
				Destroy(this.gameObject);
			}
		}

	}
}

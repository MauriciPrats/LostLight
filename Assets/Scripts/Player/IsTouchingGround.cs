using UnityEngine;
using System.Collections;

public class IsTouchingGround : MonoBehaviour {
	GravityBody playerGB;
	// Use this for initialization
	void Start () {
		playerGB = GameManager.player.GetComponent<GravityBody> ();
	}
	void OnTriggerEnter (Collider col)
	{

		playerGB.checkTouchEnter (col.gameObject);
	}
	
	void OnCollisionEnter (Collision col)
	{
		playerGB.checkTouchEnter (col.gameObject);
	}
	
	void OnTriggerExit(Collider col)
	{
		playerGB.checkTouchExit (col.gameObject);
	}
	
	void OnCollisionExit(Collision col)
	{
		playerGB.checkTouchExit (col.gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}
}

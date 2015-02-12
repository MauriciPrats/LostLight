using UnityEngine;
using System.Collections;

public class IAController : MonoBehaviour {


	public float minimumDistanceSeePlayer = 30f;
	public LayerMask layersToFindCollision;


	private GameObject player;
	// Use this for initialization
	void Start () {
		player = (GameObject)GameObject.FindGameObjectWithTag("Player");
	}

	private bool canSeePlayer(){
		Vector3 playerDirection = player.transform.position - transform.position;
		if(playerDirection.magnitude<minimumDistanceSeePlayer){
			RaycastHit hit;

			if (Physics.Raycast(transform.position,playerDirection, out hit, minimumDistanceSeePlayer,layersToFindCollision))
			{
				Collider target = hit.collider; // What did I hit?
				//Debug.Log(target.name);
				if(target.tag == "Player"){ return true;}
				else{ return false; }
			}
		}

		return false;
	}

	private void offensiveMoves(){

	}

	private void idleWalking(){


	}

	private void walkRight(){

	}

	private void walkLeft(){

	}

	private void jump(){

	}

	private void attack(){

	}
	// Update is called once per frame
	void Update () {
		if(canSeePlayer ()){
			offensiveMoves();
		}else{
			idleWalking();
		}
		//canSeePlayer ();
	}
}

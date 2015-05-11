using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {

	private Vector3 direction;
	private Vector3 gravityDirection;

	private Attack parentAttack;


	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.layer == LayerMask.NameToLayer("Player")){
			parentAttack.enemyCollisionEnter (collision.gameObject);
			Destroy(gameObject);
		}else if(collision.gameObject.layer == LayerMask.NameToLayer("Planets")){
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.layer == LayerMask.NameToLayer("Player")){
			parentAttack.enemyCollisionEnter (collider.gameObject);
			Destroy(gameObject);
		}else if(collider.gameObject.layer == LayerMask.NameToLayer("Planets")){
			Destroy(gameObject);
		}
	}

	public void setParameters(Vector3 direction,Vector3 gravityDirection,Attack parentAttack){
		this.gravityDirection = gravityDirection;
		this.parentAttack = parentAttack;
		this.direction = direction;
	}

	void Update(){
		if(direction!=null){
			direction+=(gravityDirection*Time.deltaTime);
			transform.position+=(direction*Time.deltaTime);
		}
	}
}

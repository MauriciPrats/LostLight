using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {

	public float dashSpeed = 30f;
	public float dashTime = 0.3f;
	public GameObject dashStartParticles;
	public LayerMask layerToDash;
	public float dashCooldown = 1f;

	private bool cooldownFinished = true;
	private bool isDoingDash = false;

	Vector3 originalMovement;

	public bool getIsDoingDash(){
		return isDoingDash;
	}

	public bool isCooldownFinished(){
		return cooldownFinished;
	}

	IEnumerator doDash(){
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemy"),true);
		isDoingDash = true;
		cooldownFinished = false;

		dashStartParticles.SetActive (true);
		dashStartParticles.GetComponent<ParticleSystem> ().Play ();
		dashStartParticles.transform.position = GameManager.player.GetComponent<Rigidbody>().worldCenterOfMass;
		originalMovement = GameManager.player.GetComponent<CharacterController>().getMoveAmout ();
		GameManager.playerAnimator.SetBool("isWalking",true);
		Vector3 newMove;
		if(GameManager.player.GetComponent<PlayerController>().getIsLookingRight()){
			newMove = (dashSpeed) * -GameManager.player.transform.right;
		}else{
			newMove = (dashSpeed) * GameManager.player.transform.right;
		}
		GameManager.player.GetComponent<CharacterController> ().setMoveAmount (newMove);
		yield return new WaitForSeconds(dashTime);
		isDoingDash = false;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemy"),false);
		GameManager.player.GetComponent<CharacterController> ().setMoveAmount (originalMovement);
		GameManager.playerAnimator.SetBool("isWalking",false);
		yield return new WaitForSeconds(dashCooldown);
		dashStartParticles.SetActive (false);
		cooldownFinished = true;
	}

	public void startAction(){
		StartCoroutine ("doDash");
	}
}

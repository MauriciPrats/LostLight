using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour {

	public float dashSpeed = 30f;
	public float dashTime = 0.3f;
	public GameObject dashStartParticles;
	public LayerMask layerToDash;
	public float dashCooldown = 1f;
	public LayerMask maskToCollide;
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
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Dashing"),true);
		isDoingDash = true;
		cooldownFinished = false;

		dashStartParticles.SetActive (true);
		dashStartParticles.GetComponent<ParticleSystem> ().Play ();
		dashStartParticles.transform.position = GameManager.player.GetComponent<Rigidbody>().worldCenterOfMass;
		originalMovement = GameManager.player.GetComponent<CharacterController>().getMoveAmout ();
		GameManager.playerAnimator.SetTrigger("isDashing");
		Vector3 newMove;
		newMove = (dashSpeed) * GameManager.player.transform.forward;
		GameManager.player.GetComponent<CharacterController> ().setMoveAmount (newMove);

		float distance = dashSpeed * dashTime;
		float dashTimeR = dashTime;
		//Raycast 
		RaycastHit hit;

		Vector3 velocity = GameManager.player.GetComponent<Rigidbody> ().velocity;
		Vector3 forward = GameManager.player.transform.forward;
		Vector3 direction = (velocity + (forward * dashSpeed)).normalized;
		Vector3 position = GameManager.player.GetComponent<Rigidbody> ().worldCenterOfMass;

		if (Physics.Raycast (position,direction, out hit, distance, layerToDash)) {
			dashTimeR = ((hit.distance/distance)*dashTime);
		}
		dashTimeR = dashTimeR - 0.05f;

		yield return new WaitForSeconds(dashTimeR);
		isDoingDash = false;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemy"),false);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Dashing"),false);
		GameManager.player.GetComponent<CharacterController> ().setMoveAmount (originalMovement);
		GameManager.playerAnimator.SetBool("isWalking",false);
		yield return new WaitForSeconds(dashCooldown);
		dashStartParticles.SetActive (false);
		cooldownFinished = true;
		GameManager.playerAnimator.ResetTrigger("isDashing");
	}

	public void startAction(){
		StartCoroutine ("doDash");
	}
}

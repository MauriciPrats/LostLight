using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (CharacterController))]

public class InputController : MonoBehaviour {
	public float startChargeSpaceJump;
	public float maxDistanceToInteract;

	private float timeIsSpaceJumpCharged;
	private bool isSpaceJumpCharged;

	private float timeJumpPressed;
	private bool isSpaceJumping;
	private CharacterController character;

	void Start () {
		timeJumpPressed = 0;
		character = GetComponent<CharacterController>();
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }
		//Movement Input.
		if (Input.GetAxis ("Horizontal")!=0f) {
			ResetJumping ();
			character.Move ();
		} else {
			character.StopMove ();
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			ResetJumping ();
			character.Attack ();
		} else {
			character.StopAttack();
		}

		if (Input.GetKeyUp (KeyCode.Space) && isSpaceJumpCharged) { ResetJumping(); character.SpaceJump(); }
		if (Input.GetKeyUp (KeyCode.Space) && !isSpaceJumpCharged) { ResetJumping (); character.Jump(); }

		//Setting jumping Inputs
		if (Input.GetKey (KeyCode.Space)) {
			timeJumpPressed += Time.deltaTime;
			if (timeJumpPressed >= startChargeSpaceJump) { character.ChargeJump(); }
			if (timeJumpPressed >= timeIsSpaceJumpCharged) {isSpaceJumpCharged = true; }
		} else {
			ResetJumping();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			Interactuable entity = EntityManager.Instance.GetClosestInteractuableEntity(this.gameObject, maxDistanceToInteract);
			if (entity != null) entity.doInteractAction();
		}
	}

	void ResetJumping () {
		isSpaceJumpCharged = false;
		isSpaceJumping = false;
		timeJumpPressed = 0;
	}
}


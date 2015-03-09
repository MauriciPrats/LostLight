using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (CharacterController))]

public class InputController : MonoBehaviour {
	public float startChargeSpaceJump;
	public float timeIsSpaceJumpCharged;
	public float maxDistanceToInteract;

	
	private bool isSpaceJumpCharging = false;
	private bool isSpaceJumpCharged;
	private float timeJumpPressed;
	private CharacterController character;
	private GravityBody characterGravityBody;

	private bool isCraftingMenuOpen = false;

	void Start () {
		timeJumpPressed = 0;
		character = GetComponent<CharacterController>();
		characterGravityBody = character.GetComponent<GravityBody> ();
		WeaponManager wpm = WeaponManager.Instance;
	}

	void Update() {
		if(!GameManager.gameState.isGameEnded){
			if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }
			//Movement Input.
			if (Input.GetAxis ("Horizontal")!=0f) {
				ResetJumping ();
				character.Move ();
			} else {
				character.StopMove ();
			}
			//starts the charge of the attack
			if (Input.GetKeyDown(KeyCode.Q)) {
				character.StartAttack();
			}
			//releases the attack
			if (Input.GetKeyUp (KeyCode.Q)) {
				ResetJumping ();
				character.Attack ();
			}

			if (Input.GetKeyUp (KeyCode.Space) && isSpaceJumpCharged) {
				ResetJumping(); 
				character.SpaceJump(); 
			}else if (Input.GetKeyUp (KeyCode.Space) && !isSpaceJumpCharged && characterGravityBody.getIsTouchingPlanet()) { 
				ResetJumping (); 
				character.Jump(); 
			}

			//Setting jumping Inputs
			if (Input.GetKey (KeyCode.Space)) {
				timeJumpPressed += Time.deltaTime;
				if (timeJumpPressed >= startChargeSpaceJump && !isSpaceJumpCharging) {isSpaceJumpCharging = true; }
				if (timeJumpPressed >= timeIsSpaceJumpCharged) {isSpaceJumpCharged = true; character.ChargeJump(); }
			} else {
				ResetJumping();
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				Interactuable entity = EntityManager.getClosestInteractuable();
				if (entity != null) entity.doInteractAction();
			}

			if(Input.GetKeyUp(KeyCode.Escape)){
				GUIManager.closeCraftingMenu();
			}
		}
	}

	void ResetJumping () {
		isSpaceJumpCharging = false;
		isSpaceJumpCharged = false;
		timeJumpPressed = 0;
	}
}


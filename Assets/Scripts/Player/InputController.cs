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


			//MOVEMENT BUTTON
			if (Input.GetAxis ("Horizontal")!=0f) {
				if(isSpaceJumpCharged){
					//character.CancelChargingSpaceJump();
					character.MoveArrow(Input.GetAxisRaw ("Horizontal"),Input.GetAxis ("Vertical"));
					//character.StopMove();
				}else if(isCharacterAllowedToMove()){
					ResetJumping ();
					character.Move ();
				}else{
					character.StopMove ();
				}
			} else {
				character.StopMove ();
			}


			//NORMAL ATTACK BUTTON
			if(character.getIsJumping() && !character.getIsSpaceJumping() && !character.isDoingAttack()){
				if (Input.GetButtonUp("Normal Attack")) {
					//Uppercut
					character.doOnAir();
				}
			}else if(Mathf.Abs(Input.GetAxisRaw("Vertical"))>Mathf.Abs(Input.GetAxisRaw("Horizontal"))){
				if (Input.GetButtonUp("Normal Attack") && Input.GetAxisRaw("Vertical")>0f) {
					//Uppercut
					character.doUppercut();
				}else if(Input.GetButtonUp("Normal Attack") && Input.GetAxisRaw("Vertical")<0f){
					//Undercut (De momento, normal attack)
					character.StartAttack();
				}
			}else{
				if (Input.GetButtonUp("Normal Attack") && isCharacterAllowedToDoNormalAttack()) {
					character.StartAttack();
				}
			}

			//SPECIAL ATTACK BUTTON
			if (Input.GetButton("Special Attack") && isCharacterAllowedToDoSpecialAttack()) {
				if(Mathf.Abs(Input.GetAxisRaw("Vertical"))>Mathf.Abs(Input.GetAxisRaw("Horizontal"))){
					if(Input.GetAxisRaw("Vertical")>0f){
						GetComponent<CharacterSpecialAttackController>().doUpSpecialAttack();
					}else if(Input.GetAxisRaw("Vertical")<0f){
						GetComponent<CharacterSpecialAttackController>().doDownSpecialAttack();
					}
				}else{
					GetComponent<CharacterSpecialAttackController>().doSidesSpecialAttack();
				}
			}

			//JUMP BUTTON
			if (Input.GetButtonUp("Jump") && isSpaceJumpCharged) {
				ResetJumping(); 
				character.SpaceJump(); 
			}else if (Input.GetButtonUp("Jump") && !isSpaceJumpCharged && characterGravityBody.getIsTouchingPlanet()) { 
				ResetJumping (); 
				character.Jump(); 
			}

			if (Input.GetButton("Jump")) {
				if(!character.getIsSpaceJumping()){
					timeJumpPressed += Time.deltaTime;
					if (timeJumpPressed >= startChargeSpaceJump && !isSpaceJumpCharging) {isSpaceJumpCharging = true; }
					if (timeJumpPressed >= timeIsSpaceJumpCharged && !isSpaceJumpCharged) {isSpaceJumpCharged = true; character.ChargeJump(); }
				}
			} else {
				ResetJumping();
			}

			if (Input.GetButtonDown("Jump")){
				if(character.getIsSpaceJumping()){
					GravityBody body = GetComponent<GravityBody>();
					if(body.getIsOrbitingAroundPlanet()){
						body.setIsGettingOutOfOrbit(true);
					}
				}
			}

			//BLOCK BUTTON
			if(Input.GetButton("Block") && isSpaceJumpCharged){
				CancelChargingSpaceJump();
			}else if(Input.GetButton("Block") && isCharacterAllowedToDash()){
				character.doDash();
			}

			if (Input.GetButton("Block")) {
				Interactuable entity = EntityManager.getClosestInteractuable();
				if (entity != null) entity.doInteractAction();
			}




			//OTHER BUTTONS
			if(Input.GetKeyUp(KeyCode.Escape)){
				GUIManager.closeCraftingMenu();
			}

			if(Input.GetKeyUp(KeyCode.P)){
				if(!GameManager.gameState.isGameEnded){
					if(!GameManager.gameState.isGamePaused){
						GameManager.pauseGame();
						GUIManager.activatePauseMenu();
					}else{
						GameManager.unPauseGame();
						GUIManager.deactivatePauseMenu();
					}
				}
			}
		}
	}

	bool isCharacterAllowedToMove(){
		if(GetComponent<CharacterSpecialAttackController>().isDoingAnySpecialAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDoSpecialAttack(){
		if(character.getIsJumping()){
			return false;
		}else if(GetComponent<CharacterAttackController>().isAttacking){
			return false;
		}else if(GetComponent<CharacterSpecialAttackController>().isDoingAnySpecialAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDoNormalAttack(){
		if(GetComponent<CharacterAttackController>().isAttacking){
			return false;
		}else if(GetComponent<CharacterSpecialAttackController>().isDoingAnySpecialAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDash(){
		if(GetComponent<CharacterAttackController>().isAttacking){
			return false;
		}else if(GetComponent<CharacterSpecialAttackController>().isDoingAnySpecialAttack()){
			return false;
		}
		return true;
	}

	void CancelChargingSpaceJump(){
		timeJumpPressed = 0f;
		isSpaceJumpCharged = false;
		isSpaceJumpCharging = false;
		character.CancelChargingSpaceJump ();
	}

	void ResetJumping () {
		isSpaceJumpCharging = false;
		isSpaceJumpCharged = false;
		timeJumpPressed = 0;
	}

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (PlayerController))]

public class InputController : MonoBehaviour {
	public float startChargeSpaceJump;
	public float timeIsSpaceJumpCharged;
	public float maxDistanceToInteract;

	
	private bool isSpaceJumpCharging = false;
	private bool isSpaceJumpCharged;
	private float timeJumpPressed;
	private PlayerController character;
	private CharacterAttackController attackController;
	private SpaceGravityBody characterGravityBody;

	private bool isCraftingMenuOpen = false;

	public AttackType upSpecialAttack;
	public AttackType sidesSpecialAttack;
	public AttackType downSpecialAttack;

	public AttackType upNormalAttack;
	public AttackType sidesNormalAttack;
	public AttackType downNormalAttack;

	public AttackType onAirAttack;

	void Start () {
		timeJumpPressed = 0;
		character = GetComponent<PlayerController>();
		characterGravityBody = character.GetComponent<SpaceGravityBody> ();
		attackController = GetComponent<CharacterAttackController> ();
		WeaponManager wpm = WeaponManager.Instance;
	}

	void Update() {
		if(!GameManager.gameState.isGameEnded){
			if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }


			//MOVEMENT BUTTON
			if(!attackController.isDoingDash()){
				if (Input.GetAxis ("Horizontal")!=0f) {
					if(isSpaceJumpCharged){
						character.MoveArrow(Input.GetAxisRaw ("Horizontal"),Input.GetAxis ("Vertical"));
					}else if(isCharacterAllowedToMove()){
						ResetJumping ();
						character.Move ();

					}else{
						character.StopMove ();
					}
				} else {
					character.StopMove ();
				}
			}

			//NORMAL ATTACK BUTTON

			if(character.getIsJumping() && !character.getIsSpaceJumping()){
				if (Input.GetButtonUp("Normal Attack")) {
					attackController.doAttack(onAirAttack);
				}
			}else if(Mathf.Abs(Input.GetAxisRaw("Vertical"))>Mathf.Abs(Input.GetAxisRaw("Horizontal"))){
				if (Input.GetButtonUp("Normal Attack") && Input.GetAxisRaw("Vertical")>0f) {
					if(isCharacterAllowedToDoNormalAttack()){
						attackController.doAttack(upNormalAttack);
					}
				}else if(Input.GetButtonUp("Normal Attack") && Input.GetAxisRaw("Vertical")<0f){
					if(isCharacterAllowedToDoNormalAttack()){
						attackController.doAttack(downNormalAttack);
					}
				}
			}else{
				if (Input.GetButtonUp("Normal Attack") && isCharacterAllowedToDoNormalAttack()) {
					attackController.doAttack(sidesNormalAttack);
				}
			}

			//SPECIAL ATTACK BUTTON
			if (Input.GetButton("Special Attack") && isCharacterAllowedToDoSpecialAttack()) {
				if(Mathf.Abs(Input.GetAxisRaw("Vertical"))>Mathf.Abs(Input.GetAxisRaw("Horizontal"))){
					if(Input.GetAxisRaw("Vertical")>0f){
						attackController.doAttack(upSpecialAttack);
					}else if(Input.GetAxisRaw("Vertical")<0f){
						attackController.doAttack(downSpecialAttack);
					}
				}else{
					attackController.doAttack(sidesSpecialAttack);
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
					SpaceGravityBody body = GetComponent<SpaceGravityBody>();
					if(body.getIsOrbitingAroundPlanet()){
						body.setIsGettingOutOfOrbit(true);
					}
				}
			}

			//BLOCK BUTTON
			/*if(Input.GetButton("Block") && isSpaceJumpCharged){
				CancelChargingSpaceJump();
			}else if(Input.GetButton("Block") && isCharacterAllowedToDash()){
				attackController.doDash();
			}*/

			if (Input.GetButton("Block")) {
				Interactuable entity = EntityManager.getClosestInteractuable();
				if (entity != null){ entity.doInteractAction();}
				else if (isSpaceJumpCharged){
					CancelChargingSpaceJump();
				}else if(Input.GetAxis("Vertical")<0f && isCharacterAllowedToBlock()){
					attackController.doBlock();
				}else if(isCharacterAllowedToDash()){
					attackController.doDash();
				}
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
		if(GetComponent<CharacterAttackController>().isDoingAnyAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDoSpecialAttack(){
		if(character.getIsJumping()){
			return false;
		}else if(GetComponent<CharacterAttackController>().isDoingAnyAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDoNormalAttack(){
		if(GetComponent<CharacterAttackController>().isDoingAnyAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDash(){
		if(attackController.isDoingAnyAttack()){
			return false;
		}else if(attackController.isDoingDash() || attackController.isDashOnCooldown()){
			return false;
		}else if(attackController.isDoingBlock()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToBlock(){
		if (attackController.isDoingAnyAttack ()) {
			return false;
		}else if (attackController.isDoingBlock () || attackController.isBlockOnCooldown ()) {
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
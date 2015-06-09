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

	private bool isEnabled = true;
	
	void Start () {
		timeJumpPressed = 0;
		character = GetComponent<PlayerController>();
		characterGravityBody = character.GetComponent<SpaceGravityBody> ();
		attackController = GetComponent<CharacterAttackController> ();
		WeaponManager wpm = WeaponManager.Instance;
		
		
		
	}

	void Update() {

		if(!GameManager.gameState.isGameEnded && isEnabled){
			if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }

			//MOVEMENT BUTTON
			if(!attackController.isDoingDash()){
				if (Input.GetAxis ("Horizontal")!=0f) {
					if(isSpaceJumpCharged){
						character.MoveArrow(Input.GetAxisRaw ("Horizontal"),Input.GetAxis ("Vertical"));
					}else if(isCharacterAllowedToMove()){
						ResetJumping ();
						character.Move (Input.GetAxisRaw("Horizontal"));

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
					attackController.doAttack(onAirAttack,true);
				}
			}else if(Mathf.Abs(Input.GetAxisRaw("Vertical"))>Mathf.Abs(Input.GetAxisRaw("Horizontal"))){
				if (Input.GetButtonUp("Normal Attack") && Input.GetAxis("Vertical")>0.5f) {
					if(isCharacterAllowedToDoNormalAttack()){
						attackController.doAttack(upNormalAttack,true);
					}
				}else if(Input.GetButtonUp("Normal Attack") && Input.GetAxis("Vertical")<-0.5f){
					if(isCharacterAllowedToDoNormalAttack()){
						attackController.doAttack(downNormalAttack,true);
					}
				}
			}else{
				if (Input.GetButtonUp("Normal Attack") && isCharacterAllowedToDoNormalAttack()) {
					attackController.doAttack(sidesNormalAttack,true);
				}
			}

			//SPECIAL ATTACK BUTTON
			KameAttackDirectionable kameDir = attackController.getAttack(sidesSpecialAttack) as KameAttackDirectionable;
			if (isCharacterAllowedToDoSpecialAttack()) {
				if(Input.GetButtonDown("Special Attack")){
					
					if (kameDir.isAttackFinished()) {
						attackController.doAttack(sidesSpecialAttack,true);
					} 
	
					kameDir.Detonate();
					
				
				}else if(Input.GetButtonUp("Special Attack")){
					kameDir.buttonReleased();
				}else if(kameDir.canReceiveInputDirections()){
					kameDir.receiveInputDirections(Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
				}
				/*if(Mathf.Abs(Input.GetAxisRaw("Vertical"))>Mathf.Abs(Input.GetAxisRaw("Horizontal"))){
					if(Input.GetAxis("Vertical")>0.5f){
						attackController.doAttack(upSpecialAttack,true);
					}else if(Input.GetAxis("Vertical")<-0.5f){
						attackController.doAttack(downSpecialAttack,true);
					}
				}else{
					attackController.doAttack(sidesSpecialAttack,true);
				}*/

			} 

			//JUMP BUTTON


			if (Input.GetButtonUp("Jump") && isSpaceJumpCharged) {
				ResetJumping(); 
				character.SpaceJump(); 
			}

			if(Input.GetButton("Jump") && (Input.GetAxis("Vertical")<-0.5f || isSpaceJumpCharging)){
				if(isCharacterAllowedToSpaceJump()){
					timeJumpPressed += Time.deltaTime;
					if (timeJumpPressed >= startChargeSpaceJump && !isSpaceJumpCharging) {isSpaceJumpCharging = true; }
					if (timeJumpPressed >= timeIsSpaceJumpCharged && !isSpaceJumpCharged) {isSpaceJumpCharged = true; character.ChargeJump(); }
				}
			}else if(Input.GetButtonDown("Jump") && isCharacterAllowedToJump()) {
				character.Jump(); 
			}else {
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

			if (Input.GetButton("Block")) {
				Interactuable entity = EntityManager.getClosestInteractuable();
				SpaceGravityBody body = GetComponent<SpaceGravityBody>();
				if(character.getIsSpaceJumping() && body.getIsOrbitingAroundPlanet()){
					body.setIsFallingIntoPlanet(true);
				}else if (entity != null){ entity.doInteractAction();}
				else if (isSpaceJumpCharged){
					CancelChargingSpaceJump();
				}else if(Input.GetAxis("Vertical")<-0.5f && isCharacterAllowedToBlock()){
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
		}else if(!isEnabled){
			attackController.interruptActualAttacks();
			if (Input.GetButtonUp("Jump")){
				//If it's not enabled, interrupt any ongoing cinematic 
				Planet actualPlanet = GameManager.playerSpaceBody.getClosestPlanet();
				if(actualPlanet!=null && actualPlanet.isPlanetCorrupted()){
					(actualPlanet as PlanetCorrupted).getPlanetEventsManager().interrupt();
				}
			}
		}
	}

	bool isCharacterAllowedToJump(){
		if(character.getIsSpaceJumping()){
			return false;
		}else if(character.getIsJumping()){
			return false;
		}

		return true;
	}

	bool isCharacterAllowedToSpaceJump(){
		if(character.getIsSpaceJumping()){
			return false;
		}else if(character.getIsJumping()){
			return false;
		}else if(GameManager.playerSpaceBody.getClosestPlanet()!=null && GameManager.playerSpaceBody.getClosestPlanet().isPlanetCorrupted() && (GameManager.playerSpaceBody.getClosestPlanet() as PlanetCorrupted).getPlanetSpawnerManager().isActive){
			return false;
		}else if(GameManager.getIsInsidePlanet()){
			return false;
		}else if(!GameManager.gameState.canPlayerSpaceJump){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToMove(){
		if(GetComponent<CharacterAttackController>().isDoingAnyAttack() && !character.getIsJumping()){
			return false;
		}else if(attackController.isMovementLocked()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDoSpecialAttack(){
		if(character.getIsSpaceJumping()){
			return false;
		}else if(GetComponent<CharacterAttackController>().isDoingAnyAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDoNormalAttack(){
		if(character.getIsSpaceJumping()){
			return false;
		}else if(!GetComponent<CharacterAttackController>().canDoAttack()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToDash(){
		if(character.getIsSpaceJumping()){
			return false;
		}else if(!attackController.canDoAttack()){
			return false;
		}else if(attackController.isDoingDash() || attackController.isDashOnCooldown()){
			return false;
		}else if(attackController.isDoingBlock()){
			return false;
		}
		return true;
	}

	bool isCharacterAllowedToBlock(){
		if(character.getIsSpaceJumping()){
			return false;
		}else if (attackController.isDoingAnyAttack ()) {
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
		timeJumpPressed = 0f;
	}

	public void disableInputController(){
		character.StopMove ();
		GUIManager.deactivatePlayingGUI ();
		isEnabled = false;
	}

	public void enableInputController(){
		isEnabled = true;
		GUIManager.activatePlayingGUIWithFadeIn ();
	}

}
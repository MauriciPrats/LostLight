using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 5f;
	public float normalJumpForce = 15f;
	public float spaceJumpForce = 100f;
	public GameObject particleSystemJumpCharge;
	public GameObject animationBigPappada;
	public GameObject breathingBubble;
	public float timeBetweenDamageWhenNotBreathing = 0.5f;
	public int damageWhenNotBreathing = 1;
	public float minimumBreathingBubbleScale = 6f;
	public float maximumBreathingBubbleScale = 1f;


	public float timeToDieInSpace = 4f;

	public float centerToExtremesDistance = 0f;
	public float extraSafeDistanceFromEnemies = 0.3f;
	public bool isInvulnerable = false;

	public GameObject pappada;

	//SpaceJump line
	public float lineJumpDistance;
	private LineRenderer lineRenderer;
	private bool isShowingLineJump;
	private Vector3 lineJumpDirection;
	public GameObject flyingParticles;
	private ParticleSystem flyParticles;
	private bool isFinishedSpaceJump;
	public GameObject explosionOnDieInSpacePrefab;
	public GameObject weapon;



	private CharacterController characterController;
	private Animator bpAnimator;
	private SpaceGravityBody body;

	private Vector3 smoothMoveVelocity;

	private bool isSpaceJumping = false;

	private bool isOutsideAthmosphere;
	private float timeHasBeenInSpace = 0f;
	private Killable killable;
	private float timeHasNotBeenBreathing;
	private PappadaController pappadaC;



	void Awake(){
		GameManager.registerPlayer (gameObject);		
	}

	void Start () {

		body = GetComponent<SpaceGravityBody> ();
		killable = GetComponent<Killable> ();
		characterController = GetComponent<CharacterController> ();
		GameObject attack = GameObject.Find("skillAttack");
		transform.forward = new Vector3(1f,0f,0f);
		//OLD ATTACK
		//cAttackController = GetComponent<CharacterAttackController>();
		bpAnimator = animationBigPappada.GetComponent<Animator>();
		pappadaC = pappada.GetComponent<PappadaController> ();
		flyParticles = flyingParticles.GetComponent<ParticleSystem> ();

		initializeVariables ();
		
	}

	private void initializeVariables(){
		characterController.stopJumping ();
		isSpaceJumping = false;
		timeHasNotBeenBreathing = timeBetweenDamageWhenNotBreathing;
		timeHasBeenInSpace = 0f;
		centerToExtremesDistance = (animationBigPappada.GetComponent<Collider>().bounds.size.z /2f)+extraSafeDistanceFromEnemies;
		isInvulnerable = false;
		GetComponent<Rigidbody>().velocity = new Vector3 (0f, 0f, 0f);
		isFinishedSpaceJump = false;

		lineRenderer = GetComponent<LineRenderer> ();

		//Initialize the animator
		if(bpAnimator!=null){
			bpAnimator.SetBool("isJumping",false);
			bpAnimator.SetBool("isSpaceJumping",false);
			bpAnimator.SetBool("isGoingUp",false);
			bpAnimator.SetBool("isChargingSpaceJumping",false);
			bpAnimator.SetBool("isWalking",false);
		}

		FinishSpaceJump ();
	}


	void Update() {

		if(characterController.getIsJumping()){
			if (characterController.getIsGoingUp()) {
				bpAnimator.SetBool("isGoingUp",true);
			}else{
				bpAnimator.SetBool("isGoingUp",false);
			}
		}else{
			FinishJump();
			if(isSpaceJumping){
				FinishSpaceJump();
			}
		}


		if(body.getIsOutsideAthmosphere()){
			breathingBubble.SetActive(true);
			//rigidbody.velocity = rigidbody.velocity.normalized * (Constants.GRAVITY_FORCE_OF_PLANETS/1.5f);

			if(!GameManager.gameState.isGameEnded){
				if(timeHasBeenInSpace>=timeToDieInSpace){
					breathingBubble.transform.localScale = new Vector3(0f,0f,0f);
					timeHasNotBeenBreathing+=Time.deltaTime;
					if(timeHasNotBeenBreathing>=timeBetweenDamageWhenNotBreathing){
						kill ();
						flyParticles.Stop();
						GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
						GameObject newEffect = GameObject.Instantiate(explosionOnDieInSpacePrefab) as GameObject;
						newEffect.transform.position = transform.position;

						timeHasNotBeenBreathing = 0f;
					}
				}else{
					timeHasBeenInSpace += Time.deltaTime;
					float ratio = 1f - (timeHasBeenInSpace/timeToDieInSpace);
					float newScale = ((maximumBreathingBubbleScale - minimumBreathingBubbleScale) * ratio)+minimumBreathingBubbleScale;
					breathingBubble.transform.localScale = new Vector3(newScale,newScale,newScale);
				}
			}

		}else{


			if(!GameManager.gameState.isGameEnded){
				timeHasBeenInSpace = 0f;
				breathingBubble.SetActive(false);
			}
		}

		if(!isShowingLineJump){
			HideArrow();
		}else{
			ActArrow();
		}
	}

	void FinishSpaceJump(){
		bpAnimator.SetBool("isSpaceJumping",false);
		isSpaceJumping = false;
		flyParticles.Stop();
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().returnOriginalZ();
		HideArrow();
		isFinishedSpaceJump = true;
	}

	void FinishJump(){
		bpAnimator.SetBool("isJumping",false);
		characterController.stopJumping ();
	}

	public void SpaceJump() {
		//We jump with the 
		characterController.Jump (spaceJumpForce);
		GetComponent<Rigidbody> ().velocity = lineJumpDirection * spaceJumpForce;
		//If we jump into the space, stop the particle system.
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isSpaceJumping",true);
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		isSpaceJumping = true;
		HideArrow ();
		flyParticles.Clear();
		flyParticles.Play();

		isFinishedSpaceJump = false;
	}

	public void Jump() {

		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		bpAnimator.SetBool("isJumping",true);
		characterController.Jump (normalJumpForce);
	}

	public void Move() {
			bpAnimator.SetBool("isWalking",true);
			if (!body.getUsesSpaceGravity()) {
				characterController.Move(Input.GetAxisRaw("Horizontal"));
			}
	}


	public void CancelChargingSpaceJump(){
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().returnOriginalZ();
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
		bpAnimator.SetBool("isChargingSpaceJumping",false);
		HideArrow ();
	}

	public void StopMove() {
		bpAnimator.SetBool("isWalking",false);
		characterController.StopMoving ();
	}


	public void ChargeJump() {
		bpAnimator.SetBool("isChargingSpaceJumping",true);
		GameManager.mainCamera.GetComponent<CameraFollowingPlayer> ().setObjectiveZCameraOnSpaceJump ();
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Play ();
		ShowArrow ();
	}

	public void getHurt(int hitPointsToSubstract){
		if(!isInvulnerable){
			GUIManager.getHurtEffect ();
			killable.HP -= hitPointsToSubstract;
			pappadaC.newProportionOfLife(killable.proportionHP());
			if(killable.HP<=0){
				GameManager.loseGame();
			}
		}
	}

	public void kill(){
		getHurt(killable.HP);
	}

	public void MoveArrow(float horizontalMove,float verticalMove){
		Vector3 horizontalDirection = transform.forward * horizontalMove * Time.deltaTime * 20f;
		if(!characterController.getIsLookingRight()){
			horizontalDirection *= -1f;
		}
		Vector3 newPositionLine = (lineJumpDirection +(0.05f * horizontalDirection )).normalized;

		if(Vector3.Angle(transform.up,newPositionLine)<70){
			lineJumpDirection = newPositionLine;
			ActArrow();
		}
	}

	public void ActArrow(){
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition(1,transform.position + (lineJumpDirection * lineJumpDistance));
		lineRenderer.SetWidth (2f,2f);
	}

	public void ShowArrow(){
		lineJumpDirection = transform.up;
		isShowingLineJump = true;
	}

	public void HideArrow(){
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, transform.position);
		isShowingLineJump = false;

	}

	public void reset(){
		if(killable!=null){
			killable.resetHP ();
			pappadaC.newProportionOfLife(killable.proportionHP());
		}
		initializeVariables ();
	}

	public bool getIsSpaceJumping(){
		return isSpaceJumping;
	}

	public bool getIsLookingRight(){
		return characterController.getIsLookingRight();
	}

	public bool getIsJumping(){
		return characterController.getIsJumping();
	}

	public Animator getAnimator(){
		if(bpAnimator==null){
			bpAnimator = animationBigPappada.GetComponent<Animator>();
		}
		return bpAnimator;
	}

}

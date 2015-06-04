using UnityEngine;
using System.Collections;

public class IAControllerCrane : IAController {

	//IA NOT IMPLEMENTED YET

	private enum ActualBehaviour{FlyingPatroling,FlyingChasing,FlyingFalling,GroundAttack,GroundStartFlying};
	
	public AttackType meleeAttack;
	public AttackType FlyingAttack;

	public float minimumDistanceInFront = 0.4f;
	public float patrolTimeToTurn = 1.5f;
	public float maxDistancePlayer = 20f;
	public float minDistanceMeleeAttack = 0.4f;

	private OutlineChanging outlineChanger;
	private ActualBehaviour actualBehaviour;
	private float patrolTime = 0f;
	private float changeBehaviourTimer = 0f;
	
	private bool isSliding = false;
	private bool isDoingMeleeAttack = false;
	
	private float looktimer = 0f;
	private bool isFlying = false;

	public float timeBetweenGroundRaycast = 0.1f;

	public float preferredHeight = 3f;
	public float upSpeed = 0.5f;
	public LayerMask planetsMask;
	private float upDirection = 0f;
	
	protected override void initialize(){
		/*Attack meleeAttackA = attackController.getAttack(meleeAttack);
		meleeAttackA.informParent(gameObject);
		
		Attack slideMoveA = attackController.getAttack(slideMove);
		slideMoveA.informParent(gameObject);*/
		GetComponent<GravityBody> ().setHasToApplyForce (false);
		StartCoroutine ("updateFlyingHeight");
		StartCoroutine ("Patrol");
	}
	
	protected override void UpdateAI(){
		changeBehaviour();
		doActualBehaviour ();
		if(isFlying){

		}
	}

	private IEnumerator updateFlyingHeight(){
		float timer = 0f;
		while(!isDead){
			timer+=Time.deltaTime;
			if(timer>=timeBetweenGroundRaycast){
				timer = 0f;
				RaycastHit hit;
				if (Physics.Raycast(GetComponent<Rigidbody>().worldCenterOfMass,transform.up*-1f, out hit, 20f,planetsMask)){
					float distance = hit.distance;
					float missingDistance = preferredHeight - distance;
					upDirection = missingDistance/upSpeed;
					upDirection = Mathf.Clamp(upDirection,-1f,1f);
					GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity/4f;
					if(GetComponent<Rigidbody>().velocity.magnitude<0.4f){
						GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
					}
				}
			}
			transform.position = transform.position + (transform.up * upDirection * upSpeed * Time.deltaTime);
			yield return null;
		}
	}


	private void changeBehaviour(){
		iaAnimator.SetBool ("isFlying",true);
		isFlying = true;
	}
	
	private void doActualBehaviour(){

	}
	
	private void Patrol(){
		//Patrols around
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			Move(getLookingDirection()*-1f);
		}else{
			Move(getLookingDirection());
		}
	}
}

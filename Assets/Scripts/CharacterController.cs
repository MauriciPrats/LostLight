using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {
	public float moveSpeed = 5f;
	public float normalJumpForce = 10f;
	public float spaceJumpForce = 100f;
	public GameObject particleSystemJumpCharge;
	public GameObject animationBigPappada;

	private bool isAttacking = false;
	private GravityBody body;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;
	private float timeJumpPressed;
	private bool isMoving;
	private AnimationController animCont;
	private bool isLookingRight = true;

	private float timeSinceAttackStarted = 0f;
	private List<GameObject> closeEnemies = new List<GameObject>();

	bool attackEffectDone = false;
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Damageable") {
			closeEnemies.Add(col.gameObject);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Damageable") {
			closeEnemies.Remove(col.gameObject);
		}
	}
	void Start () {
		timeJumpPressed = 0;
		body = GetComponent<GravityBody> ();
		GameObject attack = GameObject.Find("skillAttack");
		animCont = GetComponent<AnimationController> ();
		transform.forward = new Vector3(1f,0f,0f);
	}


	void Update() {
		if (!isMoving) {
			if(animCont!=null){
				animCont.StopWalk();
			}
		}

		/*if (!isAttacking) {
			animCont.StopAttack();
			timeSinceAttackStarted = 0f;
		} else {
			timeSinceAttackStarted += Time.deltaTime;
			if(timeSinceAttackStarted > 0.25f && timeSinceAttackStarted<0.35f){
				if(!attackEffectDone){
					attackEffect();
				}
			}
		}*/
	}

	void FixedUpdate(){
		//Changed because the other way gave me some errors (Maurici)
		//rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		//this.rigidbody.MovePosition (newPosition);
		//rigidbody.velocity = rigidbody.velocity + movement;
		this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		
	}

	public void Attack() { 
		isAttacking = true;
		/*animCont.Attack();
		particleSystemAttack.particleSystem.Play();
		attackEffectDone = false;*/
	}

	public void SpaceJump() {
		rigidbody.AddForce (transform.up * spaceJumpForce, ForceMode.Impulse);
		//If we jump into the space, stop the particle system.
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
	}

	public void Jump() {
		rigidbody.AddForce (transform.up * normalJumpForce, ForceMode.VelocityChange);
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Stop ();
	}

	public void Move() {
		animationBigPappada.GetComponent<Animator>().SetBool("isWalking",true);
		isMoving = true;
		if (!body.getUsesSpaceGravity()) {
			float inputHorizontal = Input.GetAxisRaw ("Horizontal");

			ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
			particles.Stop ();

			//Vector3 moveDir = new Vector3 (Mathf.Abs (inputHorizontal), 0, 0).normalized;
			moveAmount = (moveSpeed * inputHorizontal) * -this.transform.right;

			//Debug.Log(moveAmount);
			//If we change the character looking direction we change the characters orientation and we invert the z angle

			/*if (inputHorizontal > 0f) {
				if (transform.localEulerAngles.y < 0f) {
					transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, -90f, transform.localEulerAngles.z);
				} else {
					transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, -90f, 360f - transform.localEulerAngles.z);
				}
			} else if (inputHorizontal < 0f) {
				if (transform.localEulerAngles.y < 0f) {
					transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 90f, 360f - transform.localEulerAngles.z);
				} else {
					transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 90f, transform.localEulerAngles.z);
				}
			}*/
			if (inputHorizontal < 0f) {
				if(!isLookingRight){
					transform.Rotate(0f,180f,0f);
					//transform.eulerAngles = new Vector3(transform.localEulerAngles.x,90f,transform.localEulerAngles.z);
					isLookingRight = true;
				}
			}else if(inputHorizontal>0f){
				if(isLookingRight){
					transform.Rotate(0f,180f,0f);
					//transform.eulerAngles = new Vector3(transform.localEulerAngles.x,-90f,transform.localEulerAngles.z);
					isLookingRight = false;
				}
			}
				/*if (transform.localEulerAngles.y < 180f) {
						transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 90f, transform.localEulerAngles.z);
				} else {
						transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 90f, 360f - transform.localEulerAngles.z);
				}
			} else if (inputHorizontal < 0f) {
				//Debug.Log(transform.localEulerAngles.y);
				if (transform.localEulerAngles.y > 180f) {
						transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 270f, transform.localEulerAngles.z);
				} else {
						transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 270f, 360f - transform.localEulerAngles.z);
				}
			}*/
		}
	}

	public void StopMove() {
		animationBigPappada.GetComponent<Animator>().SetBool("isWalking",false);

		isMoving = false;
	}

	public void StopAttack() {
		isAttacking = false;
	}

	public void ChargeJump() {
		ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
		particles.Play ();
	}
	
	void attackEffect(){
		/*attackEffectDone = true;
		foreach(GameObject o in closeEnemies){
			//o.rigidbody.AddForce((o.transform.position-transform.position).normalized*50f,ForceMode.VelocityChange);
			
			for(int i = -2;i<2;i++){
				for(int j = -2;j<2;j++){
					for(int z = -2;z<2;z++){
						GameObject newObject1 = (GameObject)Instantiate(smallParticle);
						float randx = Random.Range(-0.1f,0.1f);
						float randy = Random.Range(-0.1f,0.1f);
						float randz = Random.Range(-0.1f,0.1f);
						
						newObject1.transform.position = o.transform.position+new Vector3(i*0.2f+randx,j*0.2f+randy,z*0.2f+randz);
						newObject1.transform.parent = o.transform.parent;
						newObject1.rigidbody.useGravity=false;
						float magnitud = (newObject1.transform.position-transform.position).magnitude;
						float totalStrength = ((2f-magnitud)/2f)*200f;
						newObject1.rigidbody.AddForce(((newObject1.transform.position-transform.position)).normalized*totalStrength,ForceMode.VelocityChange);
					}
				}
			}
			Destroy(o);
			
		}
		closeEnemies = new List<GameObject>();*/
	}

}

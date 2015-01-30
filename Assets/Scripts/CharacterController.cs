using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {

	public float moveSpeed;
	public float normalJumpForce;
	public float spaceJumpForce;
	public float startChargeSpaceJump;
	public float readySpaceJump;


	private bool isAttacking = false;
	private GravityBody body;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;
	private float timeJumpPressed;
	private bool isSpaceJumping;
	private bool isSpaceJumpCharged;
	public GameObject particleSystemJumpCharge;
	public GameObject particleSystemAttack;

	public GameObject smallParticle;

	bool attackEffectDone = false;


	private float timeSinceAttackStarted = 0f;

	private List<GameObject> closeEnemies = new List<GameObject>();

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
	}

	void attackEffect(){
		attackEffectDone = true;
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
		closeEnemies = new List<GameObject>();
	}

	void Update() {
		if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }
		if(Input.GetKeyUp(KeyCode.Q)){
			if(!animation.IsPlaying("Attack")){
				isAttacking = true;
				animation.Play("Attack");
				particleSystemAttack.particleSystem.Play();
				attackEffectDone = false;
			}
		}

		
		if(!animation.IsPlaying("Attack")){
			isAttacking = false;
			timeSinceAttackStarted = 0f;
		}else{
			timeSinceAttackStarted += Time.deltaTime;
			if(timeSinceAttackStarted>0.25f && timeSinceAttackStarted<0.35f){
				if(!attackEffectDone){
					attackEffect();
				}
			}
		}
		if (body.getIsTouchingPlanet ()) {

			float inputHorizontal = Input.GetAxisRaw ("Horizontal");
			//If there is horizontal input we stop the space jumping
			if(inputHorizontal!=0f){
				isSpaceJumpCharged = false;
				isSpaceJumping = false;
				timeJumpPressed = 0;
				ParticleSystem particules = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
				particules.Stop ();
			}
			Vector3 moveDir = new Vector3 (Mathf.Abs (inputHorizontal), 0, 0).normalized;
			moveAmount = moveSpeed * moveDir;
			//If we change the character looking direction we change the characters orientation and we invert the z angle
			if (inputHorizontal > 0f) {
					if (transform.localEulerAngles.y < 90f) {
							transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 0f, transform.localEulerAngles.z);
					} else {
							transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 0f, 360f - transform.localEulerAngles.z);
					}
			} else if (inputHorizontal < 0f) {
					if (transform.localEulerAngles.y < 90f) {
							transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 180f, 360f - transform.localEulerAngles.z);
					} else {
							transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 180f, transform.localEulerAngles.z);
					}
			}

			//If there's horizontal input we play the walking animation
			if(!animation.IsPlaying("Attack")){
				if (inputHorizontal != 0f) {
						animation.Play ("Walk");
				} else {
						animation.Stop ("Walk");
				}
			}


			if (Input.GetKeyUp (KeyCode.Space)) {
					if (isSpaceJumpCharged == true) {
						rigidbody.AddForce (transform.up * spaceJumpForce, ForceMode.Impulse);
						//If we space jump we stop the particle system.
						ParticleSystem particules = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
						particules.Stop ();
						
					} else {
						rigidbody.AddForce (transform.up * normalJumpForce, ForceMode.VelocityChange);
					}
			}

			if (Input.GetKey (KeyCode.Space)) {
					timeJumpPressed += Time.deltaTime;
					if (timeJumpPressed >= startChargeSpaceJump) {
							isSpaceJumping = true;
					}
					if (timeJumpPressed >= readySpaceJump) {
							//If the space jump is charged we play the jumping particle system
							isSpaceJumpCharged = true;
							ParticleSystem particles = particleSystemJumpCharge.GetComponent<ParticleSystem> ();
							particles.Play ();
					}
			} else {
					isSpaceJumpCharged = false;
					isSpaceJumping = false;
					timeJumpPressed = 0;
			}
		}
	}

	void FixedUpdate(){
		//Changed because the other way gave me some errors (Maurici)
		//rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		Vector3 movement = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
		Vector3 newPosition = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		this.rigidbody.MovePosition (newPosition);
		//rigidbody.velocity = rigidbody.velocity + movement;
		//this.transform.position = new Vector3(this.transform.position.x + movement.x,this.transform.position.y + movement.y,this.transform.position.z);
		
	}
}

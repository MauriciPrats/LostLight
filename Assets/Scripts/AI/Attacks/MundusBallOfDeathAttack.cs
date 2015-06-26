using UnityEngine;
using System.Collections;

public class MundusBallOfDeathAttack : Attack {

	public float timeToCharge = 2f;
	public float timeToSummonBall = 0.7f;
	public float maxScaleSummoningParticles = 2f;
	public GameObject ballOfDeath;
	public GameObject particlesSpawnBallOfDeath;

	public int numBalls = 6;
	//Private variables
	private GameObject parent;
	private Animator iaAnimator;
	private OutlineChanging outlineChanger;
	private bool interrupted = false;
	private Vector3 localPositionRight, localPositionLeft;
	private GameObject backSpawningGO;
	
	public override void initialize(){
		attackType = AttackType.MundusBallOfDeath;
		particlesSpawnBallOfDeath.SetActive (false);
	}
	
	public override void enemyCollisionEnter(GameObject enemy,Vector3 point){
		//GameManager.player.GetComponent<PlayerController> ().getHurt (damage);
	}
	
	public override void startAttack(){
		if(isFinished){
			StartCoroutine("doAttack");
			isFinished = false;
			interrupted = false;
		}
	}

	IEnumerator doAttack(){

		float timer = 0f;

		for (int i =0; i<numBalls; i++) {
			Vector3 ballPosition;
			float direction = 0f;
			if(i%2 == 0){
				ballPosition = transform.position;
				direction = 1f;
			}else{
				ballPosition = backSpawningGO.transform.position;
				direction = -1f;
			}

			iaAnimator.SetBool ("isChargingBallAttack", true);
			particlesSpawnBallOfDeath.transform.localScale = new Vector3 (0f, 0f, 0f);
			particlesSpawnBallOfDeath.SetActive (true);
			particlesSpawnBallOfDeath.transform.position = ballPosition;

			while (timer<timeToCharge) {
				timer+=Time.deltaTime;
				float ratio = timer/timeToCharge;
				float proportionScale = ratio * maxScaleSummoningParticles;
				particlesSpawnBallOfDeath.transform.localScale = new Vector3(proportionScale,proportionScale,proportionScale);
				outlineChanger.setOutlineColor(Color.Lerp(Color.black,Color.red,ratio));
				yield return null;
			}

			//We create a new instance because there can be multiple ocurrences at the same time
			GameObject ballOfDeathInstance = GameObject.Instantiate (ballOfDeath) as GameObject;
			ballOfDeathInstance.transform.position = ballPosition;
			ballOfDeathInstance.GetComponent<BallOfDeath> ().setDirectionAndDamage (direction, damage, particlesSpawnBallOfDeath);
			particlesSpawnBallOfDeath.SetActive (false);
			iaAnimator.SetBool ("isChargingBallAttack", false);

			timer = 0f;
			while (timer<timeToSummonBall) {
				timer += Time.deltaTime;
				yield return null;
			}
		}
		outlineChanger.setOutlineColor(Color.black);
		isFinished = true;
		interrupted = false;
	}
	
	public override void interruptAttack(){
		interrupted = true;
		outlineChanger.setOutlineColor(Color.black);
	}
	
	public override void informParent(GameObject parentObject){
		transform.parent = parentObject.transform;
		transform.rotation = parentObject.transform.rotation;
		transform.position = parentObject.GetComponent<Rigidbody>().worldCenterOfMass + (parentObject.transform.forward*parentObject.GetComponent<WalkOnMultiplePaths>().centerToExtremesDistance*4f);
		backSpawningGO = new GameObject ();
		backSpawningGO.transform.parent = parentObject.transform;
		backSpawningGO.name = "Back Spawning GO";
		backSpawningGO.transform.position = parentObject.GetComponent<Rigidbody>().worldCenterOfMass -(parentObject.transform.forward * parentObject.GetComponent<WalkOnMultiplePaths> ().centerToExtremesDistance * 4f);
		parent = parentObject;
		iaAnimator = parent.GetComponent<IAController> ().getIAAnimator ();
		outlineChanger = parent.GetComponent<OutlineChanging> ();
	}
}

using UnityEngine;
using System.Collections;

public class IAControllerMundus : IAController {

	public GameObject rightClaw;
	public GameObject leftClaw;
	public GameObject protectionSphere;
	public int phaseHitsReceivedThreshold = 10;

	public GameObject surroundingBall1,surroundingBall2,surroundingBall3;

	public AttackType baseAttack;
	public AttackType ballOfDeathAttack;
	public AttackType spawningAttack;
	public AttackType fisureAttack;

	private float patrolTime = 0f;
	private float patrolTimeToTurn = 2f;
	private float direction = 1f;
	private bool isSpawning = false;
	private int currentSurroundingBall = 0;
	private float timerAfterAttack = 0f;
	private bool isAttacking = false;
	private MundusPlanetEventsManager eventsManager;


	private int faseSteps = 1;
	private int fase = 1;
	private bool hasSpawnedThisPhaseStep = true;
	private int damageReceived = 0;

	private bool protecting = false;
	protected override void initialize(){
		Attack meleeAttackA = attackController.getAttack(baseAttack);
		meleeAttackA.informParent(gameObject);

		Attack ballOfDeathA = attackController.getAttack(ballOfDeathAttack);
		ballOfDeathA.informParent(gameObject);

		Attack spawningA = attackController.getAttack(spawningAttack);
		spawningA.informParent(gameObject);

		Attack fisureA = attackController.getAttack(fisureAttack);
		fisureA.informParent(gameObject);
	}

	public GameObject useNewSurroundingBall(){
		currentSurroundingBall++;
		if(currentSurroundingBall==1){
			return surroundingBall1;
		}else if(currentSurroundingBall==2){
			return surroundingBall2;
		}else if(currentSurroundingBall==3){
			return surroundingBall3;
		}
		return null;
	}

	public void setProtecting(){
		isSpawning = true;
		protecting = true;
		iaAnimator.SetBool ("isProtecting", true);
		protectionSphere.SetActive (true);

	}

	public void informFisure(GameObject fisure){
		eventsManager.informFisure (fisure);
	}
	public void informPlanetEventManager(MundusPlanetEventsManager events){
		eventsManager = events;
	}

	public void stopProtecting(){
		isSpawning = false;
		protecting = false;
		iaAnimator.SetBool ("isProtecting", false);
		protectionSphere.SetActive (false);
	}

	private void setAttacksTier(int tier){
		attackController.getAttack (baseAttack).setTier (tier);
		attackController.getAttack (ballOfDeathAttack).setTier (tier);
		attackController.getAttack (spawningAttack).setTier (tier);
		attackController.getAttack (fisureAttack).setTier (tier);
	}

	protected override void UpdateAI(){
		if(fase == 1){
			timerAfterAttack += Time.deltaTime;
			if (!attackController.isDoingAnyAttack () && !isAttacking && timerAfterAttack>1f && canSeePlayer() && getPlayerDistance()<2f){
				characterController.StopMoving();
				if(damageReceived>phaseHitsReceivedThreshold){
					attackController.doAttack(fisureAttack,false);
					faseSteps++;
					setAttacksTier(faseSteps);
					damageReceived = 0;
					hasSpawnedThisPhaseStep = false;
				}else if(!hasSpawnedThisPhaseStep){
					attackController.doAttack(spawningAttack,false);
					hasSpawnedThisPhaseStep = true;
				}else{
					float random = Random.value;
					if(random<0.6f){
						attackController.doAttack(baseAttack,false);
					}else{
						attackController.doAttack(ballOfDeathAttack,false);
					}
				}
				isAttacking = true;
			}else if(!attackController.isDoingAnyAttack () && isAttacking){
				if(faseSteps>=4){
					fase = 2;
					eventsManager.informEventActivated(CutsceneIdentifyier.LastPlanetMundusSecondPhase);
				}
				isAttacking = false;
				timerAfterAttack = 0f;
			}else if(!attackController.isDoingAnyAttack () && getPlayerDistance()>1f){
				characterController.Move(getPlayerDirection());

			}else{
				StopMoving();
			}
		}else if(fase == 2){

		}
	}

	protected override bool virtualGetHurt(){
		if(!attackController.isDoingAnyAttack() || isSpawning){
			if(!protecting){
				StartCoroutine(Protect());
			}
			return false;
		}else{
			damageReceived++;
			return true;
		}
	}

	protected override bool virtualHitStone(){
		if(!attackController.isDoingAnyAttack()  || isSpawning){
			return false;
		}else{
			return true;
		}
	}

	private IEnumerator Protect(){
		protecting = true;
		iaAnimator.SetBool ("isProtecting", true);
		protectionSphere.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		protectionSphere.SetActive (false);
		iaAnimator.SetBool ("isProtecting", false);
		protecting = false;
	}

	private void Patrol(){
		//Patrols around
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			direction*=-1f;
		}
		Move (direction);
	}

	public void setPhase(int phase){
		this.fase = phase;
	}


}

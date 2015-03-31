using UnityEngine;
using System.Collections;

public class CharacterAttackControllerOLD : MonoBehaviour {

	public GameObject AttackCube;
	GameObject cubeInstance = null;
	GameObject stick = null;
	
	public GameObject animationBigPappada;
	private Animator bpAnimator;
	public bool isAttacking = false;

	//Fases del combo, separado en Startup, Attack, Link y Cooldown
	//Startup es que ya se inicio el ataque pero el hitbox no esta aun activo
	//Attack empieza cuando el hitbox esta activo
	//Link es la ventana donde se puede iniciar un nuevo ataque en combo
	//Cooldown es un periodo inactivo luego del ataque, donde no se puede atacar y se pierde el combo
	//Iddle es el estado inicial, desde el cual se puede iniciar el combo
	public enum ComboSteps {Iddle, Startup, Attack, Link, Cooldown};
	public ComboSteps comboStat;
	public int comboCount;

	//tiempos de cada estado
	float[] sTime = {0.5f,0.2f,0.8f};
	float[] aTime = {0.01f,0.01f,0.01f};
	float[] lTime = {0.2f,0.7f,0.0f};
	float cTime = 0.1f;
	//colores de cada estado
	Color[] colorList = {Color.green , Color.yellow, Color.red}; 
	


	// Use this for initialization
	void Start () {
		bpAnimator = animationBigPappada.GetComponent<Animator>();
		setAnimTimes();
	
		cubeInstance = (GameObject)Instantiate(AttackCube);
		cubeInstance.transform.parent = gameObject.transform;
		
		cubeInstance.GetComponent<ParticleSystem>().Stop();
		cubeInstance.GetComponent<MeshRenderer>().enabled = false;
		cubeInstance.GetComponent<Collider>().enabled = false;
		
		comboStat = ComboSteps.Iddle;
		comboCount = 0;
		stick = GameObject.FindGameObjectWithTag("Weapon");
		if (stick == null) print ("Failed to find stick");
		DisableHitbox();
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*Busca en UnityEditorInternal la duracion de las animaciones
	Esto va a fallar si se ejecuta fuera del entorno de desarrollo, ya que el namespace
	Solo existe aqui
	*/
	private void setAnimTimes() {
		
		UnityEditor.Animations.AnimatorController ac = bpAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
		//UnityEditor.Animations.AnimatorStateMachine sm = ac.GetLayer(0).stateMachine.GetStateMachine(0); //Doesn't work on UNITY 5

		//Doesn't work on UNITY 5
		/*for(int i = 0; i < sm.stateCount; i++) {
			UnityEditor.Animations.AnimatorState state = sm.GetState(i);
			AnimationClip clip = state.GetMotion() as AnimationClip;
			float l = clip.length;
			float s = state.speed;
			float animationTime = l/s;
			
			if (state.name == "Attack1") {
				sTime[0] = animationTime * 0.55f;
				lTime[0] = animationTime * 0.45f;
			}
			if (state.name == "Attack2") {
				sTime[1] = animationTime * 0.5f;
				lTime[1] = animationTime * 0.5f;
			}
			if (state.name == "Attack2") {
				sTime[2] = animationTime * 0.5f;
				lTime[2] = animationTime * 0.5f;
			}
		}*/
	}
	

	private void SetUpCube(bool isLookingRight, Transform character) 
	{
		int dir = -1;
		if (!isLookingRight) {
			dir = 1;
		}
		
		Vector3 posCube = transform.TransformDirection (dir * character.right*0.8f);
		
		posCube += character.position;
		Quaternion rotCube = character.rotation;
		
		cubeInstance.transform.position = posCube;
		cubeInstance.transform.rotation = rotCube;
	}

	
	public void ChargeAttack(bool isLookingRight, Transform character) {
	
		switch (comboStat) {
		case ComboSteps.Iddle:
			//Iniciar el ataque
			SetUpCube(isLookingRight,character);
			comboStat = ComboSteps.Startup;
			bpAnimator.SetTrigger("Attack");
			StartCoroutine("DoAttack");
			break;
		case ComboSteps.Startup:
		case ComboSteps.Attack:
		case ComboSteps.Cooldown:
			//Ignorar el input
			break;
		case ComboSteps.Link:
			//Iniciar el segundo ataque
			++comboCount;
			if (comboCount >= 3) break; //solo 3 ataques		
			bpAnimator.SetInteger("ComboCount",comboCount);
			StopCoroutine("DoAttack");
			DisableHitbox();
			SetUpCube(isLookingRight,character);
			comboStat = ComboSteps.Startup;
			StartCoroutine("DoAttack");
			break;	
		default:
			break;
		}
	}
	
	
	IEnumerator DoAttack() 
	{	isAttacking = true;
		//TODO: Validar que solo ataca desde iddle, walking o desde el combo en si
		if (comboStat != ComboSteps.Startup) yield break; //termina la corutina temprano
		
		
		yield return new WaitForSeconds(sTime[comboCount]);//termina tiempo de espera, empieza de ataque
		comboStat = ComboSteps.Attack;
		
		EnableHitbox(colorList[comboCount]*0.5f);
		
		yield return new WaitForSeconds(aTime[comboCount]);//termina de ataque, empieza combo
		comboStat = ComboSteps.Link;
		
						
		yield return new WaitForSeconds(lTime[comboCount]);//termina combo, empieza espera
		comboStat = ComboSteps.Cooldown;
		
		DisableHitbox();	
		
		yield return new WaitForSeconds(cTime);
		comboStat = ComboSteps.Iddle;
		comboCount = 0;
		bpAnimator.SetInteger("ComboCount",comboCount);
		isAttacking = false;
	}
	
	private void EnableHitbox(Color particleColor)
	{
		//cubeInstance.renderer.enabled = true;
		stick.GetComponent<ParticleSystem>().startColor = particleColor;
		stick.GetComponent<ParticleSystem>().enableEmission = true;
		cubeInstance.GetComponent<Collider>().enabled = true;
	}
	
	private void DisableHitbox()
	{
		//cubeInstance.renderer.enabled = false;
		stick.GetComponent<ParticleSystem>().enableEmission = false;
		
		cubeInstance.GetComponent<Collider>().enabled = false;	
	}
	
}

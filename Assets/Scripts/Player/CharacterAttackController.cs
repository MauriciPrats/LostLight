using UnityEngine;
using System.Collections;

public class CharacterAttackController : MonoBehaviour {

	public GameObject AttackCube;
	GameObject cubeInstance = null;
	public float atkTime = 0.6f;
	public bool isAttacking = false;

	// Use this for initialization
	void Start () {
		cubeInstance = (GameObject)Instantiate(AttackCube);
		cubeInstance.transform.parent = gameObject.transform;
		cubeInstance.GetComponent<ParticleSystem>().Stop();
		cubeInstance.GetComponent<MeshRenderer>().enabled = false;
		cubeInstance.GetComponent<Collider>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChargeAttack(bool isLookingRight, Transform character) {
		if (!isAttacking) {
			isAttacking = true;
			
			int dir = -1;
			if (!isLookingRight) {
				dir = 1;
			}
			
			Vector3 posCube = transform.TransformDirection (dir * character.right*0.8f);
			
			posCube += character.position;
			Quaternion rotCube = character.rotation;

			cubeInstance.transform.position = posCube;
			cubeInstance.transform.rotation = rotCube;

			StartCoroutine("ChargeUp");
			
		}
	
	}
	
	enum ChargeLevels {c0,c1,c2,cmax};
	ChargeLevels chLevel;
	public float stageCharge = 0.5f;
	IEnumerator ChargeUp() {
	
		ParticleSystem ps = cubeInstance.particleSystem;
		chLevel= ChargeLevels.c0;
		ps.Play();
		
		yield return new WaitForSeconds(stageCharge);
		
		chLevel= ChargeLevels.c1;
		
		ps.startColor = Color.yellow;
		ps.gravityModifier = -0.1f;		
		ps.startSpeed = 0.15f;
		ps.startLifetime = 0.9f;
		ps.emissionRate = 60;
		
		yield return new WaitForSeconds(stageCharge);
		
		//chLevel= ChargeLevels.c2;
		
		
	/*	ps.startColor = Color.magenta;
		ps.gravityModifier = -0.15f;
		ps.startSpeed = 0.2f;
		ps.startLifetime = 0.8f;
		ps.emissionRate = 80;*/
		
		yield return new WaitForSeconds(stageCharge);
		yield return new WaitForSeconds(stageCharge);
		
		chLevel= ChargeLevels.cmax;
		
		ps.startColor = Color.red;
		ps.gravityModifier = -0.2f;
		ps.startSpeed = 0.25f;
		ps.startLifetime = 0.7f;
		ps.emissionRate = 120;
	}

	public void Attack() { 
		if (isAttacking && cubeInstance != null) {
			StopCoroutine("ChargeUp");
			
			switch (chLevel) {
				case ChargeLevels.c0:
				cubeInstance.renderer.material.color = Color.green;
				break;
				case ChargeLevels.c1:
				cubeInstance.renderer.material.color = Color.yellow;
				break;
				case ChargeLevels.c2:
				cubeInstance.renderer.material.color = Color.magenta;
				break;
				case ChargeLevels.cmax:
				cubeInstance.renderer.material.color = Color.red;
				break;
				default:
				break;
			}
		
		}
		
		
					
		StartCoroutine("AttackFrames");
			

		
	}
	
	IEnumerator AttackFrames() {
		cubeInstance.GetComponent<MeshRenderer>().enabled = true;
		cubeInstance.GetComponent<Collider>().enabled = true;
		
		
		yield return new WaitForSeconds(atkTime);
		//Destroy(cubeInstance);
		//cubeInstance = null;
		cubeInstance.renderer.enabled = false;
		cubeInstance.collider.enabled = false;
		cubeInstance.particleSystem.Stop ();
		isAttacking = false;
		yield return null;
		
	}
	
}

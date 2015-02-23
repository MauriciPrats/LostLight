using UnityEngine;
using System.Collections;

public class CharacterAttackController : MonoBehaviour {

	public GameObject AttackCube;
	GameObject cubeInstance = null;
	public float atkTime = 0.6f;
	public bool isAttacking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Attack(bool isLookingRight, Transform character) { 
		
		if (!isAttacking && cubeInstance == null) {
			isAttacking = true;
			
			int dir = -1;
			if (!isLookingRight) {
				dir = 1;
			}
			
			Vector3 posCube = transform.TransformDirection (dir * character.right*0.8f);
			
			posCube += character.position;
			Quaternion rotCube = character.rotation;
			
			cubeInstance = Instantiate(AttackCube,posCube,rotCube) as GameObject;
			cubeInstance.transform.parent = gameObject.transform;
			StartCoroutine("AttackFrames");
			
		}
		
	}
	
	IEnumerator AttackFrames() {
		yield return new WaitForSeconds(atkTime);
		Destroy(cubeInstance);
		cubeInstance = null;
		isAttacking = false;
		yield return null;
		
	}
	
}

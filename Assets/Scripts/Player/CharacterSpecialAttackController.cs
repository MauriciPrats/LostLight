using UnityEngine;
using System.Collections;

public class CharacterSpecialAttackController : MonoBehaviour {

	public GameObject specialAttackUpGO;
	public GameObject specialAttackSidesGO;
	public GameObject specialAttackDownGO;

	private SpecialAttack specialAttackUp;
	private SpecialAttack specialAttackSides;
	private SpecialAttack specialAttackDown;
	
	// Use this for initialization
	void Start () {


		specialAttackUp = (GameObject.Instantiate(specialAttackUpGO) as GameObject).GetComponent<SpecialAttack> ();
		specialAttackSides = (GameObject.Instantiate(specialAttackSidesGO) as GameObject).GetComponent<SpecialAttack> ();
		specialAttackDown = (GameObject.Instantiate(specialAttackDownGO) as GameObject).GetComponent<SpecialAttack> ();
	}

	public bool isDoingAnySpecialAttack(){
		if(!specialAttackUp.isSpecialAttackFinished()){
			return true;
		}else if(!specialAttackSides.isSpecialAttackFinished()){
			return true;
		}else if(!specialAttackDown.isSpecialAttackFinished()){
			return true;
		}
		return false;
	}

	public void doUpSpecialAttack(){
	
		specialAttackUp.startAttack ();
	}

	public void doSidesSpecialAttack(){
		specialAttackSides.startAttack ();
	}

	public void doDownSpecialAttack(){
		specialAttackDown.startAttack ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

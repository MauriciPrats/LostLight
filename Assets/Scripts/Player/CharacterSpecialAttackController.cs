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
		if(specialAttackUp.canPaySpecialAttackCost()){
			GameManager.lightGemEnergyManager.substractPoints(specialAttackUp.lightPointsCost);
			specialAttackUp.startAttack ();
		}
	}

	public void doSidesSpecialAttack(){
		if(specialAttackSides.canPaySpecialAttackCost()){
			GameManager.lightGemEnergyManager.substractPoints(specialAttackUp.lightPointsCost);
			specialAttackSides.startAttack ();
		}
	}

	public void doDownSpecialAttack(){
		if(specialAttackDown.canPaySpecialAttackCost()){
			GameManager.lightGemEnergyManager.substractPoints(specialAttackDown.lightPointsCost);
			specialAttackDown.startAttack ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class EnemyAttackManager : MonoBehaviour {

	public int attackBudgetMax = 100;
	public int budgetRegenerationPerSecond = 30;

	public float budgetRegenerationTimer = 0f;

	private int attackBudget = 0;

	void Awake(){
		GameManager.registerEnemyAttackManager (gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}

	public bool askForNewAttack(int value){
		if(attackBudget>value){
			attackBudget -= value;
			return true;
		}else{
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		budgetRegenerationTimer += Time.deltaTime;
		if(budgetRegenerationTimer>=1f){
			if((attackBudget+budgetRegenerationPerSecond)>=attackBudgetMax){
				attackBudget = attackBudgetMax;
			}else{
				attackBudget+=budgetRegenerationPerSecond;
			}
		}

	
	}


}

using UnityEngine;
using System.Collections;

public class IAControllerMundus : IAController {


	private float patrolTime = 0f;
	private float patrolTimeToTurn = 2f;

	protected override void initialize(){


	}
	
	protected override void UpdateAI(){
		//Patrol ();
	}

	private void Patrol(){
		//Patrols around
		patrolTime += Time.deltaTime;
		if(patrolTime>=patrolTimeToTurn){
			patrolTime = 0f;
			Move(getLookingDirection()*-1f);
		}else{
			Move(getLookingDirection());
		}
	}


}

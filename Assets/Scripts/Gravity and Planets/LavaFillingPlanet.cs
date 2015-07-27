﻿using UnityEngine;
using System.Collections;

public class LavaFillingPlanet : MonoBehaviour {

	public GameObject corePlanet;

	public float timeToGrow = 2f;

	public Vector3 maxScale;

	public float timeToStayMaxGrown = 2f;
	public float timeToStayMinGrown = 5f;

	private float timer = 0f;
	private enum Phase{Growing,WaitingGrown,Ungrowing,WaitingUngrown};
	private Phase actualPhase = Phase.Growing;

	void Update(){
		timer += Time.deltaTime;
		if(actualPhase.Equals(Phase.Growing)){
			float ratio = timer/timeToGrow;
			Vector3 scale = maxScale * ratio;
			corePlanet.transform.localScale = scale;
			if(timer>timeToGrow){
				nextPhase();
			}

		}else if(actualPhase.Equals(Phase.WaitingGrown)){
			float ratio = timer/timeToStayMaxGrown;
			//Vector3 scale = maxScale * (ratio);
			if(timer>timeToStayMaxGrown){
				nextPhase();
			}

		}else if(actualPhase.Equals(Phase.Ungrowing)){
			float ratio = timer/timeToGrow;
			Vector3 scale = maxScale * (1f - ratio); 
			corePlanet.transform.localScale = scale;
			if(timer>timeToGrow){
				nextPhase();
			}

		}else if(actualPhase.Equals(Phase.WaitingUngrown)){
			float ratio = timer/timeToStayMinGrown;
			//Vector3 scale = maxScale * ratio;
			if(timer>timeToStayMinGrown){
				nextPhase();
			}

		}
	}

	private void nextPhase(){
		if(actualPhase.Equals(Phase.Growing)){
			actualPhase = Phase.WaitingGrown;
		}else if(actualPhase.Equals(Phase.WaitingGrown)){
			actualPhase = Phase.Ungrowing;
		}else if(actualPhase.Equals(Phase.Ungrowing)){
			actualPhase = Phase.WaitingUngrown;
		}else if(actualPhase.Equals(Phase.WaitingUngrown)){
			actualPhase = Phase.Growing;
		}
		timer = 0f;
	}

}

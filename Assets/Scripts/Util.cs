using UnityEngine;
using System.Collections;

public static class Util{

	//Clockwise
	public static float getPlanetaryAngleFromAToB(GameObject a,GameObject b){
		float angle = Mathf.DeltaAngle(Mathf.Atan2(b.transform.up.y, b.transform.up.x) * Mathf.Rad2Deg,Mathf.Atan2(a.transform.up.y, a.transform.up.x) * Mathf.Rad2Deg);
		return angle;
	}

	public static bool isARightToB(GameObject a,GameObject b){
		if(getPlanetaryAngleFromAToB(a,b)>0f){
			return false;
		}else{
			return true;
		}
	}


}
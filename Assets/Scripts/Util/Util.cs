using UnityEngine;
using System.Collections;

public static class Util{

	//Clockwise
	public static float getPlanetaryAngleFromAToB(GameObject a,GameObject b){
		if(a!=null && b!=null){
			return getAngleFromVectorAToB (a.transform.up, b.transform.up);
		}else{
			return float.PositiveInfinity;
		}
	}

	public static float getAngleFromVectorAToB(Vector3 a,Vector3 b){
		float angle = Mathf.DeltaAngle(Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg,Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg);
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
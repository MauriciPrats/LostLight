﻿using UnityEngine;
using System.Collections;

public static class Util{

	private static float originalTimeScale;
	private static float originalFixedDeltaTime;
	private static bool originalTimeScaleInitialized = false;


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

	public static void changeTime(float newTimeProportion){
		if(!originalTimeScaleInitialized){
			originalTimeScale = Time.timeScale;
			originalFixedDeltaTime = Time.fixedDeltaTime;
			originalTimeScaleInitialized = true;
		}
		Time.timeScale = newTimeProportion;
		Time.fixedDeltaTime = newTimeProportion * originalFixedDeltaTime;
	}

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class WeaponController : MonoBehaviour {
	public enum Position { REAR, MIDDLE, FRONT };

	private WeaponPart currentRearPart;
	private WeaponPart currentMiddlePart;
	private WeaponPart currentFrontPart;

	private Dictionary<string, WeaponPart> rearWeaponList;
	private Dictionary<string, WeaponPart> frontWeaponList;
	private Dictionary<string, WeaponPart> middleWeaponList;
	
	private string name;
	private float damage;
	private float distance;
	private bool areaDamage;
	private float speed;

	// Use this for initialization
	void Start () {
		rearWeaponList = new Dictionary<string, WeaponPart> ();
		frontWeaponList = new Dictionary<string, WeaponPart>();
		middleWeaponList = new Dictionary<string, WeaponPart>();
	}

	public void SetMiddlePart ( string name) {
		currentMiddlePart = middleWeaponList [name];
	}

	public void SetRearPart ( string name) {
		currentRearPart = middleWeaponList [name];
	}

	public void SetFrontPart ( string name) {
		currentFrontPart = middleWeaponList [name];
	}

	public WeaponPart GetRearPart() {
		return currentRearPart;
	}

	public WeaponPart GetFrontPart() {
		return currentFrontPart;
	}

	public WeaponPart GetMiddlePart() {
		return currentMiddlePart;
	}

	public void AddPart(WeaponPart item, Position sideToPlace) {
		if (sideToPlace ==  Position.FRONT ){
			frontWeaponList.Add (item.GetName(),item);
		}
		if (sideToPlace ==  Position.REAR ){
			frontWeaponList.Add (item.GetName(),item);
		}
		if (sideToPlace ==  Position.MIDDLE ){
			middleWeaponList.Add (item.GetName(),item);
		}
	}

	public float GetCurrentDamage() {
		return GetFrontPart ().GetDamage () + GetRearPart ().GetDamage () + GetMiddlePart ().GetDamage ();
	}

	public float GetCurrentDistance() {
		return GetFrontPart ().GetDistance () + GetRearPart ().GetDistance () + GetMiddlePart ().GetDistance ();
	}

	public bool IsAreaDamage() {
		return GetFrontPart ().IsAreaDamage() || GetRearPart ().IsAreaDamage() || GetMiddlePart ().IsAreaDamage ();
	}

	public float GetCurrentSpeed() {
		return GetFrontPart ().GetSpeed () + GetRearPart ().GetSpeed () + GetMiddlePart ().GetSpeed ();
	}

	public string GetCurrentName() {
		return GetFrontPart ().GetName () + GetMiddlePart ().GetName () + GetRearPart ().GetName ();
	}
}

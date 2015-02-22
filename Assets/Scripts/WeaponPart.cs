using UnityEngine;
using System.Collections;

public class WeaponPart : Item {

	public enum Position { REAR, MIDDLE, FRONT };

	private string name;
	private float damage;
	private float distance;
	private bool areaDamage;
	private Position side;
	private float speed;

	public float GetDamage() {
		return damage;
	}
	public float GetDistance() {
		return distance;
	}
	public bool IsAreaDamage() {
		return areaDamage;
	}
	public float GetSpeed() {
		return speed;
	}
	public string GetName() {
		return name;
	}

}

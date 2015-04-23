using UnityEngine;
using System.Collections;

public class GodModeCamara : MonoBehaviour {
	
	public bool areYouGod = false;
	public float wheelspeed = 5f;
	public float godspeed = 0.5f;
	// Use this for initialization
	void Start () {
		
	}

	private ElementType transformElement(ElementType et){
		if (et.Equals (ElementType.Earth)) {
			return ElementType.Fire;
		} else if (et.Equals (ElementType.Fire)) {
			return ElementType.Ice;
		} else if (et.Equals (ElementType.Ice)) {
			return ElementType.Lightning;
		} else if (et.Equals (ElementType.Lightning)) {
			return ElementType.None;
		} else if (et.Equals (ElementType.None)) {
			return ElementType.Earth;
		}
		return ElementType.None;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Period)) {
			if (areYouGod) {
				areYouGod = false;
				this.gameObject.GetComponent<CameraFollowingPlayer> ().enabled = true;
				GameManager.player.GetComponent<InputController> ().enabled = true;
			} else {
				areYouGod = true;
				this.gameObject.GetComponent<CameraFollowingPlayer> ().enabled = false;
				GameManager.player.GetComponent<InputController> ().enabled = false;
			}
		}else if (Input.GetKeyDown (KeyCode.F1)) {
			Attack attack = GameManager.player.GetComponent<CharacterAttackController>().getAttack(AttackType.Kame);
			attack.elementAttack = transformElement(attack.elementAttack);
			attack.cost = 0;
		}else if (Input.GetKeyDown (KeyCode.F2)) {
			Attack attack = GameManager.player.GetComponent<CharacterAttackController>().getAttack(AttackType.Shockwave);
			attack.elementAttack = transformElement(attack.elementAttack);
			attack.cost = 0;
		}else if (Input.GetKeyDown (KeyCode.F3)) {
			Attack attack = GameManager.player.GetComponent<CharacterAttackController>().getAttack(AttackType.Missiles);
			attack.elementAttack = transformElement(attack.elementAttack);
			attack.cost = 0;
		}
	
		if (areYouGod) {
			float xmove = Input.GetAxis ("Horizontal");
			float ymove = Input.GetAxis ("Vertical");
			float zmove = Input.GetAxis ("Mouse ScrollWheel");
			//TODO: Restore the correct position in Z
			this.transform.position += new Vector3 (xmove * godspeed, ymove * godspeed, zmove * wheelspeed);
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				transform.position = new Vector3(0.0f,0.0f,transform.position.z);
			}
			
			
			
		}
		
		
	}
}

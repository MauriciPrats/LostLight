using UnityEngine;
using System.Collections;

public class WeaponSmithInteraction : Interactuable {
	private bool weaponMenu;
	public override void doInteractAction(){
		//TODO: abrir menu de la herrera. 
		// Make a background box
		weaponMenu = true;
	}
	void OnGUI() {
		if (weaponMenu) {
			GUI.Box (new Rect (10, 10, 180, 90), "Weaponsmith Ana Marrana");
		
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if (GUI.Button (new Rect (20, 40, 160, 20), "Craft weapons")) {
			}
			
			// Make the second button.
			if (GUI.Button (new Rect (20, 70, 160, 20), "Exit")) {
				weaponMenu = false;
			}
		}
	}
}

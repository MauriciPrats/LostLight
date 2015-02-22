using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private bool weaponSmithRootMenu;
	private bool weaponCraftRootMenu;
	private bool inventory;

	private static GUIManager singleton;
	
	public static GUIManager Instance {
		get{ return singleton ?? (singleton = new GameObject("GUIManager").AddComponent<GUIManager>());}
	}	

	public void OpenWeaponsmithRootMenu () {
		weaponSmithRootMenu = true;
	}
	
	public void OpenInventory () {
		inventory = true;
	}
		
	public void CloseAllWindows() {
		weaponSmithRootMenu = false;
		weaponCraftRootMenu = false;
		inventory = false;
	}

	void OnGUI() {
		if (weaponSmithRootMenu) {
			GUI.Box (new Rect (10, 10, 180, 90), "Weaponsmith Ana Marrana");
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if (GUI.Button (new Rect (20, 40, 160, 20), "Craft weapons")) {
				weaponSmithRootMenu = false;
				weaponCraftRootMenu = true;
			}
			
			// Make the second button.
			if (GUI.Button (new Rect (20, 70, 160, 20), "Exit")) {
				weaponSmithRootMenu = false;
				weaponCraftRootMenu = false;
			}
		}
		
		if (weaponCraftRootMenu) {
			GUI.Box (new Rect (10, 10, 500, 500), "Marraning..");
			//TODO: Montar la interfaz grafica. 

			//Open Inventory to see what weapon parts we can put into our weapon.
			OpenInventory();
			// Make the second button.
			if (GUI.Button (new Rect (20, 480, 450, 20), "Back")) {
				weaponSmithRootMenu = true;
				weaponCraftRootMenu = false;
			}
		}

		if (inventory) {
			GUI.BeginGroup(new Rect(0, 550, 500, 150));
				GUI.Box (new Rect (0,0,550,500), "Your bag");
				GUI.Button (new Rect (5, 100, 45, 45), "");
				GUI.Button (new Rect (50, 100, 45, 45),"");
				GUI.Button (new Rect (95, 100, 45, 45),"");
			GUI.EndGroup();
		}
	}
}

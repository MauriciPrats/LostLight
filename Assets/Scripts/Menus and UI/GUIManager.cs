using UnityEngine;
using System.Collections;

public enum Menu{None,MainMenu,ControlsMenu};

public class GUIManager : MonoBehaviour {

	public static FadeManager fadeManager;
	/*private bool weaponSmithRootMenu;
	private bool weaponCraftRootMenu;
	private bool itemCraftRootMenu;
	private bool inventory;
	private float scrwidth, scrheight;*/


	private static GameObject mainMenuO;
	private static GameObject controlsO;

	private static Menu nextMenu;

	private static GUIManager singleton;

	//Private 
	private static void deactivateMenus(){
		if(mainMenuO!=null){
			mainMenuO.SetActive(false);
		}
		if(controlsO!=null){
			controlsO.SetActive(false);
		}
	}
	
	private static void changeMenuAndFadeIn(){
		deactivateMenus ();
		activateMenu (nextMenu);
		fadeManager.fadeIn();
	}

	//Public
	public static GUIManager Instance {
		get{ return singleton ?? (singleton = new GameObject("GUIManager").AddComponent<GUIManager>());}
	}	

	public static void registerMainMenu(GameObject mainMenuGO){
		if(mainMenuGO!=null && mainMenuO == null){
			mainMenuO = GameObject.Instantiate (mainMenuGO) as GameObject;
			mainMenuO.SetActive (false);
		}
	}

	public static void registerControlsMenu(GameObject controlsGO){
		if(controlsGO!=null && controlsO == null){
			controlsO = GameObject.Instantiate (controlsGO) as GameObject;
			controlsO.SetActive (false);
		}
	}

	public static void registerFadeManager(GameObject fadeManagerGO){
		fadeManager = fadeManagerGO.GetComponent<FadeManager> ();
	}

	public static void activateMenu(Menu newMenu){
		deactivateMenus ();
		if (newMenu.Equals (Menu.MainMenu)) {
			mainMenuO.SetActive(true);
		}else if(newMenu.Equals(Menu.ControlsMenu)){
			controlsO.SetActive(true);
		}
	}
	
	public static void fadeOutChangeMenuFadeIn(Menu newMenu){

		nextMenu = newMenu;
		fadeManager.fadeOut(changeMenuAndFadeIn);
		
	}



	/*
	public void OpenWeaponsmithRootMenu () {
		weaponSmithRootMenu = true;
	}
	public void CloseWeaponSmithRootMenu() {
		weaponSmithRootMenu = false;
	}
	
	public void OpenInventory () {
		inventory = true;
	}
	public void OpenItemCraftingMenu() {
		itemCraftRootMenu = true;
	}

	public void CloseItemCraftingMenu() {
		itemCraftRootMenu = false;
	}

	public void CloseInventory() {
		inventory = false;
	}

	public void OpenCraftMenu() {
		weaponCraftRootMenu = true;
	}
	public void CloseCraftMenu() {
		weaponCraftRootMenu = false;
	}
	public void CloseAllWindows() {
		CloseWeaponSmithRootMenu ();
		CloseCraftMenu ();
		CloseItemCraftingMenu ();
		CloseInventory ();
	}

	void OnGUI() {
		scrwidth = Screen.width;
		scrheight = Screen.height;
		if (weaponSmithRootMenu) {
			float menuwidth = Constants.WEAPONSMITH_ROOT_MENU_WIDTH;
			float menuheight = Constants.WEAPONSMITH_ROOT_MENU_HEIGHT;
			float buttonwidth = Constants.WEAPONSMITH_ROOT_MENU_BUTTONS_WIDTH;
			float buttonheight = Constants.WEAPONSMITH_ROOT_MENU_BUTTONS_HEIGHT;
			float vinterspace = Constants.WEAPONSMITH_ROOT_MENU_BUTTONS_V_OFFSET;
			GUI.BeginGroup(new Rect (scrwidth/2-menuwidth/2, scrheight/2-menuheight/2,menuwidth, menuheight));
				GUI.Box (new Rect (0,0,menuwidth,menuheight), Constants.WEAPONSMITH_ROOT_MENU_TITLE);
				if (GUI.Button (new Rect (menuwidth/2 - buttonwidth /2 , menuheight/2 - buttonheight/2, buttonwidth, buttonheight), Constants.WEAPONSMITH_ROOT_MENU_ENTER)) {
					CloseWeaponSmithRootMenu();
					OpenCraftMenu();
				}
				if (GUI.Button (new Rect (menuwidth/2-buttonwidth /2,menuheight/2-buttonheight/2+(buttonheight+vinterspace)*1,buttonwidth,buttonheight), "Craft items")) {
					OpenItemCraftingMenu();
				}
		    	if (GUI.Button (new Rect (menuwidth/2-buttonwidth /2,menuheight/2-buttonheight/2+(buttonheight+vinterspace)*2,buttonwidth,buttonheight), Constants.WEAPONSMITH_ROOT_MENU_EXIT)) {
					CloseAllWindows();
				}
			GUI.EndGroup();
		}
		
		if (weaponCraftRootMenu) {
			GUI.Box (new Rect (10, 10, 500, 500), "Customizing weapon..");
			//TODO: Montar la interfaz grafica. 
			if (GUI.Button (new Rect (20, 450, 450, 20), "Craft")){
				
			}
			if (GUI.Button (new Rect (20, 480, 450, 20), "Back")) {
				OpenWeaponsmithRootMenu();
				CloseCraftMenu();
				CloseInventory();
			}
		}

		if (itemCraftRootMenu) {
			GUI.BeginGroup(new Rect(10, 10, 500, 150));
				GUI.Box (new Rect (10, 10, 500, 500), "Crafting items...");
				OpenInventory ();
				if (GUI.Button (new Rect (20, 450, 450, 20), "Craft")){
					
				}
				if (GUI.Button (new Rect (20, 480, 450, 20), "Back")) {
					OpenWeaponsmithRootMenu();
					CloseCraftMenu();
					CloseInventory();
				}
			GUI.EndGroup();
		}

		if (inventory) {
			GUI.BeginGroup(new Rect(10, 550, 500, 150));
				GUI.Box (new Rect (0,0,550,500), "Inventory");
				GUI.Button (new Rect (5, 100, 45, 45), "");
				GUI.Button (new Rect (50, 100, 45, 45),"");
				GUI.Button (new Rect (95, 100, 45, 45),"");
			GUI.EndGroup();
		}
	}*/
}

using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public enum Menu{None,
				MainMenu,
				ControlsMenu,
				CreditsMenu,
				YouWonMenu,
				YouLostMenu,
				StartingSplashScreen,
				CraftingMenu,
				InteractuablePopup,
				OnPauseMenu
			    };

public class GUIManager : MonoBehaviour {

	public static FadeManager fadeManager;
	/*private bool weaponSmithRootMenu;
	private bool weaponCraftRootMenu;
	private bool itemCraftRootMenu;
	private bool inventory;
	private float scrwidth, scrheight;*/


	private static GameObject mainMenuO;
	private static GameObject controlsO;
	private static GameObject creditsO;
	private static GameObject youWonO;
	private static GameObject youLostO;
	private static GameObject startingSplashScreenO;
	private static GameObject craftingMenuO;
	private static GameObject interactuablePopupO;
	private static GameObject onPauseMenuO;
	private static GameObject playingGUIO;

	private static Menu nextMenu;

	private static GUIManager singleton;

	private static GameObject actualMenu;
	private static Menu actualMenuActivated = Menu.None;

	private static Action actionToDo;

	//Private 
	private static void deactivateMenus(){
		if(mainMenuO!=null){
			mainMenuO.SetActive(false);
		}
		if(controlsO!=null){
			controlsO.SetActive(false);
		}
		if(creditsO!=null){
			creditsO.SetActive(false);
		}
		if(youLostO!=null){
			youLostO.SetActive(false);
		}
		if(youWonO!=null){
			youWonO.SetActive(false);
		}
		if(startingSplashScreenO!=null){
			startingSplashScreenO.SetActive(false);
		}
		if(craftingMenuO!=null){
			craftingMenuO.SetActive(false);
		}
		if(interactuablePopupO!=null){
			interactuablePopupO.SetActive(false);
		}
		if(onPauseMenuO!=null){
			onPauseMenuO.SetActive(false);
		}
	}
	
	private static void changeMenuAndFadeIn(){
		deactivateMenus ();
		activateMenu (nextMenu);
		fadeManager.fadeIn(getMenu(nextMenu));
	}

	private static void changeMenuAndFadeInWithAction(){
		deactivateMenus ();
		activateMenu (nextMenu);
		fadeManager.fadeInWithAction(actionToDo,getMenu(nextMenu));
	}

	private static GameObject getMenu(Menu menu){
		if (menu.Equals (Menu.MainMenu)) {
			return mainMenuO;
		}else if(menu.Equals(Menu.ControlsMenu)){
			return controlsO;
		}else if(menu.Equals(Menu.CreditsMenu)){
			return creditsO;
		}else if(menu.Equals(Menu.YouWonMenu)){
			return youWonO;
		}else if(menu.Equals(Menu.YouLostMenu)){
			return youLostO;
		}else if(menu.Equals(Menu.StartingSplashScreen)){
			return startingSplashScreenO;
		}else if(menu.Equals(Menu.CraftingMenu)){
			return craftingMenuO;
		}else if(menu.Equals(Menu.InteractuablePopup)){
			return interactuablePopupO;
		}else if(menu.Equals(Menu.OnPauseMenu)){
			return onPauseMenuO;
		}
		return null;
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

	
	public static void registerCreditsMenu(GameObject creditsGO){
		if(creditsGO!=null && creditsO == null){
			creditsO = GameObject.Instantiate (creditsGO) as GameObject;
			creditsO.SetActive (false);
		}
	}

	public static void registerYouLostMenu(GameObject youLostGO){
		if(youLostGO!=null && youLostO == null){
			youLostO = GameObject.Instantiate (youLostGO) as GameObject;
			youLostO.SetActive (false);
		}
	}

	public static void registerYouWonMenu(GameObject youWonGO){
		if(youWonGO!=null && youWonO == null){
			youWonO = GameObject.Instantiate (youWonGO) as GameObject;
			youWonO.SetActive (false);
		}
	}

	public static void registerStartingSplashScreen(GameObject startingSplashScreenGO){
		if(startingSplashScreenGO!=null && startingSplashScreenO == null){
			startingSplashScreenO = GameObject.Instantiate (startingSplashScreenGO) as GameObject;
			startingSplashScreenO.SetActive (false);
		}
	}

	public static void registerCraftingMenu(GameObject craftingMenuGO){
		if(craftingMenuGO!=null && craftingMenuO == null){
			craftingMenuO = GameObject.Instantiate (craftingMenuGO) as GameObject;
			craftingMenuO.SetActive (false);
		}
	}

	public static void registerInteractuablePopup(GameObject interactuablePopupGO){
		if(interactuablePopupGO!=null && interactuablePopupO == null){
			interactuablePopupO = GameObject.Instantiate (interactuablePopupGO) as GameObject;
			interactuablePopupO.SetActive (false);
		}
	}

	public static void registerOnPauseMenu(GameObject onPauseMenuGO){
		if(onPauseMenuGO!=null && onPauseMenuO == null){
			onPauseMenuO = GameObject.Instantiate (onPauseMenuGO) as GameObject;
			onPauseMenuO.SetActive (false);
		}
	}

	public static void registerFadeManager(GameObject fadeManagerGO){
		fadeManager = fadeManagerGO.GetComponent<FadeManager> ();
	}

	public static void registerPlayingGUI(GameObject playingGUIGO){
		if(playingGUIGO!=null && playingGUIO == null){
			playingGUIO = GameObject.Instantiate (playingGUIGO) as GameObject;
			playingGUIO.SetActive (false);
		}
	}

	public static void activatePlayingGUI(){
		playingGUIO.SetActive (true);
	}

	/*private static void deactivatePlayingGUIAfterFade(){
		playingGUIO.SetActive (false);
	}*/
	public static void deactivatePlayingGUI(){
		playingGUIO.SetActive (false);
		//fadeManager.fadeOut(deactivatePlayingGUIAfterFade,playingGUIO);
	}



	public static void activateMenu(Menu newMenu){
		deactivateMenus ();
		GameObject newMenuGO = getMenu (newMenu);

		if(newMenuGO!=null){
			getMenu (newMenu).SetActive (true);
		}
		actualMenu = getMenu (newMenu);
		if(actualMenu!=null){
			MenuManager man = actualMenu.GetComponent<MenuManager>();
			if(man!=null){
				EventSystem.current.SetSelectedGameObject (man.firstButtonFocus);
			}
		}

		actualMenuActivated = newMenu;
	}



	public static void openCraftingMenu (){
		if(actualMenuActivated.Equals(Menu.None)){
			activateMenu(Menu.CraftingMenu);
			GameManager.pauseGame();
		}
	}

	public static void closeCraftingMenu(){
		activateMenu (Menu.None);
		GameManager.unPauseGame ();
		//fadeOut (GameManager.unPauseGame);
	}

	public static void activatePauseMenu(){
		activateMenu (Menu.OnPauseMenu);
	}

	public static void deactivatePauseMenu(){
		activateMenu (Menu.None);
	}

	public static void activateInteractuablePopup(){
		if(actualMenuActivated.Equals(Menu.None)){
			interactuablePopupO.SetActive (true);
		}
	}

	public static void deactivateInteractuablePopup(){
		interactuablePopupO.SetActive (false);
	}


	
	public static void fadeOutChangeMenuFadeIn(Menu newMenu){
		nextMenu = newMenu;
		fadeManager.fadeOut(changeMenuAndFadeIn,actualMenu);
	}

	public static void fadeOutChangeMenuFadeInWithAction(Action action,Menu newMenu){
		nextMenu = newMenu;
		fadeManager.fadeOut(changeMenuAndFadeInWithAction,actualMenu);
		actionToDo = action;
	}

	public static void fadeInWithAction(Action action,Menu newMenu){
		activateMenu (newMenu);
		fadeManager.fadeInWithAction(action, getMenu(newMenu));
	}

	public static void fadeIn(Menu newMenu){
		activateMenu (newMenu);
		fadeManager.fadeIn (getMenu (newMenu));

	}

	public static void fadeOut(Action action,Menu newMenu){
		fadeManager.fadeOut (action, getMenu (newMenu));
		actualMenuActivated = Menu.None;
	}

	public static void fadeOut(Action action){
		fadeManager.fadeOut (action, actualMenu);
		actualMenuActivated = Menu.None;
	}

	public static void fadeAllOut(Action action){
		fadeManager.fadeAllOut (action);
		actualMenuActivated = Menu.None;
	}

	public static void fadeAllIn(){
		fadeManager.fadeAllIn ();
	}

	public static void getHurtEffect(){
		fadeManager.getHurtEffect ();
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

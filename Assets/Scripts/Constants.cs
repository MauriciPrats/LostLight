using UnityEngine;
using System.Collections;

public static class Constants{

	//PLANET CONSTANTS
	public static float GRAVITY_DISTANCE_FROM_PLANET_FLOOR = 30f;
	public static float GRAVITY_FORCE_OF_PLANETS = 20f;
	public static float DRAG_ON_TOUCH_PLANETS = 15f;

	//CAMERA CONSTANTS
	public static float CAMERA_ANGLE_FOLLOWING_SPEED = 5f;
	public static float GRAVITY_FORCE_OF_ATHMOSPHERE = 2f;
	public static float FRONT_PATH_Z_OFFSET = 0.30f;
	public static float BACK_PATH_Z_OFFSET = 0.01f;

	//GUI NAMING CONSTANTS (BUTTON STRINGS).
	public static string WEAPONSMITH_ROOT_MENU_TITLE = "Weaponsmith Ana Marrana";
	public static string WEAPONSMITH_ROOT_MENU_ENTER = "Craft weapons";
	public static string WEAPONSMITH_ROOT_MENU_EXIT  = "Exit";

	//GUI POSITIONING CONSTANTS (POSITION AND SIZE OPTIONS). IN PIXELS.
	public static float WEAPONSMITH_ROOT_MENU_WIDTH = 180;
	public static float WEAPONSMITH_ROOT_MENU_HEIGHT = 90;
	public static float WEAPONSMITH_ROOT_MENU_BUTTONS_WIDTH = 150;
	public static float WEAPONSMITH_ROOT_MENU_BUTTONS_HEIGHT= 20;
	public static float WEAPONSMITH_ROOT_MENU_BUTTONS_V_OFFSET = 5;


}

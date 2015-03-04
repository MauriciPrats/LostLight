using UnityEngine;
using System.Collections;

public static class Constants{

	//PLANET CONSTANTS
	public static float GRAVITY_DISTANCE_FROM_PLANET_FLOOR = 8f;
	public static float GRAVITY_FORCE_OF_PLANETS = 20f;
	public static float DRAG_ON_TOUCH_PLANETS = 0f;


	//CAMERA CONSTANTS
	public static float CAMERA_ANGLE_FOLLOWING_SPEED = 5f;
	public static float GRAVITY_DRAG_OF_ATHMOSPHERE = 2f;
	public static float GRAVITY_FORCE_OF_ATHMOSPHERE = 2f;
	public static float FRONT_PATH_Z_OFFSET = -0.35f;
	public static float BACK_PATH_Z_OFFSET = -0.05f;



	//QUICK TIME EVENT CONSTANTS
	public static float MINIMUM_PLANET_DISTANCE_FOR_QUICK_TIME_EVENT = 15f;
	public static float TIME_BETWEEN_QUICK_TIME_EVENTS = 3f;
	public static float RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS = 1f; //The final time will be TIME_BETWEEN_QUICK_TIME_EVENTS +- RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS
	public static float ASTEROID_SPEED = 5f;
	public static float DISTANCE_SHOW_POPUP = 3f;

	//MINIMAPA
	public static float MINIMAP_DISTANCE = -30f;

	//GUI NAMING CONSTANTS (BUTTON STRINGS).
	public static string WEAPONSMITH_ROOT_MENU_TITLE = "Weaponsmith Ana Marrana";
	public static string WEAPONSMITH_ROOT_MENU_ENTER = "Craft weapons";
	public static string WEAPONSMITH_ROOT_MENU_EXIT  = "Exit";

	//GUI POSITIONING CONSTANTS (POSITION AND SIZE OPTIONS). IN PIXELS.
	public static float WEAPONSMITH_ROOT_MENU_WIDTH = 180;
	public static float WEAPONSMITH_ROOT_MENU_HEIGHT = 130;
	public static float WEAPONSMITH_ROOT_MENU_BUTTONS_WIDTH = 150;
	public static float WEAPONSMITH_ROOT_MENU_BUTTONS_HEIGHT= 20;
	public static float WEAPONSMITH_ROOT_MENU_BUTTONS_V_OFFSET = 5;


	public static float FADE_SPEED = 0.5f;

	public static float SPAWNING_MINIMUM_DISTANCE_OF_PLAYER = 5f;
	public static float SPAWNING_MAXIMUM_DISTANCE_OF_PLAYER = 25f;

	public static float TIME_BETWEEN_CHECK_PLAYER_DISTANCE_FOR_DESPAWN = 2f;



}

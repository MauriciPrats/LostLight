using UnityEngine;
using System;
using System.Collections;

public class Constants  : MonoBehaviour{


	private static Constants singleton;

	public static Constants Instance {
		get{ return singleton ?? (singleton = new GameObject("Constants").AddComponent<Constants>());}
	}

	//PLANET CONSTANTS
	public float GRAVITY_DISTANCE_FROM_PLANET_FLOOR = 8f;
	public float GRAVITY_FORCE_OF_PLANETS = 20f;
	public float DRAG_ON_TOUCH_PLANETS = 3f;
	public float GRAVITY_MULTIPLYIER_ON_SPACE_JUMPS = 3f;


	//CAMERA CONSTANTS
	public float CAMERA_ANGLE_FOLLOWING_SPEED = 5f;
	public float GRAVITY_DRAG_OF_ATHMOSPHERE = 3f;
	public float GRAVITY_FORCE_OF_ATHMOSPHERE = 2f;
	public float FRONT_PATH_Z_OFFSET = -0.35f;
	public float BACK_PATH_Z_OFFSET = -0.05f;



	//QUICK TIME EVENT CONSTANTS
	public float MINIMUM_PLANET_DISTANCE_FOR_QUICK_TIME_EVENT = 15f;
	public float TIME_BETWEEN_QUICK_TIME_EVENTS = 3f;
	public float RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS = 1f; //The final time will be TIME_BETWEEN_QUICK_TIME_EVENTS +- RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS
	public float ASTEROID_SPEED = 5f;
	public float DISTANCE_SHOW_POPUP = 3f;

	//MINIMAPA
	public float MINIMAP_DISTANCE = -110f;

	//GUI NAMING CONSTANTS (BUTTON STRINGS).
	public string WEAPONSMITH_ROOT_MENU_TITLE = "Weaponsmith Ana Marrana";
	public string WEAPONSMITH_ROOT_MENU_ENTER = "Craft weapons";
	public string WEAPONSMITH_ROOT_MENU_EXIT  = "Exit";

	//GUI POSITIONING CONSTANTS (POSITION AND SIZE OPTIONS). IN PIXELS.
	public float WEAPONSMITH_ROOT_MENU_WIDTH = 180;
	public float WEAPONSMITH_ROOT_MENU_HEIGHT = 130;
	public float WEAPONSMITH_ROOT_MENU_BUTTONS_WIDTH = 150;
	public float WEAPONSMITH_ROOT_MENU_BUTTONS_HEIGHT= 20;
	public float WEAPONSMITH_ROOT_MENU_BUTTONS_V_OFFSET = 5;


	public float FADE_SPEED = 0.5f;

	//ENEMY SPAWNING
	public float SPAWNING_MINIMUM_DISTANCE_OF_PLAYER = 5f;
	public float SPAWNING_MAXIMUM_DISTANCE_OF_PLAYER = 25f;

	public float TIME_BETWEEN_CHECK_PLAYER_DISTANCE_FOR_DESPAWN = 2f;



}

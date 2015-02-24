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

	public static float FRONT_PATH_Z_OFFSET = 0.30f;

	public static float BACK_PATH_Z_OFFSET = 0.01f;


	//QUICK TIME EVENT CONSTANTS
	public static float MINIMUM_PLANET_DISTANCE_FOR_QUICK_TIME_EVENT = 15f;

	public static float TIME_BETWEEN_QUICK_TIME_EVENTS = 3f;

	//The final time will be TIME_BETWEEN_QUICK_TIME_EVENTS +- RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS
	public static float RANDOMNESS_TIME_BETWEEN_QUICK_TIME_EVENTS = 1f;

	public static float ASTEROID_SPEED = 5f;

	public static float DISTANCE_SHOW_POPUP = 3f;

	//MINIMAPA
	public static float MINIMAP_DISTANCE = -30f;

}

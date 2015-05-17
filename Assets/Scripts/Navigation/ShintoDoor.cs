using UnityEngine;
using System.Collections;

public class ShintoDoor : MonoBehaviour {

	public GameObject KanjiLevel1;
	public GameObject KanjiLevel2;
	public GameObject KanjiLevel3;

	public float timeStayLookingShinto = 2f;

	public GameObject cameraShintoPosition;
	//public GameObject particleEffectOnKanjiActivate;

	int actualLevelChanging = 0;
	private bool isMovingCameraToShinto = false;
	private bool isMovingCameraToPlayer = false;
	private bool isActivatingKanji = false;
	private bool isWaitingLookingAtKanji = false;

	private float timeWaiting = 0f;
	Vector3 cameraOriginalRotation;
	Vector3 cameraOriginalPosition;

	Vector3 centerOfPlanet;


	// Use this for initialization
	void Start () {
		KanjiLevel1.SetActive (false);
		KanjiLevel2.SetActive (false);
		KanjiLevel3.SetActive (false);

		centerOfPlanet = transform.parent.position;

	}
	
	// Update is called once per frame
	void Update () {
		if(isActivatingKanji){
			if(isMovingCameraToShinto){
				moveCameraToShinto();
			}else if(isMovingCameraToPlayer){
				moveCameraToPlayer();
			}else if(isWaitingLookingAtKanji){
				waitLookingAtKanji();
			}else{
				activateKanji();
			}
		}
	}
	private float getAngleToObjective(Vector3 objective){
		Vector3 cameraCenter = getCameraCenter ();
		Vector3 objectiveCenter = objective - centerOfPlanet;
		objectiveCenter = new Vector3(objectiveCenter.x,objectiveCenter.y,0f);
		float angle = Vector3.Angle(cameraCenter,objectiveCenter);//Util.getAngleFromVectorAToB(cameraCenter,objectiveCenter);
		
		if(Util.getAngleFromVectorAToB(cameraCenter,objectiveCenter)>0f){
			angle *=-1f;
		}
		return angle;
	}

	private Vector3 getCameraCenter(){
		Vector3 cameraCenter = GameManager.mainCamera.transform.position - centerOfPlanet;
		cameraCenter = new Vector3(cameraCenter.x,cameraCenter.y,0f);
		return cameraCenter;
	}

	private Vector3 getResultingPositionToObjective(float angle,Vector3 objective){
		Vector3 cameraDestination = OrbiteAroundPoint(GameManager.mainCamera.transform.position,centerOfPlanet,Quaternion.Euler(0,0,angle * Time.fixedDeltaTime));
		float newZ = GameManager.mainCamera.transform.position.z + (Time.fixedDeltaTime * (objective.z - GameManager.mainCamera.transform.position.z));
		cameraDestination = new Vector3(cameraDestination.x,cameraDestination.y,newZ);

		Vector3 cameraCenter = getCameraCenter ();
		Vector3 objectiveCenter = objective - centerOfPlanet;
		objectiveCenter = new Vector3(objectiveCenter.x,objectiveCenter.y,0f);
		float magnitudeDifference = objectiveCenter.magnitude - cameraCenter.magnitude;

		cameraDestination += (cameraCenter.normalized * (Time.fixedDeltaTime * magnitudeDifference));

		return cameraDestination;

	}

	private void moveCameraToShinto(){

		float angle = getAngleToObjective (cameraShintoPosition.transform.position);
		//if(angle<0f){angle = 360f + angle;}

		if(Mathf.Abs(angle)<1f){
			isMovingCameraToShinto = false;
		}else{

			GameManager.mainCamera.transform.position = getResultingPositionToObjective(angle,cameraShintoPosition.transform.position);
			GameManager.mainCamera.transform.up = (getCameraCenter ()).normalized;
		}
	}


	private Vector3 OrbiteAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * ( point - pivot) + pivot;
	}

	private void moveCameraToPlayer(){
		float angle = getAngleToObjective (cameraOriginalPosition);

		if(Mathf.Abs(angle)<1f){
			GameManager.mainCamera.transform.position = cameraOriginalPosition;
			isMovingCameraToPlayer = false;
			isActivatingKanji = false;
			GameManager.gameState.isCameraLockedToPlayer = true;
			Time.timeScale = 1f;

			//Temporal game ending
			if(KanjiLevel3.activeSelf){
				GameManager.winGame();
			}
		}else{
			GameManager.mainCamera.transform.position = getResultingPositionToObjective(angle,cameraOriginalPosition);
			GameManager.mainCamera.transform.up = (getCameraCenter ()).normalized;
		}
		//On finish, give the timeScale back the original value and change the gamestate
	}

	public void activateKanjis(){
		KanjiLevel1.SetActive (true);
		KanjiLevel2.SetActive (true);
		KanjiLevel3.SetActive (true);
	}

	private void activateKanji(){
		if(actualLevelChanging == 1){
			KanjiLevel1.SetActive (true);
		}else if(actualLevelChanging == 2){
			KanjiLevel2.SetActive (true);
		}else if(actualLevelChanging == 3){
			KanjiLevel3.SetActive (true);
		}
		isWaitingLookingAtKanji = true;
	}


	private void waitLookingAtKanji(){
		timeWaiting += Time.fixedDeltaTime; //50fps
		if(timeWaiting>=timeStayLookingShinto){
			timeWaiting = 0f;
			isMovingCameraToPlayer = true;
			isWaitingLookingAtKanji = false;
		}
	}

	public void activateKanjiLevel(int level){
		actualLevelChanging = level;
		isMovingCameraToShinto = true;
		isActivatingKanji = true;
		GameManager.gameState.isCameraLockedToPlayer = false;
		Time.timeScale = 0f;

		cameraOriginalRotation = GameManager.mainCamera.transform.eulerAngles;
		cameraOriginalPosition = GameManager.mainCamera.transform.position;
	}
}

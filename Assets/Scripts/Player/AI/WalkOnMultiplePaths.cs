using UnityEngine;
using System.Collections;

public class WalkOnMultiplePaths : MonoBehaviour {

	public LayerMask layersToFindCollisionEnemies;
	public bool isFrontPath = true;
	public float maximumDistanceSee = 30f;

	private float lastTimeCheckedOtherPaths ;
	private float cooldownRaycastingCheckOtherPaths = 1f;

	private bool isChangingPath = false;
	private float changingTimer = 0f;


	// Use this for initialization
	void Start () {
		changePath ();
		lastTimeCheckedOtherPaths = Random.value * cooldownRaycastingCheckOtherPaths;
	}
	
	// Update is called once per frame
	void Update () {
		if(isChangingPath){
			changePath();
		}
		if(!isChangingPath){
			if(hasToChangePath()){
				isFrontPath = !isFrontPath;
				isChangingPath = true;
			}
		}
	}

	public void changePath(){
		changingTimer = 1f;
		isChangingPath = false;
		if (isFrontPath) {transform.position = new Vector3(transform.position.x,transform.position.y,Constants.FRONT_PATH_Z_OFFSET);}
		else{transform.position = new Vector3(transform.position.x,transform.position.y,Constants.BACK_PATH_Z_OFFSET);}
		/*changingTimer += Time.deltaTime;
		if(changingTimer>=1f){changingTimer = 1f; isChangingPath = false;}
		Vector3 frontPath = new Vector3(transform.localPosition.x,transform.localPosition.y,Constants.FRONT_PATH_Z_OFFSET);
		Vector3 backPath = new Vector3(transform.localPosition.x,transform.localPosition.y,Constants.BACK_PATH_Z_OFFSET);
		//Debug.Log("changiing")
		if(isFrontPath){
			transform.localPosition = Vector3.Lerp(backPath,frontPath,changingTimer);
		}else{
			transform.localPosition = Vector3.Lerp(frontPath,backPath,changingTimer);
		}*/
		
	}

	public int ammountOfEnemiesInFront(bool isFrontPath,int jumps){
		jumps += 1;
		//TODO: find out how many enemies are in this path between him and the player
		if(jumps<5){
			RaycastHit hit;
			Vector3 position;
			if(isFrontPath){position = new Vector3(transform.position.x,transform.position.y,Constants.FRONT_PATH_Z_OFFSET);}
			else{position = new Vector3(transform.position.x,transform.position.y,Constants.BACK_PATH_Z_OFFSET);}
			if (Physics.Raycast(position,transform.forward, out hit, maximumDistanceSee,layersToFindCollisionEnemies))
			{
				Collider target = hit.collider; // What did I hit?
				//Debug.Log(target.name);
				if(target.tag != "Enemy"){ return 0;}
				else if(target.tag == "Enemy"){ 
					WalkOnMultiplePaths controller = target.gameObject.GetComponent<WalkOnMultiplePaths>();
					return controller.ammountOfEnemiesInFront(isFrontPath,jumps)+1;
				}
			}
		}
		return 0;
	}

	private bool hasToChangePath(){
		lastTimeCheckedOtherPaths += Time.deltaTime;
		if(lastTimeCheckedOtherPaths>cooldownRaycastingCheckOtherPaths){
			lastTimeCheckedOtherPaths = Random.value*0.1f;
			int frontEnemies = ammountOfEnemiesInFront (true,0);
			int backEnemies = ammountOfEnemiesInFront (false,0);
			
			//Debug.Log("F: "+frontEnemies+" B: "+backEnemies);
			
			if(isFrontPath && backEnemies<frontEnemies){
				return true;
			}else if(!isFrontPath && backEnemies>frontEnemies){
				return true;
			}
			return false;
		}else{
			return false;
		}
	}

	
	public bool IsFrontPath(){
		return isFrontPath;
	}

	public bool getIsChangingPath(){
		return isChangingPath;
	}
}

using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress = 0f;
	private bool goingForward = true;
	private bool invertDirection = false;

	private void Update () {
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				progress = 1f;
			}
		}else{
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = 0f;
			}
		}

		Vector3 position = spline.GetPoint(spline.getTByLength(progress));
		transform.localPosition = position;
		if (lookForward) {
			transform.LookAt(position + spline.GetDirection(progress));
		}
	}

	public bool isFinished(){
		if(!invertDirection){
			if(progress >= 1f){
				return true;
			}
		}else if(invertDirection){
			if(progress <= 0f){
				return true;
			}
		}
		return false;
	}

	public void restart(){
		progress = 0f;
		goingForward = true;
	}

	public void setInvert(){
		invertDirection = true;
		progress = 1f;
		goingForward = false;
	}

	public void setNonInvert(){
		invertDirection = false;
		progress = 0f;
		goingForward = true;
	}
}
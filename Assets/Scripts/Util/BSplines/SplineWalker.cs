using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress = 0f;
	private bool goingForward = true;

	private void Update () {
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
					progress -= 1f;
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
		if (lookForward) {
			transform.LookAt(position + spline.GetDirection(progress));
		}
	}

	public bool isFinished(){
		if(mode == SplineWalkerMode.Once){
			if(progress >= 1f){
				return true;
			}
		}
		return false;
	}

	public void restart(){
		progress = 0f;
		goingForward = true;
	}
}
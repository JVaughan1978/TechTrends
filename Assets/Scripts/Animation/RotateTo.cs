using UnityEngine;
using System.Collections;

public class RotateTo : MonoBehaviour {

	public float endRotation = 270.0f;
	public float rotDuration = 2.0f;

	//not implemented yet
	//public delegate float EaseFunction();
	//private EaseFunction ease;
	//public EaseType easeType = EaseType.Linear;

	IEnumerator DoRotate(float start, float end, float duration){
		float time = 0f;
		while (time < duration) {
			time += Time.deltaTime;
			if( time > duration) { time = duration; }

			float newZRotation = Easing.CubicEaseOut(time, start, end, duration);
			Vector3 newRotation = new Vector3(this.transform.localEulerAngles.x,
			                                  this.transform.localEulerAngles.y,
			                                  newZRotation);
			transform.rotation = Quaternion.Euler(newRotation);
			yield return null;
		}
		Debug.Log ("DoRotate finished");
	}

	void Start(){
		Invoke ("DoRotate", 3.0f);
	}

	void DoRotate() {
		StartCoroutine(DoRotate(this.transform.localEulerAngles.z, endRotation, rotDuration));
	}

	void Update () {
		//intentionally left blank!
	}
}

using UnityEngine;
using System.Collections;

public class RotateTo : MonoBehaviour {

	public float endRotation = 270.0f;
	public float rotDuration = 2.0f;
    private float v1 = 0f;    

	//not implemented yet
	//public delegate float EaseFunction();
	//private EaseFunction ease;
	//public EaseType easeType = EaseType.Linear;

	IEnumerator DoRotate(float duration){
		float time = 0f;
        float start = 0f;
        float end = 1f;

		while (time < duration) {
			time += Time.deltaTime;
			if( time > duration) { time = duration; }

			float lerpTime = Easing.CubicEaseOut(time, start, end, duration);
            float angle = Mathf.Lerp(v1,endRotation, lerpTime);
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
			yield return null;
		}
		Debug.Log ("DoRotate finished");
	}

	void Start(){
		Invoke ("DoRotate", 3.0f);
	}

	void DoRotate() {
		StartCoroutine(DoRotate(rotDuration));
	}

	void Update () {
		//intentionally left blank!
	}
}

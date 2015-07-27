using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathAnimation : MonoBehaviour {

	public List<Vector3> controlPoints = new List<Vector3>();
	private AnimationCurve _xCurve = new AnimationCurve();
	private AnimationCurve _yCurve = new AnimationCurve();
	private AnimationCurve _zCurve = new AnimationCurve();

	private bool _paused = false;
	private bool _playing = false;
	public float speed = 1.0f; //time to execute in seconds
	private float _clipPosition = 0.0f; //the control for curve evaluation

	//loop functionality? reverse? pingpong? eh, later maybe
	//implementing support for more easeTypes later.
	//ease type would control the animation over the length of the curve
	/*
	public EaseType ease = EaseType.Linear;
	//ease delegate funtion;
	public delegate float EaseFunction();
	private EaseFunction ease;	
	*/

	//keytype: linear smooth stepped

	public void SetControlPoints(List<Vector3> points){
		controlPoints.Clear();

		foreach(Vector3 cp in points){
			controlPoints.Add(cp);		
		}

		SetAnimationCurves();
	}

	private void SetAnimationCurves(){
		//remove all the keys for the AnimationCurves
		for (int i = _xCurve.length; i > 0; i--) {
			_xCurve.RemoveKey(i);
		}

		for (int i = _yCurve.length; i > 0; i--) {
			_yCurve.RemoveKey(i);
		}

		for (int i = _zCurve.length; i > 0; i--) {
			_zCurve.RemoveKey(i);
		}
		//may need to create an array of keyframe times based on the vector magnitude of each point
		//read the values from controlPoints
		for(int i = 0; i < controlPoints.Count; i++){
			Keyframe key = new Keyframe(); //create a key set someboiler plate values
			key.time = (float)i/((float)controlPoints.Count-1.0f); //probably a hack
			key.tangentMode = 1; 
			key.inTangent = 0f; 
			key.outTangent = 0f; 
			//set each of the animation clips
			key.value = controlPoints[i].x;
			_xCurve.AddKey(key);
			key.value = controlPoints[i].y;
			_yCurve.AddKey(key);
			key.value = controlPoints[i].z;
			_zCurve.AddKey(key);
		}

		//smoothing the tangents
		for (int i = 0; i <_xCurve.length; i++) {
			_xCurve.SmoothTangents(i, 0);
		}

		for (int i = 0; i <_yCurve.length; i++) {
			_yCurve.SmoothTangents(i, 0);
		}

		for (int i = 0; i <_zCurve.length; i++) {
			_zCurve.SmoothTangents(i, 0);
		}
	}

	IEnumerator PlayAnimations(float duration){
		float time = 0f;
		float start = 0f;
		float end = 1.0f;

		//Debug.Log (this.name + " is going to path animate");
		while (time < duration) {
			if(!_paused){
				time += Time.deltaTime;
				if( time > duration) { time = duration; } //cap the time
				
				_clipPosition = Easing.ExpoEaseInOut(time, start, end, duration); //this will change later once I finish implementing ease

				Vector3 apply = new Vector3(_xCurve.Evaluate(_clipPosition),
				                            _yCurve.Evaluate(_clipPosition),
				                            _zCurve.Evaluate(_clipPosition));


				transform.localPosition = apply;
				yield return null;
			}
		}

		_playing = false;
		yield return null;
	}

	public void Play(){
		if (!_playing) {
			if (_xCurve != null && _yCurve != null && _zCurve != null) {
				StartCoroutine (PlayAnimations (speed));
				_playing = true;
			}
		}

		if (_paused) {
			_paused = false;
		}
	}

	public void Pause(){
		if (!_paused) {
			_paused = true;
		}
	}

	public void Stop(){
		StopCoroutine("PlayAnimations");
		_clipPosition = 0f;
		_playing = false;
	}

	void Start () {
		if (_xCurve.length == 0) {
			SetAnimationCurves ();
		}
		Invoke ("Play", 0.5f);
	}

	void Update () {
	}
}

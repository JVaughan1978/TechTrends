//Once more we are going to use Robert Penner's easing equations for maximum animated awesome
using UnityEngine;
using System.Collections;

// a plugin to implement Robert Penner's animation easing as a unity plugin/extension.
// this method allows for animation classes to be described that can then pick and choose
// implementations based on need and implement them more cleanly as part of other 
// other behaviors rather having them implemented as a monolithic object ala iTween.
// the basic pattern is simply current time(t), which will need to be tracked in the animating
// behavior, overall duration(d), start point(s), end point(e).
public enum EaseType {
	Linear,
	QuadEaseIn,
	QuadEaseOut,
	QuadEaseInOut,
	CubicEaseIn,
	CubicEaseOut,
	CubicEaseInOut,
	QuarticEaseIn,
	QuarticEaseOut,
	QuarticEaseInOut,
	QuinticEaseIn,
	QuinticEaseOut,
	QuinticEaseInOut,
	SineEaseIn,
	SineEaseOut,
	SineEaseInOut,
	ExpoEaseIn,
	ExpoEaseOut,
	ExpoEaseInOut,
	CircEaseIn,
	CircEaseOut,
	CircEaseInOut
}

//implement delegate function to evalute based on global easetype here

public class Easing : MonoBehaviour {	 

	public static float Linear(float t, float s, float e, float d) {
		return e*t/d + s;
	}

	// quadratic easing in - accelerating from zero velocity	
	public static float QuadEaseIn(float t, float s, float e, float d) {
		t /= d;
		return e*t*t + s;
	}	
	
	// quadratic easing out - decelerating to zero velocity	
	public static float QuadEaseOut(float t, float s, float e, float d) {
		t /= d;
		return -e * t*(t-2.0f) + s;
	}	
	
	// quadratic easing in/out - acceleration until halfway, then deceleration	
	public static float QuadEaseInOut(float t, float s, float e, float d) {
		t /= d/2.0f;
		if (t < 1.0f) return e/2.0f*t*t + s;
		t--;
		return -e/2.0f * (t*(t-2.0f) - 1.0f) + s;
	}	
	
	// cubic easing in - accelerating from zero velocity	
	public static float CubicEaseIn(float t, float s, float e, float d) {
		t /= d;
		return e*t*t*t + s;
	}	

	// cubic easing out - decelerating to zero velocity	
	public static float CubicEaseOut(float t, float s, float e, float d) {
		t /= d;
		t--;
		return e*(t*t*t + 1.0f) + s;
	}

	// cubic easing in/out - acceleration until halfway, then deceleration		
	public static float CubicEaseInOut(float t, float s, float e, float d){
		t /= d/2.0f;
		if (t < 1.0f) return e/2.0f*t*t*t + s;
		t -= 2.0f;
		return e/2.0f*(t*t*t + 2.0f) + s;
	}
	
	
	// quartic easing in - accelerating from zero velocity		
	public static float QuarticEaseIn(float t, float s, float e, float d){
		t /= d;
		return e*t*t*t*t + s;
	}	
	
	// quartic easing out - decelerating to zero velocity	
	public static float QuarticEaseOut(float t, float s, float e, float d){
		t /= d;
		t--;
		return -e * (t*t*t*t - 1.0f) + s;
	}	
	
	// quartic easing in/out - acceleration until halfway, then deceleration	
	public static float QuarticEaseInOut(float t, float s, float e, float d){
		t /= d/2.0f;
		if (t < 1.0f) return e/2.0f*t*t*t*t + s;
		t -= 2;
		return -e/2.0f * (t*t*t*t - 2.0f) + s;
	}	
	
	// quintic easing in - accelerating from zero velocity	
	public static float QuinticEaseIn(float t, float s, float e, float d){
		t /= d;
		return e*t*t*t*t*t + s;
	}	
	
	// quintic easing out - decelerating to zero velocity	
	public static float QuinticEaseOut(float t, float s, float e, float d){
		t /= d;
		t--;
		return e*(t*t*t*t*t + 1) + s;
	}	
	
	// quintic easing in/out - acceleration until halfway, then deceleration	
	public static float QuinticEaseInOut(float t, float s, float e, float d){
		t /= d/2.0f;
		if (t < 1.0f) return e/2.0f*t*t*t*t*t + s;
		t -= 2.0f;
		return e/2.0f*(t*t*t*t*t + 2.0f) + s;
	}	
	
	// sinusoidal easing in - accelerating from zero velocity	
	public static float SineEaseIn(float t, float s, float e, float d){
		return -e * Mathf.Cos(t/d * (Mathf.PI/2.0f)) + e + s;
	}	
	
	// sinusoidal easing out - decelerating to zero velocity	
	public static float SineEaseOut(float t, float s, float e, float d){
		return e * Mathf.Sin(t/d * (Mathf.PI/2.0f)) + s;
	}	
	
	// sinusoidal easing in/out - accelerating until halfway, then decelerating	
	public static float SineEaseInOut(float t, float s, float e, float d){
		return -e/2 * (Mathf.Cos(Mathf.PI*t/d) - 1.0f) + s;
	}	
	
	// exponential easing in - accelerating from zero velocity	
	public static float ExpoEaseIn(float t, float s, float e, float d){
		return e * Mathf.Pow( 2.0f, 10.0f * (t/d - 1.0f) ) + s;
	}	
	
	// exponential easing out - decelerating to zero velocity	
	public static float ExpoEaseOut(float t, float s, float e, float d){
		return e * ( -Mathf.Pow( 2.0f, -10.0f * t/d ) + 1.0f ) + s;
	}	
	
	// exponential easing in/out - accelerating until halfway, then decelerating	
	public static float ExpoEaseInOut(float t, float s, float e, float d){
		t /= d/2.0f;
		if (t < 1.0f) return e/2.0f * Mathf.Pow( 2.0f, 10.0f * (t - 1.0f)) + s;
		t--;
		return e/2 * ( -Mathf.Pow( 2.0f, -10.0f * t) + 2.0f ) + s;
	}	
	
	// circular easing in - accelerating from zero velocity	
	public static float CircEaseIn(float t, float s, float e, float d){
		t /= d;
		return -e * (Mathf.Sqrt(1.0f - t*t) - 1.0f) + s;
	}	
	
	// circular easing out - decelerating to zero velocity	
	public static float CircEaseOut(float t, float s, float e, float d){
		t /= d;
		t--;
		return e * Mathf.Sqrt(1.0f - t*t) + s;
	}	
	
	// circular easing in/out - acceleration until halfway, then deceleration	
	public static float CircEaseInOut(float t, float s, float e, float d){
		t /= d/2.0f;
		if (t < 1.0f) return -e/2.0f * (Mathf.Sqrt(1.0f - t*t) - 1.0f) + s;
		t -= 2.0f;
		return e/2.0f * (Mathf.Sqrt(1.0f - t*t) + 1.0f) + s;
	}
}
using UnityEngine;
using System.Collections;

public class LogoPlayer : MonoBehaviour {

	// Use this for initialization
	void PlayAnim(){
		this.GetComponent<PathAnimation>().Play();
	}
	void Start () {
		if (GetComponent<PathAnimation>() != null) {
			Invoke("PlayAnim", 3.0f);
		}
	}
}

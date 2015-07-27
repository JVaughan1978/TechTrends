using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find("Main Camera");
		if (target == null) {
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);		 
	}
}

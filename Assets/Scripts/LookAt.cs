using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	GameObject target;

	void Start () {
		target = GameObject.Find("Main Camera");
		if (target == null) {
			this.enabled = false;
		}
	}	
	
	void Update () {
		transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);		 
	}
}

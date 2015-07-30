using UnityEngine;
using System.Collections;

public class OrientTowards : MonoBehaviour {

    GameObject target;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Main Camera");
	}

    void Update() {
    }

	void LateUpdate () {
        Vector3 lookDir = transform.position - target.transform.position;
        lookDir.y = 0; 
        transform.rotation = Quaternion.LookRotation(lookDir);	
	}
}

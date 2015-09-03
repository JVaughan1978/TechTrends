using UnityEngine;
using System.Collections;

public class PinToPosition : MonoBehaviour {

    public GameObject target;

	void LateUpdate () {
        transform.position = target.transform.position;
	}
}

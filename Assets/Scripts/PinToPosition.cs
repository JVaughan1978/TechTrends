using UnityEngine;
using System.Collections;

public class PinToPosition : MonoBehaviour {

    public GameObject target;

	void Update () {
        transform.position = target.transform.position;
	}
}

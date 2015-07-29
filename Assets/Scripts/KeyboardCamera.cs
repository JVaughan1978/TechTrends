using UnityEngine;
using System.Collections;

public class KeyboardCamera : MonoBehaviour {

    float speed = 75.0f;
	
	// Update is called once per frame
	void Update () {
        Vector3 newRotation = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0f);
        transform.Rotate(newRotation * speed * Time.deltaTime);
	}
}

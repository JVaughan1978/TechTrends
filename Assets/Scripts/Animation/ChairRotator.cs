using UnityEngine;
using System.Collections;

public class ChairRotator : MonoBehaviour {

    private GameObject _camera;
    public float rotationOffset = 0;

	// Use this for initialization
	void Start () {
        _camera = GameObject.Find("Main Camera");
        if(_camera == null) {
            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newRotation = new Vector3(0, _camera.transform.localEulerAngles.y + rotationOffset, 0);
        transform.localEulerAngles = newRotation;
	}
}

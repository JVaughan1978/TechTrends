using UnityEngine;
using System.Collections;

public class PinPosition : MonoBehaviour {

	public float zOffset = 0;
	private bool _set = false;
	private float _lastOffset = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_lastOffset != zOffset) {
			_set = false;
		}

		if (!_set) {
			transform.localRotation = transform.parent.localRotation;
			transform.localPosition = transform.parent.localPosition;
			Vector3 translator = new Vector3 (0f, 0f, zOffset);
			transform.Translate(translator); 
			_set = true;
		}

		_lastOffset = zOffset;
	}
}

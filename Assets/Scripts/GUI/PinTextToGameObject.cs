using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class PinTextToGameObject : MonoBehaviour {

	public GameObject pinnedObject;
	private MeshRenderer _mr;
	private Text _txt;
	public float zOffset = 0;
	public bool usePinnedObjectRotation = false;
	private bool _set = false;
	private float _lastOffset = 0;

	// Use this for initialization
	void Start () {
		if (pinnedObject != null) {
			_mr = pinnedObject.GetComponent<MeshRenderer>();
			if(_mr == null) {
				Debug.Log ("No MeshRenderer found.");
			}
		}
		_txt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_mr.enabled == true) {
			_txt.enabled = true;
			if (_lastOffset != zOffset) {
				_set = false;
			}
			
			if (!_set) {
				if (usePinnedObjectRotation) {
					transform.localRotation = pinnedObject.transform.localRotation;
				}
				transform.localPosition = pinnedObject.transform.position; //need the world space for this
				Vector3 translator = new Vector3 (0f, 0f, zOffset);
				transform.Translate (translator); 
				_set = true;
			}
			
			_lastOffset = zOffset;
		} else {
			_txt.enabled = false;
		}	
	}
}

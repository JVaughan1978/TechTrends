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
            Vector3 offsetPinnedObjectPosition = new Vector3(pinnedObject.transform.position.x, pinnedObject.transform.position.y,
                                                             pinnedObject.transform.position.z + zOffset);
					
            if(!ExtensionMethods.CompareVectors(offsetPinnedObjectPosition, transform.localPosition, 0.1f)){
				_set = false;
			}
			
			if (!_set) {
				if (usePinnedObjectRotation) {
					transform.localRotation = pinnedObject.transform.localRotation;
				}
				transform.localPosition = offsetPinnedObjectPosition; //need the world space for this				
				_set = true;
			}
            _txt.enabled = true;				
		} else {
			_txt.enabled = false;
		}	
	}
}

using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	public float zOffset = -0.25f;
	public Material highlightMaterial;

    private bool _animating = false;
	private bool _highlighted = false;
	private bool _lasthighlight = false;
	private float _duration = 0.5f;
	private float _time = 0;
    private bool _positionSet = false;
	private Vector3 _originalPos = Vector3.zero;
	private Vector3 _highlightedPos = Vector3.zero;
	private Material _originalMaterial;

	void OnEnable(){
		HighlightReaction.OnHighlight += Highlighted;
		HighlightReaction.OnDeselect += Deselected;
	}

	void OnDisable(){
		HighlightReaction.OnHighlight -= Highlighted;
		HighlightReaction.OnDeselect -= Deselected;
	}

	void Highlighted(string broadcastName) {
		if (gameObject.name == broadcastName) {
            _highlighted = true;
            _animating = true;
        } else {
            _highlighted = false;
        }
	}

	void Deselected(string broadcastName) {
		if (gameObject.name == broadcastName) {
            _highlighted = false;
		}
	}

    //putting this in the wrong place for now
    bool CompareVectors(Vector3 v1, Vector3 v2, float angleError) {
        if(!Mathf.Approximately(v1.magnitude, v2.magnitude)) {
            return false;
        }

        float cosAngleError = Mathf.Cos(angleError * Mathf.Deg2Rad);
        float cosAngle = Vector3.Dot(v1.normalized, v2.normalized);

        if(cosAngle >= cosAngleError) {
            return true;
        } else {
            return false;
        }        
    }
    
	// Update is called once per frame
	void Update () {
        //material swapping
        if(_highlighted != _lasthighlight) { 
           // _positionSet = false;
        }
        
        if(_animating && !_positionSet) {
            if (CompareVectors(transform.localPosition, _originalPos, 0.1f)){
                _positionSet = true;
            } else {
                _originalPos = transform.localPosition;
                _highlightedPos = new Vector3(transform.localPosition.x, transform.localPosition.y, (transform.localPosition.z + zOffset));
                _positionSet = true;
            }
        }

        if(_animating) { 
            if (_highlighted) {           
                _time += Time.deltaTime;
                _time = Mathf.Clamp(_time, 0f, _duration);
                float lerp = Easing.ExpoEaseInOut(_time, 0f, 1f, _duration);
                transform.localPosition = Vector3.Lerp(_originalPos, _highlightedPos, lerp);
            } else {
                _time -= Time.deltaTime;
                _time = Mathf.Clamp(_time, 0f, _duration);
                float lerp = Easing.ExpoEaseInOut(_time, 0f, 1f, _duration);
                transform.localPosition = Vector3.Lerp(_originalPos, _highlightedPos, lerp);
            }
        }

        _lasthighlight = _highlighted;
	}
}

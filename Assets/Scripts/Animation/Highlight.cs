using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	public float zOffset = -0.33f;
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
    private Vector3 two = new Vector3(2.0f, 2.0f, 2.0f);

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
	
	void Update () {
        //NOT IMPLEMENTED: material swapping
        if(_highlighted != _lasthighlight) { 
           // _positionSet = false;
        }
        
        if(_animating && !_positionSet) {                        
            if (ExtensionMethods.CompareVectors(transform.localPosition, _originalPos, 0.1f)){
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

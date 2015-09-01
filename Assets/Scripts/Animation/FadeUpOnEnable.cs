using UnityEngine;
using System.Collections;

public class FadeUpOnEnable : MonoBehaviour {

    public float fadeUpTime = 0.3f;
    public float lowerYOffset = -1.0f;
    public bool changeMat = true;

    private MeshRenderer _mr = null;    
    private Color _initialColor = Color.black;
    private Vector3 _initialPosition = Vector3.zero;
    private float _currTime = 0;
    private bool _done = false;

    // Use this for initialization
	void Start () {
        _mr = GetComponent<MeshRenderer>();
        if(_mr == null) {
            Debug.LogWarning("no mesh renderer found.");
        }

        if(changeMat) {
            _initialColor = _mr.material.GetColor("_Color");
        }        
        _initialPosition = transform.localPosition;
	}

    void OnEnable() {
        _currTime = 0;
        _done = false;
    }
	
	// Update is called once per frame
	void Update () {        
        if(_currTime < fadeUpTime) {
            _currTime += Time.deltaTime;
            
            if(changeMat) {                
                float fade = _currTime / fadeUpTime;
                Color currentColor = new Color(_initialColor.r * fade, _initialColor.g * fade, _initialColor.b * fade, _initialColor.a);
                _mr.material.SetColor("_Color", currentColor);
            }                      
            
            float currentOffset = lowerYOffset * (( fadeUpTime -_currTime) / fadeUpTime);
            Vector3 currentPos = new Vector3(_initialPosition.x, _initialPosition.y - currentOffset, _initialPosition.z);
            transform.localPosition = currentPos;            

        } else {
            if(!_done) {
                if(changeMat) {
                    _mr.material.SetColor("_Color", _initialColor);
                }
                
                transform.localPosition = _initialPosition;
                _done = true;
            }            
        }
	}
}

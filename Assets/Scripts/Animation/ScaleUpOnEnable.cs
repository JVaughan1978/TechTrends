using UnityEngine;
using System.Collections;

public class ScaleUpOnEnable : MonoBehaviour {
    
    public float fadeUpTime = 0.3f;
    public Vector3 startScale = new Vector3( 0.05f, 0.05f, 0.05f);

    private Vector3 _initialScale = Vector3.one;
    private float _currTime = 0;
    private bool _done = false;

    // Use this for initialization
	void Start () {
        _initialScale = transform.localScale;
	}

    void OnEnable() {
        _currTime = 0;
        _done = false;
    }
	
	// Update is called once per frame
	void Update () {        
        if(_currTime < fadeUpTime) {
            _currTime += Time.deltaTime;                        
            
            float lerp = _currTime / fadeUpTime;

            Vector3 currentScale = Vector3.Lerp(startScale, _initialScale, lerp);        
            transform.localScale = currentScale;            

        } else {
            if(!_done) {                          
                transform.localScale = _initialScale;
                _done = true;
            }            
        }
	}
}

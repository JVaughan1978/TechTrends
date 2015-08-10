using UnityEngine;
using System.Collections;

public class GazeDirectionVolume : MonoBehaviour {

    private GameObject _camera;
    private AudioSource _audioSource;
    private float _volume = 0;
   
    public float direction = 0;
    public float offset = 45;
    public float boost = .25f;

    public float currentDirection = 0;
    public float currentBoost = 0;

	// Use this for initialization
	void Start () {
        _camera = GameObject.Find("Main Camera");
        if(_camera == null) {
            this.enabled = false;
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null) {
            this.enabled = false;
        } else {
            _volume = _audioSource.volume;
        }
	}

    float BoostVolume(float cd) {
        float ret = 0;       
        float minAngle = direction - offset;
        float maxAngle = direction + offset;

        if(minAngle < 0f && cd > (minAngle + 360.0f)) {
            minAngle += 360.0f;  
            return ret = ((cd - minAngle) / offset) * boost;            
        }

        if(maxAngle > 360f && cd < (maxAngle - 360.0f)) {
            maxAngle -= 360.0f;
            return ret = ((maxAngle - cd) / offset) * boost;
        }

        if(cd > minAngle && cd <= direction) {
            return ret = ((cd - minAngle) / offset) * boost;
        } else if (cd < maxAngle && cd >= direction) {
            return ret = ((maxAngle - cd) / offset) * boost;
        }
        
        return ret;
    }
	
	// Update is called once per frame
	void Update () {        
        offset = Mathf.Clamp(offset, 0f, 90f);        
        boost = Mathf.Clamp01(boost);

        _audioSource.volume = _volume + BoostVolume(_camera.transform.localEulerAngles.y);
	}
}

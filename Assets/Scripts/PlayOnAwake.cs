using UnityEngine;
using System.Collections;

public class PlayOnAwake : MonoBehaviour {

    public GameObject mmt;

	// Use this for initialization
	void Start () {
        if(mmt == null) {
            this.enabled = false;
        }
	}

    void OnEnable() {
        MMT.MobileMovieTexture _mmt = mmt.GetComponent<MMT.MobileMovieTexture>();
        _mmt.Play();
    }

    void OnDisable() {
        //we'll reset the video later.
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}

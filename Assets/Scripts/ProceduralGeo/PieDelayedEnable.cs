using UnityEngine;
using System;
using System.Collections;

public class PieDelayedEnable : MonoBehaviour {

    [System.Serializable]
    public class DelayedStart {
        public GameObject gameObject;
        public float startTime = 0;
        public bool started = false;
    }

    public DelayedStart[] startTimes = new DelayedStart[0];

    private float _time = 0;
    private float _lastStartTime = 0;
    private bool _allStarted = false;

	// Update is called once per frame
    void Start() {
        foreach (DelayedStart ds in startTimes) {
            if (ds.startTime > _lastStartTime) { _lastStartTime = ds.startTime; }
            ds.gameObject.SetActive(false);
        }        
    }

	void Update () {
        if (!_allStarted) {
            foreach(DelayedStart ds in startTimes){
                if(ds.started == false){
                    if(_time > ds.startTime){
                        ds.gameObject.SetActive(true);
                        ds.started = true;
                    }
                }
            }

            if (_time > _lastStartTime) { _allStarted = true; };
        }

        _time += Time.deltaTime;
    }
}
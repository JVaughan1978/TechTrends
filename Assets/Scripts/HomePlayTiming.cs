using UnityEngine;
using System.Collections;

public class HomePlayTiming : MonoBehaviour {

    public AudioSource vidAudio;
    public MMT.MobileMovieTexture mmt;
    public AudioSource introAudio;
    public GameObject textA;
    public GameObject textB;
    public GameObject textHolder;

    public float introTime = 12.0f;
    public float textSwitchTime = 50.0f;
    private float _currentTime = 0.0f;
    private bool _vidStarted = false;
    private bool _targetEnabled = false;

    void TargetEnabled() {
        vidAudio.enabled = false;
        mmt.enabled = true;
        mmt.Pause = true;            
        introAudio.enabled = true;
        textA.SetActive(false);
        textB.SetActive(false);
        textHolder.SetActive(false);
    }

    void TargetDisabled() {
        vidAudio.enabled = false;
        mmt.enabled = true;
        mmt.Pause = true;            
        introAudio.enabled = true;
        _currentTime = 0.0f;
        _vidStarted = false;
        textA.SetActive(false);
        textB.SetActive(false);
        textHolder.SetActive(false);
    }
        
	// Use this for initialization
	void Start () {
        if(vidAudio == null) {
            Debug.LogWarning("No VidAudio Found");            
        }

        if(mmt == null) {
            Debug.LogWarning("No MobileMovieTexture Found");
        }

        if(introAudio == null) {
            Debug.LogWarning("No IntroAudio Found");
        }
	}

    bool GetTargetState() {
        bool ret = mmt.gameObject.active;
        return ret;
    }
	
	// Update is called once per frame
	void Update () {

        _targetEnabled = GetTargetState();

        if(_targetEnabled) {
            _currentTime += Time.deltaTime;

            if(_currentTime > introTime && !_vidStarted ) {
                vidAudio.enabled = true;
                mmt.Pause = false;            
                introAudio.enabled = false;
                _vidStarted = true;
                textA.SetActive(true);
                textB.SetActive(false);
                textHolder.SetActive(true);
            }

            if(_currentTime > textSwitchTime) {
                textA.SetActive(false);
                textB.SetActive(true);
            }
        }
	}
}

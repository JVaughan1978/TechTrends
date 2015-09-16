using UnityEngine;
using System.Collections;

public class HomePlayTiming : MonoBehaviour {

    public AudioSource vidAudio;
    public MMT.MobileMovieTexture mmt;
    public AudioSource introAudio;
    public GameObject vidMesh;
    public GameObject textA;
    public GameObject textB;
    public GameObject textHolder;
    public GameObject x_Button;

    public bool hasIntroAudio = false;
    public bool separateMovieAudio = false;
    public bool textSwitches = false;

    public float introTime = 12.0f;
    public float xTime = 22.0f;
    public float textSwitchTime = 50.0f;
    private float _currentTime = 0.0f;
    private bool _vidStarted = false;

    void OnEnable() {
        SelectionReaction.OnSelect += OnSelect;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= OnSelect;
    }

    void OnSelect() {        
        vidAudio.enabled = false;
        mmt.Pause = true;
        introAudio.enabled = false;
        vidMesh.SetActive(false);
        textA.SetActive(false);
        textB.SetActive(false);
        textHolder.SetActive(false);
        x_Button.SetActive(false);
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
	
	// Update is called once per frame
	void Update () {
        _currentTime += Time.deltaTime;
        
        if(hasIntroAudio == false) {
            introTime = 0f;
            introAudio.enabled = false;
        }
        
        if(_currentTime > introTime && !_vidStarted ) {
            vidAudio.enabled = true;
            mmt.Pause = false;
            if(!hasIntroAudio) { introAudio.enabled = false; }            
            _vidStarted = true;
            vidMesh.SetActive(true);
            textA.SetActive(true);
            textB.SetActive(false);
            textHolder.SetActive(true);
            x_Button.SetActive(false);
        }

        if(_currentTime > xTime) {
            x_Button.SetActive(true);
        }

        if(_currentTime > textSwitchTime && textSwitches) {
           textA.SetActive(false);
           textB.SetActive(true);
        }        
	} 
}

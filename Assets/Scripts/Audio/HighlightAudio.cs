using UnityEngine;
using System.Collections;

public class HighlightAudio : MonoBehaviour {

    private bool _isHighlighted = false;

    public delegate void HighlightAudioPlayAction (string name);
    public static event HighlightAudioPlayAction OnHighlightAudioPlay;

    public delegate void HighlightAudioStopAction (string name);
    public static event HighlightAudioStopAction OnHighlightAudioStop;

    void OnEnable() {
        HighlightAudio.OnHighlightAudioPlay += Playing;
        HighlightAudio.OnHighlightAudioStop += Stopped;
    }

    void OnDisable() {
        HighlightAudio.OnHighlightAudioPlay -= Playing;
        HighlightAudio.OnHighlightAudioStop -= Stopped;
    }

    void IsPlaying() {
        if(OnHighlightAudioPlay != null) {
            OnHighlightAudioPlay(this.name);
        }
    }

    void IsStopped() {
        if(OnHighlightAudioStop != null) {
            OnHighlightAudioStop(this.name);
        }
    }

    void Playing(string name) {
        if(name == this.name) {
            IsPlaying();
        }
    }

    void Stopped(string name) {
        if(name == this.name) {
            IsStopped();            
        }
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}

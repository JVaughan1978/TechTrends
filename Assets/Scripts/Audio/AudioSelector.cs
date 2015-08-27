using UnityEngine;
using System.Collections;

public class AudioSelector : MonoBehaviour {

    public bool playOnSelect = false;
    public bool playOnDeselect = false;
    public bool playOnMode = false;
    public bool playOnFocus = false;    

    private AudioSource _as;

    void OnEnable() {
        SelectionReaction.OnDeselect += DeselectSwitch;
        SelectionReaction.OnFocus += FocusSwitch;
        SelectionReaction.OnMode += ModeSwitch;
        SelectionReaction.OnSelect += SelectSwitch;
    }

    void OnDisable() {
        SelectionReaction.OnDeselect -= DeselectSwitch;
        SelectionReaction.OnFocus -= FocusSwitch;
        SelectionReaction.OnMode -= ModeSwitch;
        SelectionReaction.OnSelect += SelectSwitch;
    }

    void DeselectSwitch() {
        if(playOnDeselect) {
            _as.Play();
        }
    }

    void FocusSwitch() {
        if(playOnFocus) {
            _as.Play();
        }
    }

    void ModeSwitch(bool check) {
        if(playOnMode) {
            _as.Play();
        }

    }

    void SelectSwitch() {
        if(playOnSelect) {
            _as.Play();
        }
    }

    void Start() {
        _as = GetComponent<AudioSource>();
        if(_as == null) {
            this.enabled = false;
        }
    }

    void Update() {

    }
}
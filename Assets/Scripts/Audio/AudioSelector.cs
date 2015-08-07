using UnityEngine;
using System.Collections;

public class AudioSelector : MonoBehaviour {

    public bool playOnSelect = false;
    public bool playOnDeselect = false;
    public bool playOnMode = false;
    public bool playOnFocus = false;

    public Sector sector = Sector.Automotive;

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

    void DeselectSwitch(Sector sect) {
        if(playOnDeselect) {
            _as.Play();
        }
    }

    void FocusSwitch(Sector sect) {
        if(playOnFocus) {
            _as.Play();
        }
    }

    void ModeSwitch(bool check) {
        if(playOnMode) {
            _as.Play();
        }

    }

    void SelectSwitch(Sector sect) {
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
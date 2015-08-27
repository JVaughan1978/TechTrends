using UnityEngine;
using System.Collections;

public class BackIcon : MonoBehaviour {

    private MeshRenderer _mr = null;
    private BoxCollider _bc = null;
    private bool _toggle = true;

    void OnEnable() {        
        SelectionReaction.OnSelect += SectorSwitch;
    }

    void OnDisable() {        
        SelectionReaction.OnSelect += SectorSwitch;
    }

    void ModeSwitch() {
        SetIcon(_toggle);
        _toggle = !_toggle;
    }

    void SectorSwitch() {
    }

    void SetIcon(bool check) {
        _mr.enabled = check;
        _bc.enabled = check;
    }

    // Use this for initialization
    void Start() {
        _mr = GetComponent<MeshRenderer>();
        _bc = GetComponent<BoxCollider>();        
        SetIcon(false);
    }

    // Update is called once per frame
    void Update() {
    }
}

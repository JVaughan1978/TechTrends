using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeatMeterVisibility : MonoBehaviour {

    private bool _toggle = true;
    private Text _txt;

    void OnEnable() {
        SelectionReaction.OnMode += ModeSwitch;
    }

    void OnDisable() {
        SelectionReaction.OnMode -= ModeSwitch;
    }

    void ModeSwitch(bool check) {
        SetIcon(_toggle);
        _toggle = !_toggle;
    }

    void SetIcon(bool check) {
        int goCount = transform.childCount;
        for(int i = 0; i < goCount; i++) {
            Transform t = transform.GetChild(i);
            GameObject go = t.gameObject;
            go.SetActive(check);
        }

        if(_txt != null) {
            _txt.enabled = check;
        }
    }

    // Use this for initialization
    void Start() {
        _txt = GetComponent<Text>();
        SetIcon(false);        
    }

    
    void Update() {
    }
}

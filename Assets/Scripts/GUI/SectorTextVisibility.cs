using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SectorTextVisibility : MonoBehaviour {

    private bool _toggle = false;
    private string _text = "";
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
        if(_txt != null) {
            if(check) {
                StartCoroutine(WaitAndShow(1.0f));
            } else {
                _txt.text = "";
            }
        }
    }

    IEnumerator WaitAndShow(float wait) {
        yield return new WaitForSeconds(wait);
        _txt.text = _text;
    }

    // Use this for initialization
    void Start() {        
        SetIcon(true);
        _txt = GetComponent<Text>();
        if(_txt != null) {
            _text = _txt.text;
        } else {
            this.enabled = false;
        }
        
    }


    void Update() {
    }
}

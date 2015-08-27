using UnityEngine;
using System.Collections;

public class SetBasePosition : MonoBehaviour {

    public GameObject anchorHead = null;

    private Transform[] _anchors = new Transform[8];

    void OnEnable() {
        SelectionReaction.OnSelect += SwitchAnchor;
    }

    void OnDisable() {
        SelectionReaction.OnSelect += SwitchAnchor;
    }

    void SwitchAnchor() {
        transform.localPosition = _anchors[0].position;
    }

    void Start() {
        if(anchorHead != null) {
            for(int i = 0; i < 8; i++) {
                Transform tempT = anchorHead.transform.GetChild(i);
                _anchors[i] = tempT;
            }
        }
    }

    void Update() {
    }
}
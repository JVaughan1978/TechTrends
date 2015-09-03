﻿using UnityEngine;
using System.Collections;

public class GazeManager : MonoBehaviour {

    public float coolDownTime = 0.1f;
    private int gazeDistance = 0;
    private int scannableLayerMask = 0;
    private bool _casting = true;    

    private SelectionReaction selectionReaction;
    private HighlightReaction highlightReaction;

    void OnEnable() {
        SelectionReaction.OnSelect += TimeOut;        
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= TimeOut;        
    }

    IEnumerator CoolDown(float time) {
        _casting = false;
        yield return new WaitForSeconds(time);
        _casting = true;
    }

    void TimeOut() {
        StartCoroutine(CoolDown(0.1f));        
    }

    void Start() {
        gazeDistance = 100;
        scannableLayerMask = 1 << 8;
    }

    void Update() {
        if(_casting) {
            RaycastHit hit;
            if(Physics.Raycast(transform.position,
                                 transform.rotation * Vector3.forward, out hit, gazeDistance, scannableLayerMask)) {
            } else {
                if(selectionReaction != null) {
                    selectionReaction.Deselected();
                    selectionReaction = null;
                }
                if(highlightReaction != null) {
                    highlightReaction.Deselect();
                    highlightReaction = null;
                }
            }

            if(hit.collider != null) {
                CheckGazedObject(hit);
            }

            Debug.DrawRay(transform.position, transform.rotation * Vector3.forward, Color.green);
        }
    }

    void CheckGazedObject(RaycastHit hit) {
        selectionReaction = hit.collider.gameObject.GetComponent<SelectionReaction>();
        if(selectionReaction != null) {
            selectionReaction.InFocus();
        }
        highlightReaction = hit.collider.gameObject.GetComponent<HighlightReaction>();
        if(highlightReaction != null) {
            highlightReaction.Highlight();
        }
    }
}

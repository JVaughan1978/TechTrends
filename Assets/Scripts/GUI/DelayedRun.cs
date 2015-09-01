using UnityEngine;
using System.Collections;

public class DelayedRun : MonoBehaviour {    
    
    public float movieDuration = 15.0f;
    private bool switched = false;

    public delegate void EndAction();
    public static event EndAction OnEnd;

    void OnEnable() {
        SelectionReaction.OnSelect += Selected;
        TopMenuVis.OnMenu += VidOver;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= Selected;
        TopMenuVis.OnMenu += VidOver;
    }

    private void Ended() {
        if(OnEnd != null) {
            OnEnd();
        }
    }

    private void VidOver(string name) {
        Hide();
    }

    void Selected() {
        if(!switched) {
            Debug.Log("DelayedRun switched");
            Invoke("Show", 0.2f);
            Invoke("Ended", movieDuration);
            switched = true;
        }
    }

    void Show() {
        
        for(int i = 0; i < transform.childCount; i++) {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(true);
        }
    }

    void Hide() {
        for(int i = 0; i < transform.childCount; i++) {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start() {
        Hide();
    }

    // Update is called once per frame
    void Update() {

    }
}
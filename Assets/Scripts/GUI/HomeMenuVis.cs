using UnityEngine;
using System.Collections;

public class HomeMenuVis : MonoBehaviour {

    public float movieDuration = 15.0f;
    public GameObject MTT;
    public string reaction = "";
    private bool switched = false;

    public delegate void EndAction(string name);
    public static event EndAction OnEnd;
    public bool _ended = false;

    void OnEnable() {
        SelectionReaction.OnNameSelect += Selected;
        SelectionReaction.OnSelect += Switch;
    }

    void OnDisable() {
        SelectionReaction.OnNameSelect -= Selected;
        SelectionReaction.OnSelect -= Switch;
    }

    private void Ended() {
        if(OnEnd != null) {
            OnEnd(this.name);
            _ended = true;
        }
    }

    void Selected(string name) {
        if(name == reaction) {
            if(!switched) {
                switched = true;
                Debug.Log("DelayedRun switched");
                Invoke("Show", 0.2f);
                Invoke("Ended", movieDuration);
            }
        }
    }

    void Switch() {
        if(switched && !_ended) {
            MTT.GetComponent<HomePlayTiming>().enabled = false;
            CancelInvoke("Ended");
            Ended();
        } else {
            Hide();
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

    void Start() {
        Hide();
    }

    void Update() {

    }
}

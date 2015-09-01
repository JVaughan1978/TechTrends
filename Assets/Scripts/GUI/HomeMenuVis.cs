using UnityEngine;
using System.Collections;

public class HomeMenuVis : MonoBehaviour {

    public float movieDuration = 15.0f;
    public GameObject MTT;
    private bool switched = false;

    public delegate void EndAction();
    public static event EndAction OnEnd;
    public bool _ended = false;

    void OnEnable() {
        SelectionReaction.OnSelect += Selected;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= Selected;
    }

    private void Ended() {
        if(OnEnd != null) {
            OnEnd();
            _ended = true;
        }
    }

    void Selected() {
        if(!switched) {
            switched = true;
            Debug.Log("DelayedRun switched");
            Invoke("Show", 0.2f);
            Invoke("Ended", movieDuration);
        } else if(!_ended) {
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

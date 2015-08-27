using UnityEngine;
using System.Collections;

public class DelayedRun : MonoBehaviour {

    private bool switched = false;
    public float movieDuration = 15.0f;

    void OnEnable() {
        SelectionReaction.OnSelect += Selected;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= Selected;
    }

    void Selected() {
        if(switched == false) {
            Show();
            Invoke("Hide", movieDuration);
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

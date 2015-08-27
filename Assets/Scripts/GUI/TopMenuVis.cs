using UnityEngine;
using System.Collections;

public class TopMenuVis : MonoBehaviour {

    public delegate void MenuAction(string name);
    public static event MenuAction OnMenu;

    void OnEnable() {
        DelayedRun.OnEnd += VidEnd;
        SelectionReaction.OnSelect += MenuSelection;
    }

    void OnDisable() {
        DelayedRun.OnEnd -= VidEnd;
        SelectionReaction.OnSelect -= MenuSelection;
    }

    void MenuSelection() {
        if(OnMenu != null) {
            OnMenu(this.name);
        }
    }   
 
    void VidEnd() {
        Show();
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

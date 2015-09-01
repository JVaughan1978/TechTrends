using UnityEngine;
using System.Collections;

public class TopMenuVis : MonoBehaviour {

    public delegate void MenuAction(string name);
    public static event MenuAction OnMenu;

    void OnEnable() {
        HomeMenuVis.OnEnd += VidEnd;
        SelectionReaction.OnSelect += MenuSelection;
    }

    void OnDisable() {
        HomeMenuVis.OnEnd -= VidEnd;
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
    
    void Start() {
        Hide();
    }
    
    void Update() {

    }
}

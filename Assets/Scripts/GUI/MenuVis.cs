using UnityEngine;
using System.Collections;

public class MenuVis : MonoBehaviour {

    public string reactionName = "";
    private bool _shown = false;

    void OnEnable() {
        SelectionReaction.OnNameSelect += Select;        
    }

    void OnDisable() {
        SelectionReaction.OnNameSelect -= Select;        
    }

    void Select(string name) {
        if(name == reactionName) {
            Show();
            _shown = true;
        }
        if(name != reactionName) {
            Deselect();
        }
    }

    void Deselect() {
        if(_shown) {
            Hide();
            _shown = false;
        }
    }

    void Show() {
        for(int i = 0; i < transform.childCount; i++) {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(true);//going to need another way to set this, too many objects might not need to be seen
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

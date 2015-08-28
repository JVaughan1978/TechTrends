using UnityEngine;
using System.Collections;

public class WeekVisManager : MonoBehaviour {

    public GameObject weekRoot = null;
    private bool switched = false;

    void OnEnable() {
        SelectionReaction.OnSelect += Selected;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= Selected;
    }

    void Selected() {
        if(switched == false) { 
            Hide();
            switched = true;   
        }        
    }

    void Show() {
        weekRoot.SetActive(true);
    }

    void Hide() {
        weekRoot.SetActive(false);
    }

    void Start() {
        Show();
    }

    void Update() {

    }
}

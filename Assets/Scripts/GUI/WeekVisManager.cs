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

    void Hide() {
        weekRoot.SetActive(false);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}

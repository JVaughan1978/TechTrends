using UnityEngine;
using System.Collections;

public class TelMailPop : MonoBehaviour {

    private MeshRenderer _mr = null;
    public string highlightMeshName = "";

    void OnEnable() {
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
    }

    void OnDisable() {
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
    }

    void Highlighted(string name) {
        if(name == highlightMeshName) {
            _mr.enabled = true;
        }   
    }

    void Deselected(string name) {
        if(name == highlightMeshName) {
            _mr.enabled = false;
        } 
    }
        
	// Use this for initialization
	void Start () {
        _mr = GetComponent<MeshRenderer>();
        if(_mr == null) {
            Debug.LogWarning("no MeshRenderer found");
        }
        _mr.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

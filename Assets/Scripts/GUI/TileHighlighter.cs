using UnityEngine;
using System.Collections;

public class TileHighlighter : MonoBehaviour {

    public Material baseMat = null;
    public Material highlightMat = null;
    
    private MeshRenderer _mr = null;

    void OnEnable() {
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
    }

    void OnDisable() {
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
    }

    void Highlighted(string name) {
        if(name == this.name) {
            _mr.material = highlightMat;
        }
    }

    void Deselected(string name) {
       _mr.material = baseMat;
    }

    void Start() {
        _mr = gameObject.GetComponent<MeshRenderer>();
        if(_mr == null) {
            Debug.LogWarning("No MeshRenderer found!");
        }
    }

    void Update() {

    }
}

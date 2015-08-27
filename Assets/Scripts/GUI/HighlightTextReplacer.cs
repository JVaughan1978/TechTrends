using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HighlightTextReplacer : MonoBehaviour {

    private Text _thisText;    
    private string _text = "";
    private bool _highlightedEvent = false;
    private bool _deselectedEvent = false;
        
    // YES THIS SHOULD REALLY NOT BE HERE.
    private Dictionary<string, int> _data = new Dictionary<string,int>();

    void OnEnable() {        
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
    }

    void OnDisable() {        
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
    }    

    void Highlighted(string text) {
        _highlightedEvent = true;
        _text = text;
    }

    void Deselected(string text) {
        _deselectedEvent = true;
    }

    IEnumerator Blank() {
        yield return new WaitForSeconds(0.25f);
        _thisText.text = "";
        yield return null;
    }
	
	void Start () {
        _thisText = GetComponent<Text>();        
	}	
	
	void Update () {     
        if(_deselectedEvent && !_highlightedEvent) {
            if(_thisText != null) {
                StopAllCoroutines();
                StartCoroutine("Blank");
            }            
            _deselectedEvent = false;
        }

        if(_highlightedEvent) {
            if(_thisText != null) {
                StopAllCoroutines();
                _thisText.text = GetText();
            }
            _highlightedEvent = false;
        }
	}
    private string GetText(){
        int _trendValue = 0;
        if(_data.TryGetValue(_text, out _trendValue)){
            Debug.Log(_trendValue.ToString());
        }
        string outText = _text + " : " + _trendValue.ToString();
        return outText;
    }    
}

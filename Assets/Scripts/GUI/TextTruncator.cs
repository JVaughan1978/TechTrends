using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTruncator : MonoBehaviour {

    public string fullName = "";
    public int trendCount = 0;
    
    private string _currentName = ""; 
    private Text _text;

    private bool _highlightedEvent = false;
    private bool _deselectedEvent = false;
    private bool _working = true;
    
    void OnEnable(){        
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
    }

    void OnDisable(){        
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
    }

    void Highlighted(string text){
        if(fullName == text){
            _highlightedEvent = true;
        } else {
            _deselectedEvent = true;
        }
    }

    void Deselected(string text){
        _deselectedEvent = true;
    }

    string SetDisplayString(int _override, bool showDigits){
        int lookup = trendCount;        
        if(_override > lookup) { lookup = _override; }            

        string r_string = "";
        
        if(lookup > 20) {
            if(showDigits){
                r_string = fullName + " : " + trendCount.ToString();
                return r_string;                
            } else {
                r_string = fullName;
                return r_string;                
            }
        }

        if(lookup > 7) {
            if(showDigits){
                r_string = TT_TextTools.GetAbbreviatedName(fullName) + " : " + trendCount;
                return r_string;                
            } else {
                r_string = TT_TextTools.GetAbbreviatedName(fullName);
                return r_string;                
            }
        }

        if(3 > lookup) {
            _text.fontSize = 6;
        }
        
        if(showDigits){
            r_string = TT_TextTools.GetInitialedName(fullName) + " : " + trendCount;
            return r_string;                
        } else {
            r_string = TT_TextTools.GetInitialedName(fullName);
            return r_string;                
        }        
    }
    
    IEnumerator WaitToView() {
        _currentName = "";
        _text.text = _currentName;
        _working = false;
        yield return new WaitForSeconds(5.0f);
        _working = true;
        _currentName = SetDisplayString(0, false);
        _text.text = _currentName;
        yield return null;
    }

    IEnumerator Blank() {
        _working = false;
        _currentName = SetDisplayString(0, false);
        _text.text = _currentName;        
        yield return new WaitForSeconds(0.25f);
        _working = true;
        yield return null;
    }

    void Awake(){
        _text = GetComponent<Text>();
        _text.color = new Vector4((183f / 255f), (179f / 255f), (168f / 255f), 0.5f);
        StartCoroutine("WaitToView");        
    }
	
	// Update is called once per frame
	void Update () {
        if(_working) {
            if(_deselectedEvent && !_highlightedEvent) {
                if(_text != null) {
                    StopAllCoroutines();
                    StartCoroutine("Blank");
                }
                _text.color = new Vector4((183f / 255f), (179f / 255f), (168f / 255f), 0.5f);
                _deselectedEvent = false;
            }

            if(_highlightedEvent) {
                if(_text != null) {
                    StopAllCoroutines(); 
                    _currentName = SetDisplayString(22, true);
                    _text.text = _currentName;
                }
                _text.color = new Vector4((183f / 255f), (179f / 255f), (168f / 255f), 1.0f);
                _highlightedEvent = false;
            }
        }        
	}
}

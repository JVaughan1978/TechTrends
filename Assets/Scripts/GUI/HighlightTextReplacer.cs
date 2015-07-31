using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HighlightTextReplacer : MonoBehaviour {

    private Text _thisText;    
    private string _text = "";
    private bool _highlightedEvent = false;
    private bool _deselectedEvent = false;
    
    private bool _sectorEvent = false;
    private Sector _sector = Sector.Automotive;
    private GameObject _dataObject;
    // YES THIS SHOULD REALLY NOT BE HERE.
    private Dictionary<string, int> _data = new Dictionary<string,int>();

    void OnEnable() {
        SelectionReaction.OnSelect += SectorChange;
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= SectorChange;
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
    }

    void SectorChange(Sector sect){
        _sector = sect;
        int i = GetTrendingTopicsDictionary();
        if(i == 0){
            Debug.LogError("Didn't get dictionary");
        }
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
        _dataObject = GameObject.Find("DataObject");
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

    private int GetTrendingTopicsDictionary() {
        if (_dataObject.GetComponent<TechTrendsWrapper>().isLoaded == true) {
            _data = _dataObject.GetComponent<TechTrendsWrapper>().GetSectorToDictionary(0, _sector);
            return 1;
        }
        return 0;
    }
}

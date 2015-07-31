using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeaderTextReplacer : MonoBehaviour {

	public Sector sector;
    private bool _toggle = false;
	private Text _text;

	void OnEnable() {
		SelectionReaction.OnSelect += Select;        
	}

	void OnDisable() {
		SelectionReaction.OnSelect -= Select;
	}

	void Select (Sector sect){
        sector = sect;
        _toggle = !_toggle;
        if(_toggle) {
            _text.text = sector.ToString();
        } else {
            _text.text = "";
        }
	}

	void Start(){
		_text = GetComponent<Text>();		
	}
	
	// Update is called once per frame
	void Update () {	
	}
}

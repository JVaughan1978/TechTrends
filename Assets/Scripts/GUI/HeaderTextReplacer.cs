using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeaderTextReplacer : MonoBehaviour {

	public Sector sector;
	private Text _text;

	void OnEnable() {
		SelectionReaction.OnFocus += Select;
	}

	void OnDisable() {
		SelectionReaction.OnFocus -= Select;
	}

	void Select (Sector sect){
		sector = sect;
		_text.text = sector.ToString();
	}

	void Start(){
		_text = GetComponent<Text>();
		/*
		string dt = "23-07-2015";
		string path = Application.persistentDataPath + "_Data-" + dt + ".xml";

		_text.text = path;*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

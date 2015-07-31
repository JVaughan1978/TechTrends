using UnityEngine;
using System.Collections;

public class BackIcon : MonoBehaviour {

	MeshRenderer _mr;
	BoxCollider _bc;
    SelectionReaction _sr;
	bool _toggle = true;

	void OnEnable(){
		SelectionReaction.OnMode += ModeSwitch;
        SelectionReaction.OnSelect += SectorSwitch;
	}

	void OnDisable(){
		SelectionReaction.OnMode -= ModeSwitch;
        SelectionReaction.OnSelect += SectorSwitch;
	}

	void ModeSwitch(bool check){
		SetIcon(_toggle);
		_toggle = !_toggle;
	}

    void SectorSwitch(Sector sect) {
        _sr.sector = sect;
    }

	void SetIcon(bool check){		
		_mr.enabled = check;
		_bc.enabled = check;
	}

	// Use this for initialization
	void Start () {
		_mr = GetComponent<MeshRenderer>();
		_bc = GetComponent<BoxCollider>();
        _sr = GetComponent<SelectionReaction>();
		SetIcon(false);
	}
	
	// Update is called once per frame
	void Update () {	
	}
}

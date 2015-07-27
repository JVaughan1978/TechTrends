using UnityEngine;
using System.Collections;

public class BackIcon : MonoBehaviour {

	MeshRenderer _mr;
	BoxCollider _bc;
	bool _toggle = true;

	void OnEnable(){
		SelectionReaction.OnMode += ModeSwitch;
	}

	void OnDisable(){
		SelectionReaction.OnMode -= ModeSwitch;
	}

	void ModeSwitch(bool check){
		SetIcon(_toggle);
		_toggle = !_toggle;
	}

	void SetIcon(bool check){
		Debug.Log ("SetIcon " + check);
		_mr.enabled = check;
		_bc.enabled = check;
	}

	// Use this for initialization
	void Start () {
		_mr = GetComponent<MeshRenderer>();
		_bc = GetComponent<BoxCollider>();

		SetIcon(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

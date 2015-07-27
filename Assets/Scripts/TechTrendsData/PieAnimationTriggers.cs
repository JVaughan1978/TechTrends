using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PieAnimationTriggers : MonoBehaviour {

	Animator animator;
	SelectionReaction sr;
	bool _toggle = true;

	void OnEnable() {
		SelectionReaction.OnDeselect += Deselect;
		SelectionReaction.OnFocus += Focus;
		SelectionReaction.OnMode += Mode;
	}

	void OnDisable() {
		SelectionReaction.OnDeselect -= Deselect;
		SelectionReaction.OnFocus -= Focus;
		SelectionReaction.OnMode -= Mode;
	}

	void Deselect(Sector sect) {
		animator.SetBool("isGazed", false);
		animator.SetBool("Selected", false);
	}

	void Focus(Sector sect) {
		if (sr.sector == sect) {
			animator.SetBool ("isGazed", true);
		}
	}

	void Select(Sector sect) {
		if (sr.sector == sect) {
			animator.SetBool("Selected", true);
		}
	}

	void Mode(bool check) {
		animator.SetBool ("DetailView", _toggle);
		_toggle = !_toggle;
	}
	                 
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		sr = GetComponent<SelectionReaction>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

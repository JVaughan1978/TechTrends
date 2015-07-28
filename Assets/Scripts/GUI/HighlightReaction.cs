using UnityEngine;
using System.Collections;

public class HighlightReaction : MonoBehaviour {

	private bool _highlighted = false;

	public delegate void HighlightAction(string name);
	public static event HighlightAction OnHighlight;

	public delegate void DeselectAction(string name);
	public static event DeselectAction OnDeselect;

	public void Highlight(){
		Debug.Log(gameObject.name + " highlighted.");
		_highlighted = true;

		if (OnHighlight != null) {
			OnHighlight(gameObject.name);
		}
	}

	public void Deselect(){
		Debug.Log(gameObject.name + " deselected.");
		_highlighted = false;
		if (OnDeselect != null) {
			OnDeselect(gameObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {	
	}
}

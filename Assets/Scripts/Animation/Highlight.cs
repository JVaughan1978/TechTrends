using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	public float zOffset = 0.25f;
	public Material highlightMaterial;

	private bool _highlighted = false;
	private bool _lasthighlight = false;
	private float _duration = 0.5f;
	private float _time = 0;
	private Vector3 _originalPos = Vector3.zero;
	private Vector3 _highlightedPos = Vector3.zero;
	private Material _originalMaterial;

	void OnEnable(){
		HighlightReaction.OnHighlight += Highlighted;
		HighlightReaction.OnDeselect += Deselected;
	}

	void OnDisable(){
		HighlightReaction.OnHighlight -= Highlighted;
		HighlightReaction.OnDeselect -= Deselected;
	}

	void Highlighted(string broadcastName) {
		if (gameObject.name == broadcastName) {
			//stubby town
		}
	}

	void Deselected(string broadcastName) {
		if (gameObject.name == broadcastName) {
			//stubby town
		}
	}
	
	// Update is called once per frame
	void Update () {
		//stubby town
	}
}

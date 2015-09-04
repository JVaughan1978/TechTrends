using UnityEngine;
using System.Collections;

public class HighlightReaction : MonoBehaviour {

    public bool highlighted = false;

	public delegate void HighlightAction(string name);
	public static event HighlightAction OnHighlight;

	public delegate void DeselectAction(string name);
	public static event DeselectAction OnDeselect;

    public delegate void CoolDownAction(float time);
    public static event CoolDownAction OnCoolDown;    

	public void Highlight(){
        Debug.Log(gameObject.name + " highlighted.");
		highlighted = true;

		if (OnHighlight != null) {
		   OnHighlight(gameObject.name);
		}        
	}

	public void Deselect(){
		Debug.Log(gameObject.name + " deselected.");
		highlighted = false;
		if (OnDeselect != null) {
			OnDeselect(gameObject.name);
		}
	}

    public void CoolDown(float time) {
        if(OnCoolDown != null) {
            OnCoolDown(time);
        }
    }
	
	// Update is called once per frame
	void Update () {	
	}
}

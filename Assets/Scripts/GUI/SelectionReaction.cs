using UnityEngine;
using System.Collections;

//Parent Class used to handled objects selected by VR Gaze
public class SelectionReaction : MonoBehaviour {

    private bool focused = false;    
    private bool selected = false;
    public float timeToSelected = 3f; // should get this from somewhere else...    
    private float _time = 0;

    public delegate void FocusAction();
    public static event FocusAction OnFocus;

    public delegate void SelectAction();
    public static event SelectAction OnSelect;

    public delegate void DeselectAction();
    public static event DeselectAction OnDeselect;

	private void Selected() {                
        if (OnSelect != null) {
            OnSelect();
        }
	}

	public void Deselected() {        
		Reset();

        if (OnDeselect != null) 
            OnDeselect();       
	}

	public void InFocus() {
        if (focused) {
            if (!selected) { 
                _time += Time.deltaTime;
                if (_time > timeToSelected) {
                    selected = true;
                    Selected();
                }
            }
        } else {
            focused = true;
            if(OnFocus != null){
                OnFocus();               
            }
        }
	}

    void Reset() {
        _time = 0;
        focused = false;
        selected = false;
    }

    void Update() {
    }
}
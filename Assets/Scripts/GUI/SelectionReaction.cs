using UnityEngine;
using System.Collections;

//Parent Class used to handled objects selected by VR Gaze
public class SelectionReaction : MonoBehaviour {

    private bool focused = false;    
    private bool selected = false;
	public bool mode = true;
    public float timeToSelected = 3f; // should get this from somewhere else...
    public Sector sector;
    private float _time = 0;

    public delegate void FocusAction(Sector sect);
    public static event FocusAction OnFocus;

    public delegate void SelectAction(Sector sect);
    public static event SelectAction OnSelect;

    public delegate void DeselectAction(Sector sect);
    public static event DeselectAction OnDeselect;

	public delegate void ModeAction(bool check);
	public static event ModeAction OnMode;

    void OnEnable() {                
		SelectionReaction.OnMode += ModeSwitch;
    }

    void OnDisable() {                
		SelectionReaction.OnMode -= ModeSwitch;
    }       

	void ModeSwitch(bool check){
		mode = !check;
	}

	private void Selected() {                
        if (OnSelect != null) {
            OnSelect(sector);
        }
		if (OnMode != null) {
			OnMode(mode);
		}
	}

	public void Deselected() {        
		Reset();

        if (OnDeselect != null) 
            OnDeselect(sector);       
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
                OnFocus(sector);               
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
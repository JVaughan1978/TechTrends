using UnityEngine;
using System.Collections;

public class GazeManager : MonoBehaviour {

	private int gazeDistance;
	private int scannableLayerMask;
	private Ray ray;
    private bool _casting = true;
    
    private SelectionReaction selectionReaction;
	private HighlightReaction highlightReaction;
	
    void OnEnable(){
        SelectionReaction.OnSelect += TimeOut;
    }

    void OnDisable() {
        SelectionReaction.OnSelect -= TimeOut;
    }

    IEnumerator CoolDown() {
        _casting = false;
        yield return new WaitForSeconds(3.0f);
        _casting = true;
    }

    void TimeOut(Sector sect) {
        StartCoroutine("CoolDown");
    }

	void Start(){
		gazeDistance       = 100;
		scannableLayerMask = 1 << 8;
	}

	void Update () {
        if (_casting) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position,
                                 transform.rotation * Vector3.forward, out hit, gazeDistance, scannableLayerMask)) {
            } else {
                if (selectionReaction != null) {                    
                    selectionReaction.Deselected();
                    selectionReaction = null;
                }
				if (highlightReaction != null) {                    
					highlightReaction.Deselect();
					highlightReaction = null;
				}
            }

            if (hit.collider != null) {
                CheckGazedObject(hit);
            }

            Debug.DrawRay(transform.position, transform.rotation * Vector3.forward, Color.green);
        }
	} 

	void CheckGazedObject(RaycastHit hit) {
		selectionReaction = hit.collider.gameObject.GetComponent<SelectionReaction>();
        if (selectionReaction != null) { 
            selectionReaction.InFocus(); 
        }		
		highlightReaction = hit.collider.gameObject.GetComponent<HighlightReaction>();
		if (highlightReaction != null) { 
			highlightReaction.Highlight(); 
		}
	}
}

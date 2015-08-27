using UnityEngine;
using System.Collections;

public class WormholeRotation : MonoBehaviour {

    public float rotationSpeed = 3f;
    public float rotationMinSpeed = 0f;
    public float rotationMaxSpeed = 3f;
    public float speedDelta = 0.1f;

    private bool _focused = false;
   
    void OnEnable(){
        SelectionReaction.OnFocus += Focus;
        SelectionReaction.OnDeselect += Unfocus;
    }

    void OnDisable(){
        SelectionReaction.OnFocus -= Focus;
        SelectionReaction.OnDeselect -= Unfocus;
    }

    void Focus() {
        Debug.Log("Wormhole slowing.");
        _focused = true;
    }

    void Unfocus() {
        _focused = false;
        Debug.Log("Wormhole speeding back up.");        
    }

    void RotationDelta() {
        if(_focused) {
            rotationSpeed -= speedDelta;// *Time.deltaTime;
        } else {
            rotationSpeed += speedDelta;// *Time.deltaTime;
        }
        rotationSpeed = Mathf.Clamp(rotationSpeed, rotationMinSpeed, rotationMaxSpeed);
    }

    // Update is called once per frame
    void Update() {
        RotationDelta();
        float currRot = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, currRot, Space.Self);
    }
}

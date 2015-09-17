using UnityEngine;
using System.Collections;

public class SineSquashScale : MonoBehaviour {

    float sineAmount = 0;
    float cosAmount = 0;
    public float speed = 2.0f;
    public float xScalar = 0.1f;
    public float yScalar = 0.125f;
    Vector3 initialScale = Vector3.one;

    void Start() {
        initialScale = transform.localScale;
    }   
 
    void Update() {
        sineAmount = (Mathf.Sin(Time.time * speed)) * xScalar;//+ 1.0f) / 2.0f; //normalized
        cosAmount = (Mathf.Cos(Time.time * speed)) * yScalar;// + 1.0f) / 2.0f; //normalized
        transform.localScale = new Vector3 ((initialScale.x-sineAmount), (initialScale.y-cosAmount), initialScale.z);
    }
}

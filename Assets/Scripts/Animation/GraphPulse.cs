using UnityEngine;
using System.Collections;

public class GraphPulse : MonoBehaviour {

    public float duration = 6.0f;
    	
    void Update() {
        
        Vector3 firstPos = new Vector3(1f, 1f, 1f);
        Vector3 lastPos = new Vector3(1.5f, 1f, 1f);
        float lerp = Mathf.PingPong(Time.time, duration) / duration;

        transform.localScale = Vector3.Lerp(firstPos, lastPos, lerp);
    }
}

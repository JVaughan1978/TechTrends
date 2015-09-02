using UnityEngine;
using System.Collections;

public class DeScaler : MonoBehaviour {

    public GameObject scaleRoot;
    public float scaler;

	void Update () {
	    float xScale = (1.0f / scaleRoot.transform.localScale.x  ) * scaler;
        transform.localScale = new Vector3(xScale, 1.0f, 1.0f);
	}
}

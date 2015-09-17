using UnityEngine;
using System.Collections;

public class iOS_Mesh : MonoBehaviour {

    // Use this for initialization
    void Start() {
#if !UNITY_IOS
        Destroy(gameObject);
#endif
    }

    // Update is called once per frame
    void Update() {

    }
}

using UnityEngine;
using System.Collections;

public class WP8_Mesh : MonoBehaviour {

    // Use this for initialization
    void Start() {
#if !UNITY_WP8 || !UNITY_WP8_1
        Destroy(gameObject);
#endif
    }

    // Update is called once per frame
    void Update() {

    }
}

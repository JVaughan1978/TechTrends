using UnityEngine;
using System.Collections;

public class Android_Mesh : MonoBehaviour {

    void Start() {
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif
    }

    void Update() {

    }
}

﻿using UnityEngine;
using System.Collections;

public class Editor_Mesh : MonoBehaviour {

    // Use this for initialization
    void Start() {
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif
    }

    // Update is called once per frame
    void Update() {

    }
}

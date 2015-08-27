﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class PinTextToGameObject : MonoBehaviour {

	public GameObject pinnedObject = null;
    public Vector3 offset = Vector3.zero;
    	
	void Start () {				
	}	
	
	void Update () { 
        transform.localPosition = new Vector3(pinnedObject.transform.position.x, pinnedObject.transform.position.y,
                                                             pinnedObject.transform.position.z);

        transform.Translate(offset.x, offset.y, offset.z, Space.Self);
	}
}

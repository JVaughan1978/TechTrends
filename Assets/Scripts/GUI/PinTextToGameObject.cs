using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]
public class PinTextToGameObject : MonoBehaviour {

	public GameObject pinnedObject;
    public Vector3 offset = Vector3.zero;

	// Use this for initialization
	void Start () {				
	}
	
	// Update is called once per frame
	void Update () {        		
        Vector3 pinnedObjectPosition = new Vector3(pinnedObject.transform.position.x, pinnedObject.transform.position.y,
                                                             pinnedObject.transform.position.z);       
		
        transform.localPosition = new Vector3(pinnedObject.transform.position.x, pinnedObject.transform.position.y,
                                                             pinnedObject.transform.position.z);

        transform.Translate(offset.x, offset.y, offset.z, Space.Self);
	}
}

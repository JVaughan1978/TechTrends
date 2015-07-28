using UnityEngine;
using System.Collections;

public class CollisionHelper : MonoBehaviour {

	MeshCollider _mc;
	MeshFilter _mf;

	void Start(){
		_mc = GetComponent<MeshCollider>();
		_mf = GetComponent<MeshFilter>();
	}

	void Update() {
		if (_mc != null && _mf != null) {
			if (_mc.sharedMesh == null) {
				_mc.sharedMesh = _mf.sharedMesh;
				if (_mc.sharedMesh != null) {
					Destroy (this);
				}
			}	
		}
	}
}

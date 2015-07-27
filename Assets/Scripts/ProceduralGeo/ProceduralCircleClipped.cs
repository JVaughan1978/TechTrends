using UnityEngine;
using System.Collections;

public class ProceduralCircleClipped : MonoBehaviour {

	public bool jDebug = true; 

	[Range(6,250)]
	public int totalVerts = 24;
	private int _lastTotalVerts = 0;

	[Range(6,250)]
	public int segmentVerts = 12;
	private int _lastSegmentVerts = 0;

	[Range(0f,1.0f)]
	public float innerSegmentPercent = .75f;
	private float _lastSegmentPercent = 0f;

	public Material material;
	
	// Use this for initialization
	void Start() {
		_lastTotalVerts = totalVerts;
		_lastSegmentVerts = segmentVerts;
		_lastSegmentPercent = innerSegmentPercent;
		
		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.sharedMesh = MakeClippedCircle(totalVerts, segmentVerts, innerSegmentPercent);
		
		MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
		mr.material = material;	
	}
	
	Mesh MakeClippedCircle(int numVerts, int lastVert, float innerScale){

		//innerScale is clamped 0-1 and currently we don't support non-uniform scale
		innerScale = Mathf.Clamp01(innerScale);

		//cleaning up a potential race case with lastVerts being greater than numVerts;
		if (lastVert > numVerts) {
			lastVert = numVerts;
			Debug.Log ("Verts are equal");
		}

		if (lastVert < 2) {
			return null;
		}

		Mesh circle = new Mesh();

		Vector3[] verts = new Vector3[lastVert];
		Vector3[] vertsInner = new Vector3[lastVert];
		Vector3[] vertsFinal = new Vector3[(2 * lastVert)];

		Vector2[] uvs = new Vector2[lastVert];
		Vector2[] uvsInner = new Vector2[lastVert];
		Vector2[] uvsFinal = new Vector2[(2 * lastVert)];

		int triCount =  (lastVert - 1) * 2;

		int[] tris = new int[triCount * 3];

		//Since this is clipped the number of edges are defined by last vert, but the angle covered 
		//is defined by numVerts, which must always be greater than or equal to lastVerts.
		float angle = 360.0f / (float)(numVerts - 1);  
		
		//create each vert
		for (int i = 0; i < lastVert; i++) {
			verts[i] = Quaternion.AngleAxis(angle * (float)(i), Vector3.back) * Vector3.up;
			float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
			float normedVertical = (verts[i].y + 1.0f) * 0.5f;  //one of these is probably VERY wrong
			uvs[i] = new Vector2(normedHorizontal, normedVertical);
		}

		//then copy it to the inner verts and scale them.
		for (int i = 0; i < vertsInner.Length; i++) {
			vertsInner[i] = new Vector3(verts[i].x, verts[i].y, verts[i].z);
			vertsInner[i] = Vector3.Scale(new Vector3(vertsInner[i].x, vertsInner[i].y, vertsInner[i].z), new Vector3(innerScale, innerScale, 1.0f));
		}

		for (int i = 0; i < vertsInner.Length; i++) {
			uvsInner[i] = new Vector2(uvs[i].x, uvs[i].y);
			uvsInner[i] = Vector2.Scale(new Vector2(uvsInner[i].x, uvsInner[i].y), new Vector2(innerScale, innerScale));
		}

		//then I have to copy these over and interleave the values
		for (int i = 0; i < vertsFinal.Length; i++) {
			if( i % 2 == 0 ) {
				vertsFinal[i] = vertsInner[(i / 2)];
			} else {
				vertsFinal[i] = verts[(i / 2)];
			}
		}

		if(jDebug) {
			for (int i = 1; i < vertsFinal.Length; i++) {
				//Debug.Log ("vertsFinal.Length = " + vertsFinal.Length);
				if (i == 1)
					Debug.Log (vertsFinal [0]);
				//Debug.Log (vertsFinal[i] + " " + i);
				//Debug.DrawLine (new Vector3 (vertsFinal[i - 1].x, vertsFinal[i - 1].y, vertsFinal [i - 1].z), new Vector3 (vertsFinal[i].x, vertsFinal[i].y, vertsFinal[i].z), Color.green, 15, false);
			}
		}

		for (int i = 0; i < uvsFinal.Length; i++) {
			if( i % 2 == 0 ) {
				uvsFinal[i] = uvsInner[(i / 2)];
			} else {
				uvsFinal[i] = uvs[(i / 2)];
			}
		}
		
		//create the triangles, a very different algothim with the clipped circle

		for (int i = 0; i < triCount; i++) {
			int index = i * 3;
			if( i % 2 == 0 ){
				tris[index + 0] = i + 0;
				tris[index + 1] = i + 1;
				tris[index + 2] = i + 2;
			} else {
				tris[index + 0] = i ;
				tris[index + 1] = i + 2;
				tris[index + 2] = i + 1;
			}
		}

		// need to write this into the mesh object...
		circle.vertices = vertsFinal;
		circle.uv = uvsFinal;
		circle.triangles = tris;
		circle.normals = vertsFinal;
		
		circle.RecalculateBounds ();
		circle.RecalculateNormals ();
		
		return circle;
	}
	
	
	
	// Update is called once per frame
	void Update() {
		if (_lastTotalVerts != totalVerts || 
		    _lastSegmentVerts != segmentVerts ||
		    _lastSegmentPercent != innerSegmentPercent ) {
			this.GetComponent<MeshFilter>().mesh = MakeClippedCircle(totalVerts, segmentVerts, innerSegmentPercent);
		}
		
		_lastSegmentPercent = innerSegmentPercent;
		_lastSegmentVerts = segmentVerts;
		_lastTotalVerts = totalVerts;		
	}
}

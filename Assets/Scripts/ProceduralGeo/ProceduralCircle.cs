using UnityEngine;
using System.Collections;

public class ProceduralCircle : MonoBehaviour {

	[Range(6,250)]
	public int circleVerts = 24;

	private float _radius = 0.0f;
	public float Radius {
		get {
			return _radius;
		}
		set {
			_radius = value;
			_area = Mathf.PI * (_radius * _radius);
			transform.localScale = new Vector3(_radius, _radius, _radius);
		}
	}

	//public float circArea = 1.0f;
	private float _area = 0.0f;
	//private float _lastArea = 0.0f;
	public float Area {
		get{
			return _radius;
		}
		set{
			_area = value;
			_radius = Mathf.Sqrt(_area / Mathf.PI);
			transform.localScale = new Vector3(_radius, _radius, _radius);
		}
	}

	private int _lastCircleVerts = 0;
	public Material material;

	// Use this for initialization
	void Start() {
		_lastCircleVerts = circleVerts;

		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.sharedMesh = MakeCircle (circleVerts);
		
		MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
		mr.material = material;	
	}

	Mesh MakeCircle(int numVerts){
		Mesh circle = new Mesh ();
		Vector3[] verts = new Vector3[numVerts];  
		Vector2[] uvs = new Vector2[numVerts];  
		int[] tris = new int[(numVerts * 3)];  
		
		// The first vert is in the center of the triangle  
		verts[0] = Vector3.zero;  
		uvs[0] = new Vector2(0.5f, 0.5f);  
		float angle = 360.0f / (float)(numVerts - 1);  

		//create each vert
		for (int i = 1; i < numVerts; ++i) {
			verts[i] = Quaternion.AngleAxis(angle * (float)(i-1), Vector3.back) * Vector3.up;
			float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
			float normedVertical = (verts[i].y + 1.0f) * 0.5f;  //one of these is probably VERY wrong
			uvs[i] = new Vector2(normedHorizontal, normedVertical);
		}

		//create the triangles
		for (int i = 0; i + 2 < numVerts; ++i) {
			int index = i * 3;
			tris[index + 0] = 0;
			tris[index + 1] = i + 1;
			tris[index + 2] = i + 2;
		}

		//Building the last vert inline
		int lastTriangleIndex = tris.Length - 3;
		tris[lastTriangleIndex + 0] = 0;
		tris[lastTriangleIndex + 1] = numVerts - 1;
		tris[lastTriangleIndex + 2] = 1;

		// need to write this into the mesh object...
		circle.vertices = verts;
		circle.uv = uvs;
		circle.triangles = tris;
		circle.normals = verts;

		circle.RecalculateBounds ();
		circle.RecalculateNormals ();

		return circle;
	}
	
	// Update is called once per frame
	void Update() {

		if (_lastCircleVerts != circleVerts) {
			this.GetComponent<MeshFilter>().mesh = MakeCircle(circleVerts);
		}

		_lastCircleVerts = circleVerts;
		//_lastArea = _area;
	}
}

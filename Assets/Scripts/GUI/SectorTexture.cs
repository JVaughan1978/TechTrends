using UnityEngine;
using System.Collections;

public class SectorTexture : MonoBehaviour {

	public Sector sector = Sector.Automotive;
	private Sector _lastSector = Sector.Automotive;

	public Material[] mats = new Material[0];

	void SetMaterial(){
		Renderer r = gameObject.GetComponent<Renderer>();
		r.material = mats[(int)sector];
	}
	
	void Start () {
		SetMaterial();
	}
		
	void Update () {
		if (_lastSector != sector) {
			SetMaterial();
		}

		_lastSector = sector;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieChart : MonoBehaviour {

	private Dictionary<string, int> trendingTopics = new Dictionary<string, int>();
	public List<GameObject> pieSlices = new List<GameObject>();
	public List<Material> pieColors = new List<Material>();
    private GameObject backdrop;

	public float zOffset = 0f;
	public float backdropOffset = 0.05f;

    private GameObject _dataObject;
    public Sector sector = Sector.Automotive;

	void OnEnable() {        
		SelectionReaction.OnSelect += ModeSwitch;
	}
	
	void OnDisable() {
		SelectionReaction.OnSelect -= ModeSwitch;
	}	

	void ModeSwitch(Sector sect) {
        
		if (pieSlices.Count == 0) {
			PieChartInit (sect);
		} else {
			DestroyAllChildren ();
		}
	}

    void Init() {
        if (pieSlices.Count == 0) {
            CreateSlices(trendingTopics);
        }
    }

    void PieChartInit(Sector sect) {
        sector = sect;
        GetTrendingTopicsDictionary();
        Invoke("Init", 1.0f);
    }

	// Use this for initialization
	void Start () {
        _dataObject = GameObject.Find("DataObject");
        int newTTD = GetTrendingTopicsDictionary();
        if (newTTD == 0) {
            Debug.Log("NO Trending Topics!");
            this.enabled = false;
        }
	}

    private int GetTrendingTopicsDictionary() {
        if (_dataObject.GetComponent<TechTrendsWrapper>().isLoaded == true) {
            trendingTopics = _dataObject.GetComponent<TechTrendsWrapper>().GetSectorToTruncatedDictionary(0, sector, 9);
            return 1;
        }
        return 0;
    }
		
	//THIS IS PROBABLY GOING TO NEED TO MOVE A DATA HANDLING CLASS
	int GetTotalOfEntryValues(Dictionary<string, int> inputDictionary){
		int total = 0;
		foreach (KeyValuePair<string, int> entry in inputDictionary) {
			total += entry.Value;
		}
		return total;
	}

	void CreateSlices(Dictionary<string, int> inputDictionary){
		int _currentStartPosition = 0; // using to iterate over the picees and rotate them;
		int _totalVerts = GetTotalOfEntryValues(inputDictionary);
		float _totalVertsFloat = (float)_totalVerts;
		int iterator = 0;

		backdrop = new GameObject();
		backdrop.name = "Backdrop";
		backdrop.transform.SetParent (this.transform);
        backdrop.transform.localEulerAngles = Vector3.zero;		
        backdrop.transform.localPosition = new Vector3(0, 0, backdropOffset);

		ProceduralCircle pc = backdrop.AddComponent<ProceduralCircle>();
		pc.circleVerts = _totalVerts;
		pc.material = Resources.Load("Materials/KPMG_DataColors/Backdrop") as Material;

		foreach (KeyValuePair<string, int> entry in inputDictionary) {
			GameObject go = new GameObject();
			go.name = entry.Key;

			ProceduralCircleClipped pcc = go.AddComponent<ProceduralCircleClipped>();
			pcc.totalVerts = _totalVerts;
			pcc.segmentVerts = entry.Value+1;
			pcc.material = pieColors[iterator];
			pcc.Init();
			
			go.transform.SetParent(this.transform);
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localPosition = new Vector3(0, 0, (float)iterator *  zOffset);
			pieSlices.Add(go);

			go.AddComponent<HighlightReaction>();
			go.AddComponent<MeshCollider>();
			go.AddComponent<CollisionHelper>();
			go.layer = 8;
            go.AddComponent<Highlight>();
            
            if(iterator != 0) { //no sense rotating the first element
                float targetZRotation = -1.0f - (((float)_currentStartPosition / _totalVertsFloat) * 360.0f);
                RotateTo r2 = go.AddComponent<RotateTo>();
                r2.endRotation = targetZRotation;
            }

            _currentStartPosition += entry.Value;
            iterator++;

		}		
	}

    void DestroyAllChildren() {
		foreach(GameObject pie in pieSlices){
				Destroy(pie);
		}
		pieSlices.Clear();
		Destroy(backdrop);
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
	}    

	void Update () {
	}
}

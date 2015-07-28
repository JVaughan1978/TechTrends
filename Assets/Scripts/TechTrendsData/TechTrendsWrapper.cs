using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Sector {
	Automotive,
	ConsumerGoods,
	Energy,
	FinancialServices,
	HealthCare,
	MediaAndEntertainment,
	RetailWholesale,
	Transportation
}

public class TechTrendsWrapper : MonoBehaviour {

	TechTrendsData[] ttd = new TechTrendsData[1];
	public string[] loadPaths = new string[1];
	public string date = "";

	[System.Serializable]
	public class ttwDataPoint {
		public string fullName;
		public float trendValue;
	}

	[System.Serializable]
	public class ttwDataSet {
		public Sector sector;
		public List<ttwDataPoint> dataPoints = new List<ttwDataPoint>();
	}

	public ttwDataSet[] totalData = new ttwDataSet[8];
	public bool isLoaded = false;

	public void SetTechTrendsDataFromInspector(int arrayIndex){
		if (ttd[arrayIndex] != null) {
			ttd[arrayIndex].date = DateTime.Now.ToString("dd-MM-yyyy");
			ttd[arrayIndex].ClearAll();
			for(int i = 0; i < totalData.Length; i++){
				ttd[arrayIndex].totalData[i].sector = totalData[i].sector;
				for(int j = 0; j < totalData[i].dataPoints.Count; j++){
					ttd[arrayIndex].AddNewDataPoint(i, totalData[i].dataPoints[j].fullName, totalData[i].dataPoints[j].trendValue);
				}
			}
		}		  
	}

	public Dictionary<string, int> GetSectorToDictionary(int index, Sector sector){ //Spits out every element, used for the bubble flow
		Dictionary<string, int> d1 = new Dictionary<string, int>();
		for (int i = 0; i < ttd[index].totalData[(int)sector].dataPoints.Count; i++) {
			string key = ttd[index].totalData[(int)sector].dataPoints[i].fullName;
			int val = (int)ttd[index].totalData[(int)sector].dataPoints[i].trendValue;
			d1.Add(key, val);
		}
		return d1;
	}
	
	public Dictionary<string, int> GetSectorToTruncatedDictionary(int index, Sector sector, int truncate){ // spits out a certain number of items into a dictionary then compiles the rest into an element called "Others"
		Dictionary<string, int> d1 = new Dictionary<string, int>();
		float otherValue = 0f;
		for (int i = 0; i < ttd[index].totalData[(int)sector].dataPoints.Count; i++) {
			if (i < truncate) {
				string key = ttd[index].totalData[(int)sector].dataPoints[i].fullName;
				int val = (int)ttd[index].totalData[(int)sector].dataPoints[i].trendValue;
				d1.Add(key, val);
			} else {
				string key = "Others";
				otherValue += ttd[index].totalData[(int)sector].dataPoints[i].trendValue;
				if(i == ttd[index].totalData[(int)sector].dataPoints.Count-1) {
					d1.Add(key,(int)otherValue);
				}
			}
		}
		return d1;
	}

	void Start() {

		TechTrendsData[] newTTD = new TechTrendsData[loadPaths.Length];
		for(int i = 0; i < loadPaths.Length; i++) {
			newTTD[i] = TechTrendsData.Load(loadPaths[i]);
		}

		ttd = newTTD;
		isLoaded = true;
	}

	// Update is called once per frame
	void Update () {/*
		if (Input.GetKeyUp (KeyCode.F3)) {
			SetTechTrendsDataFromInspector(0);
		}

		if (Input.GetKeyUp (KeyCode.F5)) {
			string dt = DateTime.Now.ToString("dd-MM-yyyy");
			string path = Application.persistentDataPath + "_Data-" + dt + ".xml";
			ttd[0].Save(path);
			Debug.Log ("XML written at " + path);
		}

		if (Input.GetKeyUp (KeyCode.F9)) {
			TechTrendsData[] newTTD = new TechTrendsData[loadPaths.Length];
			for(int i = 0; i < loadPaths.Length; i++) {
				newTTD[i] = TechTrendsData.Load(loadPaths[i]);
			}
			ttd = newTTD;
		}

		if (Input.GetKeyUp (KeyCode.F10)) {
			if(ttd[0] != null){
				Debug.Log ("Got a data object");
				if(ttd[0].totalData[0].dataPoints != null) {
					Debug.Log ("Got " + ttd[0].totalData[0].dataPoints.Count + " automotive data");					
				}
			}
		}*/
		//Time to really automate this stuff
	}
}

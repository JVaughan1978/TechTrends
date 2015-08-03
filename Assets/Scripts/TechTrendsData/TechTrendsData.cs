using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("TechTrendsData")]
public class TechTrendsData {	
	public Sector currentSector = Sector.Automotive;
	public string date = "";

	public struct DataPoint {
		public string fullName;
		public float trendValue;

		public DataPoint(string fn, float tv){
			this.fullName = fn;
			this.trendValue = tv;
		}
	}	

	public struct DataSet {
		public Sector sector;
		public List<DataPoint> dataPoints;

		public DataSet(Sector sectorValue) {
			sector = sectorValue;
			dataPoints = new List<DataPoint>();
		}
	}
	
	public DataSet[] totalData = new DataSet[0];
    private TextAsset text = "";
    private WWW xmlData;

	public void ClearAll(){
		foreach (DataSet ds in totalData) {
			if(ds.dataPoints != null){
				ds.dataPoints.Clear();
			}
		}
	}

	public void AddNewDataPoint(int arrayIndex, string fn, float tv){
		DataPoint newDP = new DataPoint(fn, tv);
		totalData[arrayIndex].dataPoints.Add(newDP);
	}

	public void Save(string path){	
		XmlSerializer serializer = new XmlSerializer(typeof(TechTrendsData));
		using(FileStream stream = new FileStream(path, FileMode.Create)) {
			serializer.Serialize(stream, this);
		}
	}

    IEnumerator FetchXML(string url) {
        WWW newXML = new WWW(url);
        yield return newXML;
        text = (TextAsset)newXML;
    }

	public static TechTrendsData Load(string path){
		XmlDocument xmldoc = new XmlDocument();
        //Here's where I have to switch this over to WWW and url based fetches
        //TextAsset textAsset = (TextAsset)Resources.Load(path, typeof(TextAsset));
		//xmldoc.LoadXml (textAsset.text);

		XmlSerializer serializer = new XmlSerializer(typeof(TechTrendsData));		
		using (XmlReader reader = new XmlNodeReader(xmldoc)) {
			Debug.Log ("Reading data from " + path);
			return serializer.Deserialize(reader) as TechTrendsData;
		}
	}

	public TechTrendsData(){
		//going to manually create and instance DataSets in Constructor
		DataSet[] nds = new DataSet[8];
		for (int i = 0; i < 8; i++) {
			DataSet ds = new DataSet(Sector.Automotive);
			nds[i] = ds;
		}
		totalData = nds;
	}
}

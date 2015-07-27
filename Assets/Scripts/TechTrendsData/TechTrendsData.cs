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

	public static TechTrendsData Load(string path){
		TextAsset textAsset = (TextAsset) Resources.Load("TechTrends_Data-23-07-2015");  
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( textAsset.text );

		XmlSerializer serializer = new XmlSerializer(typeof(TechTrendsData));
		/*using(FileStream stream = new FileStream(path, FileMode.Open)) {
			return serializer.Deserialize(stream) as TechTrendsData;
		}Ye olden windowed way...*/

		using (XmlReader reader = new XmlNodeReader(xmldoc)) {
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

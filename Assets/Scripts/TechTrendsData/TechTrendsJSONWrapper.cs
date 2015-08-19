using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;

public class TechTrendsJSONWrapper : MonoBehaviour {
    
    public string url = "http://owlcdn.net/kpmg/us/data.json";
    public string filename = "data.json";
    public TextAsset textAsset = null;    
    public RootObject data;
    public static bool JSON_LOAD_COMPLETE = false;

    #region [KPMG_DATA]

    public class Child4 {
        public string name { get; set; }
        public double size { get; set; }
    }

    public class Child3 {
        public string name { get; set; }
        public List<Child4> children { get; set; }
    }

    public class Child2 {
        public string name { get; set; }
        public List<Child3> children { get; set; }
    }

    public class Child {
        public string name { get; set; }
        public List<Child2> children { get; set; }
    }

    public class Glyphicons {
        public List<string> sectors { get; set; }
        public List<object> topics { get; set; }
    }

    public class Datum {
        public string date { get; set; }
        public double size { get; set; }
    }

    public class TimeSery {
        public string topic { get; set; }
        public List<Datum> data { get; set; }
    }

    public class WalkNtalk {
        public string trend { get; set; }
        public int walk { get; set; }
        public int talk { get; set; }
    }

    public class RootObject {
        public int lastUpdate { get; set; }
        public string name { get; set; }
        public List<Child> children { get; set; }
        public Glyphicons glyphicons { get; set; }
        public List<TimeSery> timeSeries { get; set; }
        public List<WalkNtalk> walkNtalk { get; set; }
    }
    #endregion [KPMG_DATA]

    IEnumerator WWWGet() {
        Debug.Log("Attempting to get Tech Trends Data.");
        WWW www = new WWW(url);
        yield return www;
        ConvertStringToTextAsset(www.text);
        PopulateData();
        Debug.Log("Data Get!");
    }    

    void ConvertStringToTextAsset(string text) {
        string formatted = text.Remove(0, 9);
        int charLength = formatted.Length;
        formatted = formatted.Remove(charLength - 1, 1);        
        File.WriteAllText(Application.dataPath + "/Resources/" + filename, formatted);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        textAsset = (TextAsset)Resources.Load("data");        
    }
   
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
        // Unix timestamp is seconds past epoch
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    void PopulateData() {
        data = JsonConvert.DeserializeObject<RootObject>(textAsset.text);
        DateTime dateTime = UnixTimeStampToDateTime((double)data.lastUpdate);
        Debug.Log(dateTime.ToString() + " just happened");
        JSON_LOAD_COMPLETE = true;
    }
    
    public Dictionary<string, int> GetJSONDictionary(Sector sect) {        
        List<Child> top = data.children;
        List<Child2> second = top[0].children;
        List<Child3> third = second[0].children;
        List<Child4> fourth = third[(int)sect].children;

        fourth.Sort((x, y) => x.size.CompareTo(y.size));
        fourth.Reverse();

        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        foreach(Child4 node in fourth) {
            tempDict.Add(node.name, (int)node.size);
        }
        
        return tempDict;
    }

    public Dictionary<string, int> GetTruncatedJSONDictionary(Sector sect, int truncate) {
        List<Child> top = data.children;
        List<Child2> second = top[0].children;
        List<Child3> third = second[0].children;
        List<Child4> fourth = third[(int)sect].children;

        fourth.Sort((x, y) => x.size.CompareTo(y.size));
        fourth.Reverse();

        Dictionary<string, double> d1 = new Dictionary<string, double>();
		double otherValue = 0;
		for (int i = 0; i < fourth.Count; i++) {
			if (i < truncate) {
				string key = fourth[i].name;
				double val = fourth[i].size;
				d1.Add(key, val);
			} else {
				string key = "Others";
				otherValue += fourth[i].size;
				if(i == fourth.Count-1) {
					d1.Add(key, otherValue);
				}
			}
		}               

        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        foreach(KeyValuePair<string, double> node in d1) {
            tempDict.Add(node.Key, (int)node.Value);
        }
        
        return tempDict;
    }

    void WriteData() {
        string serialized = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath + "/Resources/data_serialized.json", serialized);
    }
    
	// Use this for initialization
	void Start () {
        StartCoroutine("WWWGet");
    }
	
	// Update is called once per frame
	void Update () {
	}
}

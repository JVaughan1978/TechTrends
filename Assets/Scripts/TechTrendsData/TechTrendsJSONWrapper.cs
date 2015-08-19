using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;

public class TechTrendsJSONWrapper : MonoBehaviour {
    
    public string url = "http://owlcdn.net/kpmg/us/data.json";
    public string filename = "data.json";
    public TextAsset textAsset = null;    
    public RootObject data;

    #region [KPMG_DATA]
    public class Child4 {
        public string name { get; set; }
        public double size { get; set; }
    }

    public class Child3 {
        public string name { get; set; }
        public List<Child4> children4 { get; set; }
    }

    public class Child2 {
        public string name { get; set; }
        public List<Child3> children3 { get; set; }
    }

    public class Child {
        public string name { get; set; }
        public List<Child2> children2 { get; set; }
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
        public List<Child> children1 { get; set; }
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
        //Debug.Log(dateTime.ToString() + " just happened");    
    }    
    
	// Use this for initialization
	void Start () {
        StartCoroutine("WWWGet");
    }
	
	// Update is called once per frame
	void Update () {
	}
}

using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TechTrendsJSONWrapper : MonoBehaviour {
    
    public string url = "http://owlcdn.net/kpmg/us/data.json";
    public string filename = "data.json";
    
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
        PopulateData(www.text);
        Invoke("SerializeData", 12.0f);
        Debug.Log("Data Get!");
    }

    void LoadLocalData() {
        TextAsset textAsset = (TextAsset)Resources.Load("data", typeof(TextAsset));
        data = JsonConvert.DeserializeObject<RootObject>(textAsset.text);
        JSON_LOAD_COMPLETE = true;
    }
    

    void PopulateData(string text) {        
        string formatted = text.Remove(0, 9);
        int charLength = formatted.Length;
        formatted = formatted.Remove(charLength - 1, 1);           
        data = JsonConvert.DeserializeObject<RootObject>(formatted);
        JSON_LOAD_COMPLETE = true;
    }

    void SerializeData() {
        if(data != null) {
            string path = Application.persistentDataPath + "/Resources/data.json";
            string serialized = JsonConvert.SerializeObject(data);         
            
            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate)) {
                using(StreamWriter writer = new StreamWriter(fs)) {
                    writer.Write(serialized);
                }
            }   
        }                        
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

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
        // Unix timestamp is seconds past epoch
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    public string GetJSONUpdateTime() {
        string outString = "";
        if(data != null) {            
            DateTime dateTime = UnixTimeStampToDateTime((double)data.lastUpdate);
            outString = dateTime.ToString();            
        } else {
            Debug.LogWarning("data not loaded!");
        }
        return outString;
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

    public string GetHtmlFromUri(string resource) {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try {
            using(HttpWebResponse resp = (HttpWebResponse)req.GetResponse()) {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if(isSuccess) {
                    using(StreamReader reader = new StreamReader(resp.GetResponseStream())) {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach(char ch in cs) {
                            html += ch;
                        }
                    }
                }
            }
        } catch {
            return "";
        }
        return html;
    }
    
	void Start () {
        string HtmlText = GetHtmlFromUri("http://google.com");
        if(HtmlText == "") {
            //No connection
            //load from data.json
            LoadLocalData();
        } else if(!HtmlText.Contains("schema.org/WebPage")) {
            //Redirecting since the beginning of googles html contains that 
            //phrase and it was not found
            LoadLocalData();
        } else {
            //success
            StartCoroutine("WWWGet");
        }        
    }
		
	void Update () {
	}
}

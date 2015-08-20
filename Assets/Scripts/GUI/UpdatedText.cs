using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdatedText : MonoBehaviour {

    private GameObject _dataObject = null;
    private Text _text = null;


    void SetUpdateText() {
        string dateTime = "";
        if(_dataObject.GetComponent<TechTrendsJSONWrapper>() != null) {
            dateTime = _dataObject.GetComponent<TechTrendsJSONWrapper>().GetJSONUpdateTime();
        } else {
            Debug.LogWarning("TechTrendsJSONWrapper does not exist on dataObject.");
        }
        
        string updateText = "Updated at " + dateTime;
        _text.text = updateText;
    }
	
	void Start () {
        _dataObject = GameObject.Find("DataObject");
        _text = GetComponent<Text>();

        if(_dataObject == null) {
            Debug.LogWarning("dataObject is null");
        }

        if(_text == null) {
            Debug.LogWarning("text is null");
        }

        Invoke("SetUpdateText", 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

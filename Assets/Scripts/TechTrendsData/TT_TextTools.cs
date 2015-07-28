using UnityEngine;
using System.Collections;

public class TT_TextTools : MonoBehaviour {

	[System.Serializable]
	public class TT_Text {
		public string fullName;
		public string abbreviated;
		public string initialed;
	}

	//Inspector facing
	public TT_Text[] TT_Dictionary = new TT_Text[0];
	//Data facing
	private static TT_Text[] TT_Internal = new TT_Text[0];

	public static string GetAbbreviatedName(string name){
		foreach (TT_Text text in TT_Internal) {
			if(name == text.fullName){
				return text.abbreviated;
			}
		}
		return null;
	}

	public static string GetInitialedName(string name){
		foreach (TT_Text text in TT_Internal) {
			if(name == text.fullName){
				return text.initialed;
			}
		}
		return null;
	}

	private static TT_TextTools _instance;
	
	public static TT_TextTools instance	{
		get	{
			if(_instance == null)			{
				_instance = GameObject.FindObjectOfType<TT_TextTools>();
				DontDestroyOnLoad(_instance.gameObject);
			}			
			return _instance;
		}
	}
	
	void Awake() {
		if(_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this);
		} else {
			if(this != _instance)
				Destroy(this.gameObject);
		}
		TT_Internal = TT_Dictionary;
	}	

	void Update () {
	
	}
}

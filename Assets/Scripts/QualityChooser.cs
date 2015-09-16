using UnityEngine;
using System.Collections;

public class QualityChooser : MonoBehaviour {
    
    void Start() {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_EDITOR
    Debug.Log("Unity Editor");
    QualitySettings.SetQualityLevel(0, true);
#endif

#if UNITY_IPHONE

    Debug.Log("Iphone");           
    QualitySettings.SetQualityLevel(1, false);
#endif

#if UNITY_ANDROID
    Debug.Log("Android");
    QualitySettings.SetQualityLevel(0, false);
#endif
    }
}

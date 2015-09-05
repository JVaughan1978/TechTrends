using UnityEngine;
using System.Collections;

public class WaveForm : MonoBehaviour {
    AudioSource audio;
    LineRenderer line;

    void Start() {
        audio = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();
    }

    void Update() {
        if(audio.isPlaying) {
            float[] spectrum = audio.GetSpectrumData(256, 0, FFTWindow.BlackmanHarris);
            line.SetVertexCount(255);
            int i = 1;
            while(i < 256) {                                
                Vector3 nextPoint = new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2);
                nextPoint = Vector3.Scale(nextPoint, transform.localScale);
                nextPoint += transform.localPosition;
                line.SetPosition(i - 1, nextPoint);
                i++;
            }
        } else {
            line.SetVertexCount(0);
        }        
    }
}
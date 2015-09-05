using UnityEngine;
using System.Collections;

public class WaveForm : MonoBehaviour {
    AudioSource audio;
    LineRenderer line;
    private bool cued = false;
    
    void OnEnable() {
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
    }

    void OnDisable() {
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
    }

    void StartAudio() {
        audio.Play();
        cued = false;        
    }

    void Highlighted(string name) {
        if(name == transform.parent.gameObject.name) {
            if(!audio.isPlaying && !cued){
                Invoke("StartAudio", 0.3f);
                cued = true;
            }            
        }
    }

    void Deselected(string name) {        
        if(name == transform.parent.gameObject.name) {
            if(audio.isPlaying) {
                audio.Stop();
                cued = false;
            }            
        }
    }   
 
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
                Vector3 nextPoint = new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, (Mathf.Log(spectrum[i - 1])*0.5f));
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
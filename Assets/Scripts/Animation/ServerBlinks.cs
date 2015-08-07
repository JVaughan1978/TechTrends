using UnityEngine;
using System.Collections;

public class ServerBlinks : MonoBehaviour {

    public float speed = 0.25f;
    private float time = 0;
    private int _iterator = 0;

    public Material[] blinks = new Material[4];
    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(time > speed) {
            for(int i = 0; i < 4; i++) {
                if(i == _iterator) {
                    Color col = new Color(1.0f, 1.0f, 1.0f);
                    blinks[i].SetColor("_TintColor", col);
                } else if(i == _iterator -1) {
                    Color col = new Color(0.5f, 0.5f, 0.5f);
                    blinks[i].SetColor("_TintColor", col);
                } else {
                    Color col = new Color(0f, 0f, 0f);
                    blinks[i].SetColor("_TintColor", col);
                }              
            }

            rend.materials = blinks;

            time = 0;

            _iterator++;
            if(_iterator > 3) { _iterator = 0; }
        }

        time += Time.deltaTime;	
	}
}

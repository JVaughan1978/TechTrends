using UnityEngine;
using System.Collections;

public class RandomBlending : MonoBehaviour {

    SkinnedMeshRenderer _smr;
    Vector2 _blends = Vector2.zero;
    //blend controls
    private float bs_back = 0f;
    private float bs_front = 0f;
    private float bs_left = 0f;
    private float bs_right = 0f;

    private int bs_down_lookup = 0;
    private int bs_up_lookup = 1;
    private int bs_left_lookup = 2;
    private int bs_right_lookup = 3;

    public enum BlendDirection {
        Base,
        Up,
        Down,
        Left,
        Right
    } 

    [System.Serializable]
    public class BlendDirections{
        public BlendDirection blend;
        public int lookup;
    }

    public BlendDirections[] blendCorrelations = new BlendDirections[5];

    //sinusoidal controls
    public float amplitudeX = 1.0f;
    public float amplitudeY = 0.5f;
    public float omegaX = 1.0f;
    public float omegaY = 0.5f;
    private float _index = 0;
    private float x_mult = 0;
    private float y_mult = 0;


    Vector2 SineOut() {
        _index += Time.deltaTime;
        float x = amplitudeX * Mathf.Cos(omegaX * _index);
        float y = amplitudeY * Mathf.Sin(omegaY * _index);       

        Vector2 outVec = new Vector2(x, y);
        return outVec;
    }

    void SetBlendShapes(Vector2 position) {        
        if(position.x > 0) {
            _smr.SetBlendShapeWeight(bs_right_lookup, position.x);            
        } 

        if(position.x < 0) {
            _smr.SetBlendShapeWeight(bs_left_lookup, position.x);            
        } 

        if(position.y > 0) {
            _smr.SetBlendShapeWeight(bs_up_lookup, position.y);            
        }

        if(position.y < 0) {
            _smr.SetBlendShapeWeight(bs_down_lookup, position.x);            
        }     
    }

    void SetBlendIndexes() {
        int check = 0;

        for(int i = 0; i < blendCorrelations.Length; i++) {
            if(blendCorrelations[i].blend == BlendDirection.Up) {
                bs_up_lookup = blendCorrelations[i].lookup;
                check++;
            }

            if(blendCorrelations[i].blend == BlendDirection.Down) {
                bs_down_lookup = blendCorrelations[i].lookup;
                check++;
            }

            if(blendCorrelations[i].blend == BlendDirection.Right) {
                bs_right_lookup = blendCorrelations[i].lookup;
                check++;
            }

            if(blendCorrelations[i].blend == BlendDirection.Left) {
                bs_left_lookup = blendCorrelations[i].lookup;
                check++;
            }
        }

        if(check < 4) {
            Debug.LogError("Check your BlendCorrelations, they may not be assigned correctly and may cause incorrect animation.");
        }
    }
    
    void Start () {
        _smr = GetComponent<SkinnedMeshRenderer>();
        if(_smr == null) {
            this.enabled = false;
        }
        x_mult = Random.Range(0.75f, 1.25f);
        y_mult = Random.Range(0.75f, 1.25f);
        SetBlendIndexes();
	}	
	
	void Update () {
        Vector2 current = SineOut();
        SetBlendShapes(current);
	}
}

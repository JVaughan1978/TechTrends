using UnityEngine;
using System;
using System.Collections;

public class PortraitHighlight : MonoBehaviour {
        
    public int myNum = 0;
    public float duration = 1.0f;
    public float moveX = 0;
    private float _time = 0f;
    
    private bool _selected = false;
    private bool _switching = false;

    private BoxCollider myBox;

    private Vector3 initialPosition = Vector3.zero;
    private Vector3 newPosition = Vector3.zero;    

    public delegate void ShoveAction(int num);
    public static event ShoveAction OnShove;

    public delegate void SlideAction(int num);
    public static event SlideAction OnSlide;

    void OnEnable() {
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
        PortraitHighlight.OnShove += Shoved;
        PortraitHighlight.OnSlide += Slid;
    }

    void OnDisable() {
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
        PortraitHighlight.OnShove -= Shoved;
        PortraitHighlight.OnSlide -= Slid;
    }

    void Highlighted(string name) {        
        if(name == this.name && !_selected) {
            this.GetComponent<HighlightReaction>().CoolDown(duration + 0.1f);
            _selected = true;
            _switching = true;
            Show();
            ScaleUp();
            if (OnShove != null) {
                OnShove(myNum);
            }            
        } else {
            _switching = true;
        }               
    }

    void Deselected(string name) {
        if(name == this.name && _selected) {
            this.GetComponent<HighlightReaction>().CoolDown(duration + 0.1f);
            _selected = false;
            _switching = true;            
            Hide();            
            ScaleDown();
            if(OnSlide != null) {
                OnSlide(myNum);
            }
        } else {
            _switching = true;
        }
    }

    void Reset() {
        _time = 0;
        _switching = false;
        newPosition = transform.localPosition;        
    }

    IEnumerator Move(float duration, bool push) {
        while(_time < duration) {
            if(_switching) {
                _time += Time.deltaTime;

                if(_time > duration) {
                    Invoke("Reset", 0.1f);
                    _time = duration;
                }

                float v1 = 0;
                float newX = initialPosition.x + moveX;

                if(push) {                    
                    v1 = Easing.CubicEaseInOut(_time, initialPosition.x, newX, duration);
                } else {                    
                    v1 = Easing.CubicEaseInOut(_time, newPosition.x, -newX, duration);                                      
                }
                
                Vector3 apply = new Vector3(v1, initialPosition.y, initialPosition.z);
                transform.localPosition = apply;
            }            
            yield return null;
        }
    }

    IEnumerator Scale(float duration, bool grow) {
        while(_time < duration) {
            if(_switching) {
                _time += Time.deltaTime;

                if(_time > duration) {
                    Invoke("Reset", 0.1f);
                    _time = duration;
                }

                float v1, v2, v3;

                if(grow) {                    
                    v1 = Easing.CubicEaseInOut(_time, 0.5f, 0.5f, duration);
                    v2 = Easing.CubicEaseInOut(_time, 0.5f, 0.5f, duration);
                    v3 = Easing.CubicEaseInOut(_time, 0.5f, 0.5f, duration);
                } else {                    
                    v1 = Easing.CubicEaseInOut(_time, 1.0f, -0.5f, duration);
                    v2 = Easing.CubicEaseInOut(_time, 1.0f, -0.5f, duration);
                    v3 = Easing.CubicEaseInOut(_time, 1.0f, -0.5f, duration);
                }

                Vector3 apply = new Vector3(v1, v2, v3);                
                transform.localScale = apply;
            }
            yield return null;
        }
    }

    void EnableCollision() {
        myBox.enabled = true;
    }

    void Shoved(int num) {        
        if(num != myNum) {
            myBox.enabled = false;            
        }

        if(num < myNum) {            
            StartCoroutine(Move(duration, true));            
        }
    }

    void Slid(int num) {
        if(num != myNum) {
            myBox.enabled = false;
            Invoke("EnableCollision", duration + 0.1f);
        }

        if(num < myNum) {                        
            StartCoroutine(Move(duration, false));
            myBox.enabled = false;
        }
    }

    void Show() {
        for(int i = 0; i < transform.childCount; i++) {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(true);
        }
    }

    void Hide() {
        for(int i = 0; i < transform.childCount; i++) {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(false);
        }
    }

    void ScaleUp() {
        StartCoroutine(Scale(duration, true));
    }

    void ScaleDown() {
        StartCoroutine(Scale(duration, false));
    }
    
    void Start() {
        initialPosition = transform.localPosition;
        
        myBox = GetComponent<BoxCollider>();
        if(myBox == null) {
            Debug.LogWarning("No BoxCollider found.");
        }
    }
    
    void Update() {
    }
}
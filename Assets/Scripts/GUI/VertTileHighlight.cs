using UnityEngine;
using System.Collections;

public class VertTileHighlight : MonoBehaviour
{
    public int myNum = 0;
    public float duration = 0.5f;
    public float moveX = 0;

    private bool _switching = false;
    private float _time = 0f;
    private Vector3 _startPos;
    private Vector3 _startScale;
    private BoxCollider myBox;

    private Vector3 initialPosition = Vector3.zero;
    private Vector3 newPosition = Vector3.zero; 

    public delegate void ShoveAction(int num);
    public static event ShoveAction OnShove;

    public delegate void SlideAction(int num);
    public static event SlideAction OnSlide;

    void OnEnable()
    {
        HighlightReaction.OnHighlight += Highlighted;
        HighlightReaction.OnDeselect += Deselected;
        VertTileHighlight.OnShove += Shoved;
        VertTileHighlight.OnSlide += Slid;
    }

    void OnDisable()
    {
        HighlightReaction.OnHighlight -= Highlighted;
        HighlightReaction.OnDeselect -= Deselected;
        VertTileHighlight.OnShove -= Shoved;
        VertTileHighlight.OnSlide -= Slid;
    }

    void Reset() {
        _time = 0;
        _switching = false;
        newPosition = transform.localPosition;
    }

    void Highlighted(string name)
    {
        if (name == this.name)
        {
            //Show();
            ScaleUp();
            _switching = true;
            if (OnShove != null)
            {
                OnShove(myNum);
            }
        }
        else
        {
            //Hide();
        }
    }

    void Deselected(string name)
    {
        if (name == this.name)
        {
            //Hide();
            _switching = true;
            ScaleDown();
            if (OnSlide != null)
            {
                OnSlide(myNum);
            }
        }
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
                    v1 = Easing.CubicEaseInOut(_time, 1.0f, 0.0f, duration);
                    v2 = Easing.CubicEaseInOut(_time, 1.0f, 1.0f, duration);
                    v3 = Easing.CubicEaseInOut(_time, 1.0f, 0.0f, duration);
                } else {
                    v1 = Easing.CubicEaseInOut(_time, 1.0f, 0.0f, duration);
                    v2 = Easing.CubicEaseInOut(_time, 2.0f, -1.0f, duration);
                    v3 = Easing.CubicEaseInOut(_time, 1.0f, 0.0f, duration);
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

    // Use this for initialization
    void Start()
    {
        _startPos = transform.localPosition;
        _startScale = transform.localScale;

        myBox = GetComponent<BoxCollider>();
        if(myBox == null) {
            Debug.LogWarning("No BoxCollider found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

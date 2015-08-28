using UnityEngine;
using System.Collections;

public class VertTileHighlight : MonoBehaviour
{
    public int myNum = 0;
    private Vector3 _startPos;
    private Vector3 _startScale;

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

    void Highlighted(string name)
    {
        if (name == this.name)
        {
            //Show();
            ScaleUp();
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
            ScaleDown();
            if (OnSlide != null)
            {
                OnSlide(myNum);
            }
        }
    }

    void Shoved(int num)
    {
        //this will fire off an coroutine later HACK
        if (num < myNum)
        {
            Vector3 tempVect = new Vector3(_startPos.x, (_startPos.y - 1.1f), _startPos.z);
            transform.localPosition = tempVect;
        }
    }

    void Slid(int num)
    {
        //this will fire off a coroutine later HACK
        if (num < myNum)
        {
            transform.localPosition = _startPos;
        }
    }

    void Show()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(true);//going to need another way to set this, too many objects might not need to be seen
        }
    }

    void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform go = transform.GetChild(i);
            go.gameObject.SetActive(false);
        }
    }

    void ScaleUp()
    {
        //this will fire off an coroutine later HACK
        transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
    }

    void ScaleDown()
    {
        //this will fire off an coroutine later HACK
        transform.localScale = _startScale;
    }

    // Use this for initialization
    void Start()
    {
        _startPos = transform.localPosition;
        _startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

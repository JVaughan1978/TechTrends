using UnityEngine;
using System.Collections;

public class MatStretcher : MonoBehaviour
{

    private float _startYTiling;
    private MeshRenderer _mr = null;

    // Use this for initialization
    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        if (_mr == null)
        {
            Debug.LogWarning("No MeshRenderer found");
        }

        _startYTiling = _mr.material.mainTextureScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        float yFactor = transform.localScale.y;
        _mr.material.mainTextureScale = new Vector2(1.0f, (_startYTiling * yFactor));
    }
}

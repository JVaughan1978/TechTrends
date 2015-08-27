using UnityEngine;
using System.Collections;

public class SelectionCircle : MonoBehaviour {

    private Material _currentMaterial;
    public Vector2 startTextureOffset = Vector2.zero;
    public Vector2 endTextureOffset = Vector2.zero;
    public float speed = 3f;
    private float _time = 0;
    private bool _filling = false;

    void OnEnable() {
        SelectionReaction.OnFocus += Focus;
        SelectionReaction.OnSelect += Deselect;
        SelectionReaction.OnDeselect += Deselect;
    }

    void OnDisable() {
        SelectionReaction.OnFocus -= Focus;
        SelectionReaction.OnSelect -= Deselect;
        SelectionReaction.OnDeselect -= Deselect;
    }

    IEnumerator Fill(float duration) {
        while(_time < duration) {
            if(_filling) {
                _time += Time.deltaTime;
                if(_time > duration) {
                    Invoke("Reset", 0.1f);
                    Debug.Log("Cleaning up Fill");
                    _time = duration;
                } //cap the time

                float v1 = Easing.Linear(_time, startTextureOffset.x, endTextureOffset.x, duration);
                float v2 = Easing.Linear(_time, startTextureOffset.y, endTextureOffset.y, duration);

                Vector2 apply = new Vector2(v1, v2);
                _currentMaterial.SetTextureOffset("_MainTex", apply);

            }
            yield return null;
        }
    }

    void Reset() {
        _time = 0;
        _currentMaterial.SetTextureOffset("_MainTex", startTextureOffset);
    }

    void Focus() {
        if(!_filling) {
            StartCoroutine(Fill(speed));
            _filling = true;
        }
    }

    void Deselect() {
        if(_filling) {
            StopAllCoroutines();
            Invoke("Reset", 0.1f);
            _filling = false;
        }
    }

    void Start() {
        Renderer r = gameObject.GetComponent<Renderer>();
        if(r != null) {
            _currentMaterial = r.material;
        }
    }

    void Update() {

    }
}

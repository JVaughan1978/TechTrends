using UnityEngine;
using System.Collections;

public class ExtensionMethods : MonoBehaviour {

    public static bool CompareVectors(Vector3 v1, Vector3 v2, float angleError) {
        if(!Mathf.Approximately(v1.magnitude, v2.magnitude)) {
            return false;
        }

        float cosAngleError = Mathf.Cos(angleError * Mathf.Deg2Rad);
        float cosAngle = Vector3.Dot(v1.normalized, v2.normalized);

        if(cosAngle >= cosAngleError) {
            return true;
        } else {
            return false;
        }
    }
}

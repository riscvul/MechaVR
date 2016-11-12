using UnityEngine;
using System.Collections;

public static class Utils {
    public static float ClampAngle(float angle, float min, float max) {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180) {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180) {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180) {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180) {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }
}

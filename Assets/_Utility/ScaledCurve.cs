using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ScaledCurve
{
    public AnimationCurve Curve;
    public float Scale;

    public ScaledCurve(AnimationCurve c, float s)
    {
        Curve = c;
        Scale = s;
    }

    public float Evaluate(float t)
    {
        return Curve.Evaluate(t) * Scale;
    }

    /// <summary>
    /// Returns scaled value at t = 0
    /// </summary>
    /// <returns></returns>
    public float T0()
    {
        return Curve.Evaluate(0f) * Scale;
    }

    /// <summary>
    /// Returns scaled value at t = 1
    /// </summary>
    /// <returns></returns>
    public float T1()
    {
        return Curve.Evaluate(1f) * Scale;
    }
}


[System.Serializable]
public struct ScaledVector3Curve
{
    public AnimationCurve XCurve;
    public AnimationCurve YCurve;
    public AnimationCurve ZCurve;
    public Vector3 VectorScale;

    public ScaledVector3Curve(AnimationCurve xc, AnimationCurve yc, AnimationCurve zc, Vector3 v)
    {
        XCurve = xc;
        YCurve = yc;
        ZCurve = zc;
        VectorScale = v;
    }

    public Vector3 Evaluate(float t)
    {
        return new(XCurve.Evaluate(t) * VectorScale.x,
            YCurve.Evaluate(t) * VectorScale.y,
            ZCurve.Evaluate(t) * VectorScale.z);
    }

    /// <summary>
    /// Returns scaled value at t = 0
    /// </summary>
    /// <returns></returns>
    public Vector3 T0()
    {
        return Evaluate(0f);
    }

    /// <summary>
    /// Returns scaled value at t = 1
    /// </summary>
    /// <returns></returns>
    public Vector3 T1()
    {
        return Evaluate(1f);
    }
}
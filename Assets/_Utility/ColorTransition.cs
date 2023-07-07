using UnityEngine;

[System.Serializable]
public struct ColorTransition
{
    [ColorUsage(true, false)] public Color Color1;
    [ColorUsage(true, false)] public Color Color2;
    public AnimationCurve BlendCurve;

    public ColorTransition(Color c1, Color c2, AnimationCurve curve)
    {
        Color1 = c1;
        Color2 = c2;
        BlendCurve = curve;
    }
    public Color GetBlend(float t, bool invert = false)
    {
        Color a = invert ? Color2 : Color1;
        Color b = invert ? Color1 : Color2;
        return Color.Lerp(a, b, BlendCurve.Evaluate(t));
    }

}

[System.Serializable]
public struct ColorTransitionHDR
{
    [ColorUsage(true, true)] public Color Color1;
    [ColorUsage(true, true)] public Color Color2;
    public AnimationCurve BlendCurve;

    public ColorTransitionHDR(Color c1, Color c2, AnimationCurve curve)
    {
        Color1 = c1;
        Color2 = c2;
        BlendCurve = curve;
    }
    public Color GetBlend(float t, bool invert = false)
    {
        Color a = invert ? Color2 : Color1;
        Color b = invert ? Color1 : Color2;
        return Color.Lerp(a, b, BlendCurve.Evaluate(t));
    }

}
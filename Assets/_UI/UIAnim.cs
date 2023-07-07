using UnityEngine;

[CreateAssetMenu(fileName = "uiAnim", menuName = "Data/UI/Animation")]
public class UIAnim : ScriptableObject
{
    [SerializeField] float _duration;
    [Header("Alpha")]
    [SerializeField] bool _applyAlpha;
    [SerializeField] AnimationCurve _curveAlpha;
    [Header("Motion")]
    [SerializeField] bool _applyMotion;
    [SerializeField] Vector2 _motionOffset;
    [SerializeField][Tooltip("Value of 1 = motionOffset, Value of 0 = basePosition")] AnimationCurve _curveMotion;
    [Header("Scale")]
    [SerializeField] bool _applyScale;
    [SerializeField] ScaledVector3Curve _curveScale;
    [Header("Color")]
    [SerializeField] bool _applyColor;
    [SerializeField] bool _useColorTransition;
    [ColorUsage(true, false)]
    [SerializeField] Color _colorMain;
    [SerializeField][Tooltip("Value of 1 = colorMain, Value of 0 = baseColor")] AnimationCurve _curveColorMain;
    [SerializeField] ColorTransition _colorTransition;

    public float Duration { get => _duration; }
    public bool ApplyAlpha { get => _applyAlpha; }
    public AnimationCurve CurveAlpha { get => _curveAlpha; }
    public bool ApplyMotion { get => _applyMotion; }
    public Vector2 MotionOffset { get => _motionOffset; }
    public AnimationCurve CurveMotion { get => _curveMotion; }
    public bool ApplyScale { get => _applyScale; }
    public ScaledVector3Curve CurveScale { get => _curveScale; }
    public bool ApplyColor { get => _applyColor; }
    public bool UseColorTransition { get => _useColorTransition; }
    public Color ColorMain { get => _colorMain; }
    public AnimationCurve CurveColorMain { get => _curveColorMain; }
    public ColorTransition ColorTransition { get => _colorTransition; }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "uiAnimSet", menuName = "Data/UI/Animation Set")]
public class UIAnimSet : ScriptableObject
{
    [Header("Show Element Animation")]
    [SerializeField] bool _animateShow;
    [SerializeField] UIAnim _animShow;
    [Header("Hide Element Animation")]
    [SerializeField] bool _animateHide;
    [SerializeField] UIAnim _animHide;
    [Header("Idle Animation")]
    [SerializeField] bool _animateIdle;
    [SerializeField][Tooltip("If true, ping-pongs idle anim t value rather than looping")] bool _pingPongIdle;
    [SerializeField] UIAnim _animIdle;
    [Header("Additional Animations")]
    [SerializeField] bool _animateActions;
    [SerializeField] List<UIAnim> _animActions;

    public UIAnim AnimShow { get => _animShow; }
    public UIAnim AnimHide { get => _animHide; }
    public UIAnim AnimIdle { get => _animIdle; }
    public List<UIAnim> AnimActions { get => _animActions; }

    public bool AnimateShow { get => _animateShow; }
    public bool AnimateHide { get => _animateHide; }
    public bool AnimateIdle { get => _animateIdle; }
    public bool AnimateActions { get => _animateActions; }
    public bool PingPongIdle { get => _pingPongIdle; }
}

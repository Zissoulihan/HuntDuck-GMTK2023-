using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIAnimatedElement : MonoBehaviour
{
    [SerializeField] UIAnimSet _animSet;
    [SerializeField] bool _playShowOnStart;
    [SerializeField] bool _playIdleAfterShow;
    [SerializeField] bool _playIdleAfterAction;
    [SerializeField] float _delayShow;
    [SerializeField] float _delayHide;
    [SerializeField] float _delayAction;
    [SerializeField] float _durationAddShow;
    [SerializeField] float _durationAddHide;
    [SerializeField] CanvasGroup _cg;
    [SerializeField] RectTransform _animRoot;
    [SerializeField] Image _colorizedImage;
    [SerializeField] TextMeshProUGUI _colorizedText;
    [SerializeField] bool _debugLogging;

    public event Action OnShowStart;
    public event Action OnShowComplete;
    public event Action OnHideStart;
    public event Action OnHideComplete;
    public event Action OnIdleStart;
    public event Action OnIdleStopped;
    public event Action OnActionStart;
    public event Action OnActionComplete;
    public UnityEvent OnShowStartUE;
    public UnityEvent OnShowCompleteUE;
    public UnityEvent OnHideStartUE;
    public UnityEvent OnHideCompleteUE;
    public UnityEvent OnIdleStartUE;
    public UnityEvent OnIdleStoppedUE;
    public UnityEvent OnActionStartUE;
    public UnityEvent OnActionCompleteUE;

    Coroutine _activeAnim;
    public bool Animating { get => _activeAnim != null; }
    public bool Shown { get; private set; } = false;
    public bool AnimatingIdle { get; private set; } = false;

    Vector3 _baseScale;
    Vector2 _basePos;
    Color _baseColor;   //TODO: Distinguish between text and image color OR limit colorizing to either image or text
    float _baseAlpha;

    #region Initialization

    private void Awake()
    {
        if (_animRoot != null) {
            _basePos = _animRoot.anchoredPosition;
            _baseScale = _animRoot.localScale;
        }
        if (_cg != null) {
            _baseAlpha = _cg.alpha;
        }
        if (_colorizedImage != null) {
            _baseColor = _colorizedImage.color;
        }
        if (_debugLogging) print($"UIEle {gameObject.name}: Awake complete");
    }
    private void Start()
    {
        if (_playShowOnStart) Show();
    }
    private void OnEnable()
    {
        SelfSubscribe();
    }
    private void OnDisable()
    {
        SelfUnsubscribe();
    }
    #endregion

    #region Event Actions & Relays
    void SelfSubscribe()
    {
        OnShowStart += UEShowStart;
        OnShowComplete += UEShowComplete;
        OnHideStart += UEHideStart;
        OnHideComplete += UEHideComplete;
        OnIdleStart += UEIdleStart;
        OnIdleStopped += UEIdleStopped;
        OnActionStart += UEActionStart;
        OnActionComplete += UEActionComplete;
    }
    void SelfUnsubscribe()
    {
        OnShowStart -= UEShowStart;
        OnShowComplete -= UEShowComplete;
        OnHideStart -= UEHideStart;
        OnHideComplete -= UEHideComplete;
        OnIdleStart -= UEIdleStart;
        OnIdleStopped -= UEIdleStopped;
        OnActionStart -= UEActionStart;
        OnActionComplete -= UEActionComplete;
    }
    void UEShowStart()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Show anim starting");
        OnShowStartUE?.Invoke();
    }
    void UEShowComplete()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Show anim complete");
        Shown = true;
        OnShowCompleteUE?.Invoke();
        if (_playIdleAfterShow) StartIdleAnim();
    }
    void UEHideStart()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Hide anim starting");
        OnHideStartUE?.Invoke();
    }
    void UEHideComplete()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Hide anim complete");
        Shown = false;
        OnHideCompleteUE?.Invoke();
    }
    void UEIdleStart()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Idle anim starting");
        OnIdleStartUE?.Invoke();
    }
    void UEIdleStopped()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Idle anim stopped");
        OnIdleStoppedUE?.Invoke();
    }
    void UEActionStart()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Action anim starting");
        OnActionStartUE?.Invoke();
    }
    void UEActionComplete()
    {
        if (_debugLogging) print($"UIEle {gameObject.name}: Action anim complete");
        OnActionCompleteUE?.Invoke();
        if (_playIdleAfterAction) StartIdleAnim();
    }
    #endregion

    #region Animation Triggers
    public void Show()
    {
        if (!_animSet.AnimateShow) return;
        if (_activeAnim != null) StopCoroutine(_activeAnim);
        if (AnimatingIdle) AnimatingIdle = false;
        _activeAnim = StartCoroutine(Animate(_animSet.AnimShow, OnShowStart, OnShowComplete, _delayShow, _durationAddShow));
        if (_debugLogging) print($"UIEle {gameObject.name}: Show triggered");
    }
    public void Hide()
    {
        if (!_animSet.AnimateHide) return;
        if (_activeAnim != null) StopCoroutine(_activeAnim);
        if (AnimatingIdle) AnimatingIdle = false;
        _activeAnim = StartCoroutine(Animate(_animSet.AnimHide, OnHideStart, OnHideComplete, _delayHide, _durationAddHide));
        if (_debugLogging) print($"UIEle {gameObject.name}: Hide triggered");
    }
    public void StartIdleAnim()
    {
        if (!_animSet.AnimateIdle) return;
        if (_activeAnim != null) StopCoroutine(_activeAnim);
        AnimatingIdle = true;
        _activeAnim = StartCoroutine(Animate(_animSet.AnimIdle, OnIdleStart, OnIdleStopped));
        if (_debugLogging) print($"UIEle {gameObject.name}: Idle triggered");

    }
    public void StopIdleAnim()
    {
        if (_activeAnim != null && AnimatingIdle) ResetElement();
        if (_debugLogging) print($"UIEle {gameObject.name}: Idle stop triggered");
        OnIdleStopped?.Invoke();
    }
    public void Action(int index = 0)
    {
        if (!_animSet.AnimateActions) return;
        if (index >= _animSet.AnimActions.Count) {
            if (_debugLogging) print($"UIEle {gameObject.name}: Action index out of range!");
            return;
        }
        if (_animSet.AnimActions[index] == null) {
            if (_debugLogging) print($"UIEle {gameObject.name}: Action animation for index {index} is null");
            return;
        }
        if (_activeAnim != null) StopCoroutine(_activeAnim);
        if (AnimatingIdle) AnimatingIdle = false;
        _activeAnim = StartCoroutine(Animate(_animSet.AnimActions[index], OnActionStart, OnActionComplete, _delayAction));
        if (_debugLogging) print($"UIEle {gameObject.name}: Action {index} triggered");
    }
    #endregion

    #region Animation
    IEnumerator Animate(UIAnim animation, Action startEvent, Action stopEvent, float initialDelay = 0f, float additionalDuration = 0f)
    {
        //Initialize animated elements
        AnimateAlpha(0f, animation);
        AnimatePosition(0f, animation);
        AnimateScale(0f, animation);
        AnimateColor(0f, animation);

        //Wait for animation delay
        if (initialDelay > 0f) yield return TaroH.GetWait(initialDelay);

        float startTime = Time.time;
        float duration = animation.Duration + additionalDuration;

        startEvent?.Invoke();

        //Apply Animation
        while (AnimatingIdle || Time.time < startTime + duration) {
            float t = CalculateT(animation, startTime);
            AnimateAlpha(t, animation);
            AnimatePosition(t, animation);
            AnimateScale(t, animation);
            AnimateColor(t, animation);
            yield return null;
        }

        //Animation Completed
        _activeAnim = null;
        stopEvent?.Invoke();

    }

    float CalculateT(UIAnim anim, float startTime)
    {
        float duration = anim.Duration;
        float t = (Time.time - startTime) / duration;
        if (AnimatingIdle) {
            if (_animSet.PingPongIdle) {
                return Mathf.PingPong(t, 1f);
            }
            else return Mathf.Repeat(t, 1f);
        }
        return t;
    }

    void AnimateAlpha(float t, UIAnim anim)
    {
        if (!anim.ApplyAlpha) return;
        if (_cg == null) return;

        _cg.alpha = anim.CurveAlpha.Evaluate(t);
    }
    void AnimatePosition(float t, UIAnim anim)
    {
        if (!anim.ApplyMotion) return;
        if (_animRoot == null) return;

        Vector2 goalPos = _basePos + anim.MotionOffset;

        float motionT = anim.CurveMotion.Evaluate(t);

        _animRoot.anchoredPosition = Vector2.Lerp(_basePos, goalPos, motionT);
    }
    void AnimateScale(float t, UIAnim anim)
    {
        if (!anim.ApplyScale) return;
        if (_animRoot == null) return;

        _animRoot.localScale = anim.CurveScale.Evaluate(t);
    }
    void AnimateColor(float t, UIAnim anim)
    {
        if (!anim.ApplyColor) return;
        if (_colorizedImage == null && _colorizedText == null) return;

        //Image
        if (_colorizedImage != null) {
            _colorizedImage.color = anim.UseColorTransition ? 
                anim.ColorTransition.GetBlend(t) : 
                Color.Lerp(_baseColor, anim.ColorMain, anim.CurveColorMain.Evaluate(t));
        }
        //Text
        if (_colorizedText != null) {
            _colorizedText.color = anim.UseColorTransition ?
                anim.ColorTransition.GetBlend(t) :
                Color.Lerp(_baseColor, anim.ColorMain, anim.CurveColorMain.Evaluate(t));
        }
    }

    #region Resets
    public void ResetElement()
    {
        if (_activeAnim != null) {
            StopCoroutine(_activeAnim);
            _activeAnim = null;
        }
        AnimatingIdle = false;
        ResetScale();
        ResetPosition();
        ResetAlpha();
        ResetColor();
    }
    void ResetPosition()
    {
        if (_animRoot == null) return;
        _animRoot.anchoredPosition = _basePos;
    }
    void ResetScale()
    {
        if (_animRoot == null) return;
        _animRoot.localScale = _baseScale;
    }
    void ResetAlpha()
    {
        if (_cg == null) return;
        _cg.alpha = _baseAlpha;
    }
    void ResetColor()
    {
        if (_colorizedImage == null) return;
        _colorizedImage.color = _baseColor;
    }
    #endregion
    #endregion

    #region Additional Utility
    //Mostly useful for prefabs
    public void DestroyElement()
    {
        Destroy(this.gameObject);
    }
    public void SetBasePosition(Vector3 pos)
    {
        _basePos = pos;
    }
    #endregion
}

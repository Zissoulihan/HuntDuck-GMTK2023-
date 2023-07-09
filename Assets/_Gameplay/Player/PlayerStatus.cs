using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] UIAnimatedElement _uiAlert;
    [SerializeField] GameEventDogState _evDogStateChange;
    [SerializeField] Slider _sQuack;
    [SerializeField] float _durationQuackBuildup;
    [SerializeField] float _interquackDelay;
    [SerializeField] Vector2 _quackBuildupVariance;
    [SerializeField] InvestigateNode _prefabSoundNode;
    [SerializeField] AudioSource _as;
    [SerializeField] List<AudioClip> _sfxQuacks;
    [SerializeField] GameEventVoid _evInputQuack;
    [SerializeField] float _cooldownManualQuack;

    public Vector3 PlayerPosition => transform.position;
    public bool Immortal = false;

    Coroutine _quaction;

    float _quackUrge;
    float _timeLastQuack;
    float _currentQuackCooldown;

    private void OnEnable()
    {
        _evDogStateChange.Subscribe(CheckDogState);
        _evInputQuack.Subscribe(InputQuackManual);
    }
    private void OnDisable()
    {
        _evDogStateChange.Unsubscribe(CheckDogState);
        _evInputQuack.Unsubscribe(InputQuackManual);
    }

    private void Awake()
    {
        _sQuack.value = 0f;
        _currentQuackCooldown = _cooldownManualQuack;
    }

    private void Start()
    {
        _quaction = StartCoroutine(QuackManagement());
    }

    IEnumerator QuackManagement()
    {
        while (!Immortal) {
            float startTime = Time.time;
            float quackSum = _durationQuackBuildup + Random.Range(_quackBuildupVariance.x, _quackBuildupVariance.y);
            while (Time.time < startTime + quackSum) {
                float t = (Time.time - startTime) / quackSum;
                _sQuack.value = t;
                yield return null;
            }

            Quack();
            yield return TaroH.GetWait(_interquackDelay);
        }
        _quaction = null;
    }

    void InputQuackManual()
    {
        if (Time.time < _timeLastQuack + _currentQuackCooldown) return;
        if (Immortal) return;
        if (_quaction != null) StopCoroutine(_quaction);
        Quack();
        _quaction = StartCoroutine(QuackManagement());
        _timeLastQuack = Time.time;
    }

    void Quack()
    {
        var clip = _sfxQuacks[Random.Range(0, _sfxQuacks.Count - 1)];
        _sQuack.value = 0f;
        _as.PlayOneShot(clip);
        Instantiate(_prefabSoundNode,transform.position, Quaternion.identity);
    }

    public void BecomeImmortal()
    {
        Immortal = true;
        if (_quaction != null) StopCoroutine(_quaction);
    }

    void CheckDogState(DogState state)
    {
        if (state == DogState.Chase) {
            _uiAlert.Show();
            _currentQuackCooldown = 0f;
        } else {
            _currentQuackCooldown = _cooldownManualQuack;
        }
    }
}

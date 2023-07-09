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

    public Vector3 PlayerPosition => transform.position;
    public bool Immortal = false;

    float _quackUrge;

    private void OnEnable()
    {
        _evDogStateChange.Subscribe(CheckDogState);
    }
    private void OnDisable()
    {
        _evDogStateChange.Unsubscribe(CheckDogState);
    }

    private void Awake()
    {
        _sQuack.value = 0f;
    }

    private void Start()
    {
        StartCoroutine(QuackManagement());
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
            _sQuack.value = 0f;
            yield return TaroH.GetWait(_interquackDelay);
        }
    }

    void Quack()
    {
        var clip = _sfxQuacks[Random.Range(0, _sfxQuacks.Count - 1)];
        _as.PlayOneShot(clip);
        Instantiate(_prefabSoundNode,transform.position, Quaternion.identity);
    }

    public void BecomeImmortal()
    {
        Immortal = true;
    }

    void CheckDogState(DogState state)
    {
        if (state == DogState.Chase) {
            _uiAlert.Show();
        }
    }
}

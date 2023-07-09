using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] float _fadeoutDuration;

    AudioSource _music;
    float _vol;

    private void Awake()
    {
        _music = GetComponent<AudioSource>();
        _vol = _music.volume;
    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutMusic());
    }

    IEnumerator FadeOutMusic()
    {
        float startTime = Time.time;
        while (Time.time < startTime + _fadeoutDuration) {
            float t = (Time.time - startTime) / _fadeoutDuration;
            _music.volume = Mathf.Lerp(_vol, 0f, t);
            yield return null;
        }
    }
}

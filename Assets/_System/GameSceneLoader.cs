using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    [SerializeField] float _delayLoad = .965f;
    [SerializeField] string _sceneGame;
    [SerializeField] AudioSource _music;

    float _vol;

    private void Awake()
    {
        _vol = _music.volume;
    }
    public void TriggerLoadGameScene()
    {
        StartCoroutine(LoadGameScn());
    }

    IEnumerator LoadGameScn()
    {
        float startTime = Time.time;
        while (Time.time < startTime + _delayLoad) {
            float t = (Time.time - startTime) / _delayLoad;
            _music.volume = Mathf.Lerp(_vol, 0f, t);
            yield return null;
        }
        LoadGameScene();
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(_sceneGame);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tWinLose;
    [SerializeField] string _strVictory;
    [SerializeField] string _strDefeat;
    [SerializeField] UIAnimatedElement _uiWin;
    [SerializeField] UIAnimatedElement _uiLose;
    [SerializeField] UIAnimatedElement _fade;
    [SerializeField] GameEventVoid _evGameWon;
    [SerializeField] GameEventVoid _evGameLost;
    [SerializeField] string _sceneNameMainMenu;

    private void OnEnable()
    {
        _evGameWon.Subscribe(GameWon);
        _evGameLost.Subscribe(GameLost);
    }
    private void OnDisable()
    {
        _evGameWon.Unsubscribe(GameWon);
        _evGameLost.Unsubscribe(GameLost);
    }

    void GameWon()
    {
        GameEnd(true);
    }
    void GameLost()
    {
        _fade.Hide();
        GameEnd(false);
    }
    void GameEnd(bool win)
    {
        _tWinLose.text = win ? _strVictory : _strDefeat;
        if (win) _uiWin.Show();
        else _uiLose.Show();
    }

    public void InputReturnMainMenu()
    {
        SceneManager.LoadScene(_sceneNameMainMenu);
    }
}

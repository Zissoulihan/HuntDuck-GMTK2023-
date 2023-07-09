using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] UIAnimatedElement _uiAlert;
    [SerializeField] GameEventDogState _evDogStateChange;
    public Vector3 PlayerPosition => transform.position;
    public bool Immortal = false;

    private void OnEnable()
    {
        _evDogStateChange.Subscribe(CheckDogState);
    }
    private void OnDisable()
    {
        _evDogStateChange.Unsubscribe(CheckDogState);
    }

    public void BecomeImmortal()
    {

    }

    void CheckDogState(DogState state)
    {
        if (state == DogState.Chase) {
            _uiAlert.Show();
        }
    }
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName ="evNewGameEv",menuName ="Data/Events/GameEvent")]
public class GameEventVoid : ScriptableObject
{
    protected event Action _event;

    public void Subscribe(Action subscribingMethod)
    {
        _event += subscribingMethod;
    }
    public void Unsubscribe(Action subscribedMethod)
    {
        _event -= subscribedMethod;
    }
    public void TriggerEvent()
    {
        _event?.Invoke();
    }
}
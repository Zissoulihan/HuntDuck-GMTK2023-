using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogStatus : MonoBehaviour
{
    [SerializeField] DogMovement _move;
    [SerializeField] GameEventVoid _evDogInvestigating;
    [SerializeField] GameEventVoid _evDogChase;
    [SerializeField] GameEventInvestigateNode _evInvestigateNodeAlert;
    [SerializeField] GameEventPatrolNode _evPatrolNodeInit;

    public DogState State { get; private set; }

    List<PatrolNode> _patrolNodes = new();
    PatrolNode _targetPatrolNode;
    PatrolNode _lastPatrolNode;

    private void OnEnable()
    {
        _evInvestigateNodeAlert.Subscribe(HandleInvestigateAlert);
        _evPatrolNodeInit.Subscribe(TrackPatrolNode);
    }
    private void OnDisable()
    {
        _evInvestigateNodeAlert.Unsubscribe(HandleInvestigateAlert);
        _evPatrolNodeInit.Unsubscribe(TrackPatrolNode);
    }

    public void ChangeState(DogState newState)
    {
        ExitState(State);
        State = newState;
        EnterState(State);
    }
    private void Update()
    {
        UpdateState(State);
    }

    void EnterState(DogState state)
    {

    }
    void UpdateState(DogState state)
    {

    }
    void ExitState(DogState state)
    {

    }

    void HandleInvestigateAlert(InvestigateNode node)
    {
        if (State == DogState.Chase) {
            //Chasing player, don't investigate
            return;
        }
    }
    void TrackPatrolNode(PatrolNode node)
    {
        _patrolNodes.Add(node);
    }


}

public enum DogState
{
    Idle,
    Patrol,
    Investigate,
    Chase
}
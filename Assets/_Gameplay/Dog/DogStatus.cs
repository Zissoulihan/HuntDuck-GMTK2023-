using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogStatus : MonoBehaviour
{
    [SerializeField] DogMovement _move;
    [SerializeField] float _durationIdle;
    [SerializeField] GameEventVoid _evDogInvestigating;
    [SerializeField] GameEventVoid _evDogChase;
    [SerializeField] GameEventInvestigateNode _evInvestigateNodeAlert;
    [SerializeField] GameEventPatrolNode _evPatrolNodeInit;

    public DogState State { get; private set; }

    List<PatrolNode> _patrolNodes = new();
    PatrolNode _targetPatrolNode;
    PatrolNode _lastPatrolNode;
    InvestigateNode _activeInvestigateNode;

    Coroutine _activeBehavior = null;

    float _idleTime = 0f;

    private void Awake()
    {
        State = DogState.Idle;
    }
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
        print($"Dog exiting state {State}");
        ExitState(State);
        State = newState;
        print($"Dog entering state {newState}");
        EnterState(State);
    }
    private void Update()
    {
        UpdateState(State);
    }

    #region Heinous State Machine
    void EnterState(DogState state)
    {
        switch (state) {
            case DogState.Idle:
                EnterIdle();
                break;
            case DogState.Patrol:
                EnterPatrol();
                break;
            case DogState.Investigate:
                EnterInvestigate();
                break;
            case DogState.Chase:
                EnterChase();
                break;
            default:
                return;
        }

        void EnterIdle()
        {
            _idleTime = Time.time;
        }
        void EnterPatrol()
        {
            _activeBehavior = StartCoroutine(BehavePatrol());
        }
        void EnterInvestigate()
        {

        }
        void EnterChase()
        {

        }
    }
    void UpdateState(DogState state)
    {
        switch (state) {
            case DogState.Idle:
                UpdateIdle();
                break;
            case DogState.Patrol:
                UpdatePatrol();
                break;
            case DogState.Investigate:
                UpdateInvestigate();
                break;
            case DogState.Chase:
                UpdateChase();
                break;
            default:
                return;
        }

        void UpdateIdle()
        {
            if (Time.time < _idleTime + _durationIdle) return;
            ChangeState(DogState.Patrol);
        }
        void UpdatePatrol()
        {

        }
        void UpdateInvestigate()
        {

        }
        void UpdateChase()
        {

        }
    }
    void ExitState(DogState state)
    {
        switch (state) {
            case DogState.Idle:
                ExitIdle();
                break;
            case DogState.Patrol:
                ExitPatrol();
                break;
            case DogState.Investigate:
                ExitInvestigate();
                break;
            case DogState.Chase:
                ExitChase();
                break;
            default:
                return;
        }

        if (_activeBehavior != null) {
            StopCoroutine(_activeBehavior);
            _activeBehavior = null;
        }

        void ExitIdle()
        {

        }
        void ExitPatrol()
        {

        }
        void ExitInvestigate()
        {

        }
        void ExitChase()
        {

        }
    }
    #endregion

    #region Behaviors

    #region Patrol
    IEnumerator BehavePatrol()
    {
        while (_targetPatrolNode == null) {
            _targetPatrolNode = SelectPatrolNode();
            yield return null;
        }

        _move.MoveToPosition(_targetPatrolNode.WorldPos);

        while (_move.Moving) {
            yield return null;
        }

        _lastPatrolNode = _targetPatrolNode;
        _targetPatrolNode = null;
        _activeBehavior = null;
    }
    PatrolNode SelectPatrolNode()
    {
        if (_patrolNodes == null || _patrolNodes.Count < 1) {
            return null;
        }

        PatrolNode tarNode = null;
        float minTime = -1f;
        foreach (var node in _patrolNodes) {
            if (node.LastVisitTime <= minTime) {
                tarNode = node;
                minTime = node.LastVisitTime;
            }
        }
        return tarNode;
    }
    PatrolNode SelectClosestPatrolNode()
    {
        if (_patrolNodes == null || _patrolNodes.Count < 1) {
            return null; 
        }

        float minDist = 999f;
        PatrolNode tarNode = null;
        foreach (var node in _patrolNodes) {
            if (_lastPatrolNode != null) if (node == _lastPatrolNode) continue;
            var dist = GetDistanceToNode(node.WorldPos);
            if (dist <= minDist) {
                tarNode = node;
                minDist = dist;
            }
        }
        return tarNode;
    }

    #endregion
    float GetDistanceToNode(Vector3 nodeWorldPos)
    {
        return Mathf.Abs(Vector3.Distance(transform.position, nodeWorldPos));
    }
    #endregion

    void HandleInvestigateAlert(InvestigateNode node)
    {
        if (State == DogState.Chase) {
            //Chasing player, don't investigate
            node.ClearNode();
            return;
        }
        if (State == DogState.Investigate) {
            //Already investigating, prioritize new source
            _activeInvestigateNode = node;
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
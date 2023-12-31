using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogStatus : MonoBehaviour
{
    [SerializeField] DogMovement _move;
    [SerializeField] float _durationIdle;
    [SerializeField] float _durationDelayChase;
    [SerializeField] float _distanceLeash;
    [SerializeField] float _distanceAttack;
    [SerializeField] float _durationDelayInvestigate;
    [SerializeField] GameEventVoid _evDogInvestigating;
    [SerializeField] GameEventVoid _evDogChase;
    [SerializeField] GameEventInvestigateNode _evInvestigateNodeAlert;
    [SerializeField] GameEventPatrolNode _evPatrolNodeInit;
    [SerializeField] GameEventTransform _evPlayerDetected;
    [SerializeField] GameEventVoid _evPlayerLost;
    [SerializeField] GameEventVoid _evPlayerAttacked;
    [SerializeField] GameEventDogState _evDogStateEntered;
    [SerializeField] AudioSource _as;
    [SerializeField] AudioClip _sfxBeginInvestigate;
    [SerializeField] AudioClip _sfxBeginChase;
    [SerializeField] AudioClip _sfxPatrol;

    public DogState State { get; private set; }

    List<PatrolNode> _patrolNodes = new();
    PatrolNode _targetPatrolNode;
    PatrolNode _lastPatrolNode;
    InvestigateNode _activeInvestigateNode;

    Coroutine _activeBehavior = null;

    Transform _playerTarget = null;

    float _idleTime = 0f;

    private void Start()
    {
        State = DogState.Idle;
        EnterState(State);
    }
    private void OnEnable()
    {
        _evInvestigateNodeAlert.Subscribe(HandleInvestigateAlert);
        _evPatrolNodeInit.Subscribe(TrackPatrolNode);
        _evPlayerDetected.Subscribe(PlayerDetected);
    }
    private void OnDisable()
    {
        _evInvestigateNodeAlert.Unsubscribe(HandleInvestigateAlert);
        _evPatrolNodeInit.Unsubscribe(TrackPatrolNode);
        _evPlayerDetected.Unsubscribe(PlayerDetected);
    }

    public void ChangeState(DogState newState)
    {
        //print($"Dog exiting state {State}");
        ExitState(State);
        State = newState;
        //print($"Dog entering state {newState}");
        EnterState(State);
        _evDogStateEntered.TriggerEvent(State);
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
            _as.PlayOneShot(_sfxPatrol);
            _activeBehavior = StartCoroutine(BehavePatrol());
        }
        void EnterInvestigate()
        {
            _activeBehavior = StartCoroutine(BehaveInvestigate());
        }
        void EnterChase()
        {
            _as.PlayOneShot(_sfxBeginChase);
            _activeBehavior = StartCoroutine(BehaveChase());
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
        _move.CancelMove();

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
    #region Chase
    IEnumerator BehaveChase()
    {
        yield return TaroH.GetWait(_durationDelayChase);

        _move.MoveToPosition(_playerTarget.position, DogState.Chase);
        while (PlayerInRange()) {
            _move.UpdateTargetPos(_playerTarget.position);
            if (Vector3.Distance(transform.position, _playerTarget.position) < _distanceAttack) {
                ChaseAttackPlayer();
            }
            yield return null;
        }

        if (!PlayerInRange()) _evPlayerLost.TriggerEvent();

        _playerTarget = null;
        _activeBehavior = null;
        ChangeState(DogState.Idle);
        //TODO: Add investigate node @ player's last position?
    }

    void ChaseAttackPlayer()
    {
        //print("GAME OVER, MAN!");
        _evPlayerAttacked.TriggerEvent();
        Destroy(gameObject, 1.5f);
    }

    bool PlayerInRange()
    {
        if (_playerTarget == null) return false;
        float dist = Vector3.Distance(transform.position, _playerTarget.position);
        if (dist < _distanceLeash) return true;
        return false;
    }
    #endregion

    #region Investigate
    IEnumerator BehaveInvestigate()
    {
        yield return TaroH.GetWait(_durationDelayInvestigate);

        Vector3 targetPos = _activeInvestigateNode.WorldPos;
        _move.MoveToPosition(targetPos, DogState.Investigate);

        _as.PlayOneShot(_sfxBeginInvestigate);

        while (_move.Moving) {
            if (_activeInvestigateNode.WorldPos != targetPos) {
                //Target changed
                targetPos = _activeInvestigateNode.WorldPos;
                _move.MoveToPosition(_activeInvestigateNode.WorldPos, DogState.Investigate);
            }
            yield return null;
        }

        _activeBehavior = null;
        _activeInvestigateNode = null;

        ChangeState(DogState.Idle);
    }
    #endregion

    #region Patrol
    IEnumerator BehavePatrol()
    {
        while (_targetPatrolNode == null) {
            _targetPatrolNode = SelectPatrolNode();
            yield return null;
        }
        _move.MoveToPosition(_targetPatrolNode.WorldPos, DogState.Patrol);

        while (_move.Moving) {
            yield return null;
        }

        _targetPatrolNode.Visit();
        _lastPatrolNode = _targetPatrolNode;
        _targetPatrolNode = null;
        _activeBehavior = null;

        ChangeState(DogState.Idle);
    }
    PatrolNode SelectPatrolNode()
    {
        if (_patrolNodes == null || _patrolNodes.Count < 1) {
            return null;
        }

        PatrolNode tarNode = null;
        float minTime = Time.time;
        //print($"Mintime={minTime}");
        foreach (var node in _patrolNodes) {
            if (node.LastVisitTime <= minTime) {
                tarNode = node;
                minTime = node.LastVisitTime;
            }
        }
        //print($"Node is {tarNode}");
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
        } else {
            _activeInvestigateNode = node;
            ChangeState(DogState.Investigate);
        }
    }
    void TrackPatrolNode(PatrolNode node)
    {
        //print("NODE REGISTERED");
        _patrolNodes.Add(node);
    }

    void PlayerDetected(Transform pcTransform)
    {
        if (State == DogState.Chase) return;
        _playerTarget = pcTransform;
        ChangeState(DogState.Chase);
    }

}

public enum DogState
{
    Idle,
    Patrol,
    Investigate,
    Chase
}
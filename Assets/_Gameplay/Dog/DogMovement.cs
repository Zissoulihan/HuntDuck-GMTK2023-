using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [SerializeField] Transform _tfFacing;
    [SerializeField] float _moveSpeedPatrol;
    [SerializeField] float _moveSpeedInvestigate;
    [SerializeField] Vector2 _moveSpeedChase;
    [SerializeField] float _durationChaseAccel;
    [SerializeField] float _moveDelaySeconds;
    [SerializeField] float _goalProximityTolerance;

    public bool Moving => _activeMovement != null;

    Coroutine _activeMovement = null;

    Vector3 _targetPos;

    float _xScale;
    bool _changeFacing;
    int _facingDir = 1;

    private void Awake()
    {
        _xScale = _tfFacing.localScale.x;
    }
    public void UpdateTargetPos(Vector3 pos)
    {
        _targetPos = pos;
    }
    public void MoveToPosition(Vector3 pos, DogState state)
    {
        if (Moving) StopCoroutine(_activeMovement);
        _targetPos = pos;
        _activeMovement = StartCoroutine(MoveToTarget(state));
    }
    public void CancelMove()
    {
        if (Moving) StopCoroutine(_activeMovement);
        _activeMovement = null;
    }
    IEnumerator MoveToTarget(DogState state)
    {
        //print($"MoveToTarget");
        WaitForSeconds delay = TaroH.GetWait(_moveDelaySeconds);
        float spd = _moveSpeedPatrol;
        switch (state) {
            case DogState.Idle:
            case DogState.Patrol:
                spd = _moveSpeedPatrol;
                break;
            case DogState.Investigate:
                spd = _moveSpeedInvestigate;
                break;
            case DogState.Chase:
                spd = _moveSpeedChase.x;
                break;
            default:
                break;
        }

        float startTime = Time.time;
        Vector3 origPos = transform.position;

        while (Vector3.Distance(transform.position,_targetPos) > _goalProximityTolerance) {
            if (state == DogState.Chase) {
                float t = Time.time - startTime / _durationChaseAccel;
                spd = Mathf.Lerp(_moveSpeedChase.x, _moveSpeedChase.y, t);
            }
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, spd);
            //RotateFacing(origPos);
            CheckFacing(transform.position - origPos);
            yield return delay;
        }

        //Target reached
        _activeMovement = null;
    }

    void CheckFacing(Vector3 moveDir)
    {
        if (moveDir.x == 0) return;
        var moveLRDir = (int)Mathf.Sign(moveDir.x);
        if (_facingDir != moveLRDir) {
            _facingDir = moveLRDir;
            _changeFacing = true;
        }
    }

    private void LateUpdate()
    {
        UpdateFacing();
    }
    void UpdateFacing()
    {
        if (!_changeFacing) return;
        _tfFacing.localScale = new(_xScale * _facingDir, _tfFacing.localScale.y, _tfFacing.localScale.z);
        _changeFacing = false;
    }
    void RotateFacing(Vector3 origPos)
    {
        Vector3 moveDirection = transform.position - origPos;
        if (moveDirection != Vector3.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            _tfFacing.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

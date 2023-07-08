using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _moveDelaySeconds;
    [SerializeField] float _goalProximityTolerance;

    public bool Moving => _activeMovement != null;

    Coroutine _activeMovement = null;

    Vector3 _targetPos;

    public void MoveToPosition(Vector3 pos)
    {
        if (Moving) StopCoroutine(_activeMovement);
        _targetPos = pos;
        _activeMovement = StartCoroutine(MoveToTarget());
    }
    IEnumerator MoveToTarget()
    {
        print($"MoveToTarget");
        WaitForSeconds delay = TaroH.GetWait(_moveDelaySeconds);

        while (Vector3.Distance(transform.position,_targetPos) > _goalProximityTolerance) {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveSpeed);
            yield return delay;
        }

        //Target reached
        _activeMovement = null;
        print($"MoveToTarget DONEZO");
    }
}

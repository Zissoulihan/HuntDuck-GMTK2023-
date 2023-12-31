using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MonoBehaviour
{
    [SerializeField] GameEventPatrolNode _evPatrolNodeInit;
    public Vector3 WorldPos { get; private set; }
    public float LastVisitTime { get; private set; } = 0f;

    private void Awake()
    {
        WorldPos = transform.position;
    }
    private void Start()
    {
        _evPatrolNodeInit.TriggerEvent(this);
    }
    public void Visit()
    {
        LastVisitTime = Time.time;
    }
}

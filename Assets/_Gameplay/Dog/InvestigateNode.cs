using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateNode : MonoBehaviour
{
    [SerializeField] GameEventInvestigateNode _evNodeSpawnAlert;

    public Vector3 WorldPos { get; private set; }

    private void Awake()
    {
        WorldPos = transform.position;
    }
    private void Start()
    {
        _evNodeSpawnAlert.TriggerEvent(this);
    }

    public void ClearNode()
    {
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSense : MonoBehaviour
{
    [SerializeField] GameEventTransform _evPlayerDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pc = collision.gameObject.GetComponentInParent<PlayerStatus>();
        if (pc == null) return;
        print("GOTTEM");
        _evPlayerDetected.TriggerEvent(collision.transform);
    }
   
}

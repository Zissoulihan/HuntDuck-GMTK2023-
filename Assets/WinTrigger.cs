using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] UIAnimatedElement _fade;
    [SerializeField] GameEventVoid _evGameWin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pc = collision.gameObject.GetComponentInParent<PlayerStatus>();
        if (pc == null) return;
        pc.BecomeImmortal();
        StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {

        _fade.Hide();

        while (_fade.Animating) yield return null;

        _evGameWin.TriggerEvent();
    }
}

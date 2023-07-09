using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] UIAnimatedElement _fade;
    [SerializeField] GameEventVoid _evGameWin;
    [SerializeField] DogStatus _doggo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pc = collision.gameObject.GetComponentInParent<PlayerStatus>();
        if (pc == null) return;
        pc.BecomeImmortal();
        Destroy(_doggo.gameObject);
        StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {

        _fade.Hide();

        while (_fade.Animating) yield return null;

        _evGameWin.TriggerEvent();
    }
}

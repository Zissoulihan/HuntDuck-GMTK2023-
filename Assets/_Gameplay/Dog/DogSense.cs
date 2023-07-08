using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSense : MonoBehaviour
{
    [SerializeField] GameEventTransform _evPlayerDetected;
    [SerializeField] Color _colConeDefault;
    [SerializeField] Color _colConeAlert;
    [SerializeField] Color _colConeChase;
    [SerializeField] float _durationColorChange;
    [SerializeField] SpriteRenderer _sprCone;
    [SerializeField] Transform _sightOrigin;
    [SerializeField] LayerMask _sightBlockingLayer;
    [SerializeField] GameEventDogState _evDogStateChanged;

    private void OnEnable()
    {
        _evDogStateChanged.Subscribe(ColorCone);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pc = collision.gameObject.GetComponentInParent<PlayerStatus>();
        if (pc == null) return;
        if (!EyesOnPlayer(pc.transform)) return;

        print("GOTTEM");
        _evPlayerDetected.TriggerEvent(collision.transform);
    }

    bool EyesOnPlayer(Transform tPc)
    {
        var dir = tPc.position - _sightOrigin.position;
        var dist = Vector3.Distance(_sightOrigin.position, tPc.position);
        return !Physics2D.Raycast(_sightOrigin.position, dir, dist, _sightBlockingLayer);
    }

    void ColorCone(DogState state)
    {
        Color c = _colConeDefault;
        switch (state) {
            case DogState.Idle:
                break;
            case DogState.Patrol:
                break;
            case DogState.Investigate:
                c = _colConeAlert;
                break;
            case DogState.Chase:
                c = _colConeChase;
                break;
            default:
                break;
        }

        StopAllCoroutines();
        StartCoroutine(AnimConeColor(c));
    }

    IEnumerator AnimConeColor(Color c)
    {
        float startTime = Time.time;
        Color startCol = _sprCone.color;
        while(Time.time < startTime + _durationColorChange) {
            float t = (Time.time - startTime) / _durationColorChange;
            _sprCone.color = Color.Lerp(startCol, c, t);
            yield return null;
        }
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _tfPlayer;
    [SerializeField] Transform _tfFacingPivot;
    [SerializeField] GameEventVector2 _evMoveInput;
    [SerializeField] float _moveSpeed;

    Vector2 _moveInput = new();
    int _facingDir = 1;
    float _xScale;

    private void Awake()
    {
        _xScale = _tfPlayer.localScale.x;
    }
    private void OnEnable()
    {
        _evMoveInput.Subscribe(UpdateMoveInput);
    }
    private void OnDisable()
    {
        _evMoveInput.Unsubscribe(UpdateMoveInput);
    }
    private void Update()
    {
        MovePlayer();
    }
    private void LateUpdate()
    {
        SetSpriteFacing();
    }

    void SetSpriteFacing()
    {
    }

    void MovePlayer()
    {
        if (_moveInput == Vector2.zero) return;

        _tfPlayer.position += new Vector3(_moveInput.x * _moveSpeed * Time.deltaTime, _moveInput.y * _moveSpeed * Time.deltaTime, 0f);
    }

    void UpdateMoveInput(Vector2 input)
    {
        _moveInput = input;
    }
}

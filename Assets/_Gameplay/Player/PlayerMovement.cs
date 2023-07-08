using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform _tfPlayer;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _tfFacingPivot;
    [SerializeField] GameEventVector2 _evMoveInput;
    [SerializeField] float _moveSpeedDefault;
    [SerializeField] float _moveSpeedChased;
    [SerializeField] GameEventDogState _evDogStateChanged;
    [SerializeField] GameEventVoid _evAttackedDefeat;

    Vector2 _moveInput = new();

    bool _newFacing = false;
    float _xScale;
    int _facingDir = 1;
    float _currentSpeed;

    private void Awake()
    {
        _xScale = _tfFacingPivot.localScale.x;
        _currentSpeed = _moveSpeedDefault;
    }

    private void OnEnable()
    {
        _evMoveInput.Subscribe(UpdateMoveInput);
        _evDogStateChanged.Subscribe(CheckDogState);
        _evAttackedDefeat.Subscribe(Defeat);
    }
    private void OnDisable()
    {
        _evMoveInput.Unsubscribe(UpdateMoveInput);
        _evDogStateChanged.Unsubscribe(CheckDogState);
        _evAttackedDefeat.Unsubscribe(Defeat);
    }
    private void FixedUpdate()
    {
        MovePlayer();
        CheckFacing();
    }
    private void LateUpdate()
    {
        SetSpriteFacing();
    }

    void SetSpriteFacing()
    {
        if (!_newFacing) return;
        _tfFacingPivot.localScale = new(_xScale * _facingDir, _tfFacingPivot.localScale.y, _tfFacingPivot.localScale.z);
        _newFacing = false;
    }

    void MovePlayer()
    {
        if (_moveInput == Vector2.zero) return;

        //_tfPlayer.position += new Vector3(_moveInput.x * _moveSpeed * Time.deltaTime, _moveInput.y * _moveSpeed * Time.deltaTime, 0f);
        _rb.MovePosition(_tfPlayer.position + new Vector3(_moveInput.x * _currentSpeed, _moveInput.y * _currentSpeed, 0f));
    }

    void CheckFacing()
    {
        if ((int)_moveInput.x == 0) return;
        var moveLRDir = (int)Mathf.Sign(_moveInput.x);
        if (_facingDir != moveLRDir) {
            _facingDir = moveLRDir;
            _newFacing = true;
        }
    }

    void UpdateMoveInput(Vector2 input)
    {
        _moveInput = input;
    }

    void CheckDogState(DogState state)
    {
        if (state != DogState.Chase) {
            _currentSpeed = _moveSpeedDefault;
            return; 
        }
        _currentSpeed = _moveSpeedChased;
    }
    void Defeat()
    {
        _currentSpeed = 0f;
    }
}

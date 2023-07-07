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

    bool _newFacing = false;
    float _xScale;
    int _facingDir = 1;

    private void Awake()
    {
        _xScale = _tfFacingPivot.localScale.x;
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

        _tfPlayer.position += new Vector3(_moveInput.x * _moveSpeed * Time.deltaTime, _moveInput.y * _moveSpeed * Time.deltaTime, 0f);
    }

    void CheckFacing()
    {
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
}
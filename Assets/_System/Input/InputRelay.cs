using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRelay : MonoBehaviour
{
    [SerializeField] GameEventVoid _evLMBDown;
    [SerializeField] GameEventVoid _evLMBUp;
    [SerializeField] GameEventVoid _evRMBDown;
    [SerializeField] GameEventVoid _evRMBUp;
    [SerializeField] GameEventVector2 _evMousePos;
    [SerializeField] GameEventVector2 _evMoveInput;
    [SerializeField] GameEventVoid _evSpacebarDown;
    [SerializeField] GameEventVoid _evSpacebarUp;

    public void RelayLMBToggle(InputAction.CallbackContext context)
    {
        if (context.started) _evLMBDown.TriggerEvent();
        else if (context.canceled) _evLMBUp.TriggerEvent();
    }
    public void RelayRMBToggle(InputAction.CallbackContext context)
    {
        if (context.started) _evRMBDown.TriggerEvent();
        else if (context.canceled) _evRMBUp.TriggerEvent();
    }
    public void RelayMousePos(InputAction.CallbackContext context)
    {
        _evMousePos.TriggerEvent(context.ReadValue<Vector2>());
    }
    public void RelayMoveInput(InputAction.CallbackContext context)
    {
        _evMoveInput.TriggerEvent(context.ReadValue<Vector2>());
    }
    public void RelaySpacebarToggle(InputAction.CallbackContext context)
    {
        if (context.started) _evSpacebarDown.TriggerEvent();
        else if (context.canceled) _evSpacebarUp.TriggerEvent();
    }
}

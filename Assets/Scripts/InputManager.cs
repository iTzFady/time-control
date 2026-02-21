using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
    private PlayerControls inputActions;
    private InputAction lookAction;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        inputActions = new PlayerControls();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lookAction = inputActions.Movement.Look;
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    public Vector2 GetPlayerMovement()
    {
        return inputActions.Movement.Move.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return new Vector2(inputActions.Movement.Look.ReadValue<Vector2>().x, inputActions.Movement.Look.ReadValue<Vector2>().y);
    }
    public bool Sprint()
    {
        return inputActions.Movement.Sprint.IsPressed();
    }
    public bool Jump()
    {
        return inputActions.Movement.Jump.IsPressed();
    }
    public bool Rewind()
    {
        return inputActions.Actions.Rewinding.IsPressed();
    }
    public bool FastForward()
    {
        return inputActions.Actions.FastForwarding.IsPressed();
    }
    public bool Pause()
    {
        return inputActions.Actions.Pause.WasPressedThisFrame();
    }
    public bool Carry()
    {
        return inputActions.Actions.Carry.IsPressed();
    }


    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

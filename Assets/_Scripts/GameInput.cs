using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPauseAction;

    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one GameInput");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Pause.performed -= Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementNormalised()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector.Normalize();

        return inputVector; 
    }
}

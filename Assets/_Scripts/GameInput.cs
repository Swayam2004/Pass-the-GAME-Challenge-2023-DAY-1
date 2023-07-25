using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

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
    }

    public Vector2 GetMovementNormalised()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector.Normalize();

        return inputVector; 
    }
}

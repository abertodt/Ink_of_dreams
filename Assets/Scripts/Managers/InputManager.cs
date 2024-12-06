using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput _playerInput;

    public bool IsMoving { get; private set; }

    public Vector2 MovementInput { get; private set; }

    public bool IsRunning { get; private set; }
    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Input Master
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        // Enable the input actions
        _playerInput.Enable();

        #region MovementInput

        _playerInput.CharacterControlls.Move.started += OnMovementInput;
        _playerInput.CharacterControlls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControlls.Move.performed += OnMovementInput;

        #endregion MovementInput

        #region RunInput

        _playerInput.CharacterControlls.Run.started += OnRunInput;
        _playerInput.CharacterControlls.Run.canceled += OnRunInput;
        _playerInput.CharacterControlls.Run.performed += OnRunInput;

        #endregion RunInput
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _playerInput.CharacterControlls.Move.started -= OnMovementInput;
        _playerInput.CharacterControlls.Move.canceled -= OnMovementInput;
        _playerInput.CharacterControlls.Move.performed -= OnMovementInput;

        _playerInput.CharacterControlls.Run.started -= OnRunInput;
        _playerInput.CharacterControlls.Run.canceled -= OnRunInput;
        _playerInput.CharacterControlls.Run.performed -= OnRunInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        IsMoving = MovementInput != Vector2.zero;
    }

    private void OnRunInput(InputAction.CallbackContext context)
    {
        IsRunning = context.ReadValueAsButton();
    }
}

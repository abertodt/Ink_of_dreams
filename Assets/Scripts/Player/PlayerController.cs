using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _runMultiplier;
    [SerializeField] private float _rotationSpeed;


    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private Animator _animator;

    private Vector2 _currentMovementInput;
    private Vector3 _currentWalkingMovement;
    private Vector3 _currentRunMovement;
    private Vector3 _currentMovement;

    private bool _isMovementPressed;
    private bool _isRunningPressed;

    int _isRunningHash;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        _isRunningHash = Animator.StringToHash("isRunning");

        _playerInput.CharacterControlls.Move.started += OnMovementInput;
        _playerInput.CharacterControlls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControlls.Move.performed += OnMovementInput;

        _playerInput.CharacterControlls.Run.started += OnRunInput;
        _playerInput.CharacterControlls.Run.canceled += OnRunInput;
        _playerInput.CharacterControlls.Run.performed += OnRunInput;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentWalkingMovement.x = _currentMovementInput.x;
        _currentWalkingMovement.z = _currentMovementInput.y;
        _currentRunMovement.x = _currentWalkingMovement.x * _runMultiplier;
        _currentRunMovement.z = _currentWalkingMovement.z * _runMultiplier;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    private void OnRunInput(InputAction.CallbackContext context) 
    {
        _isRunningPressed = context.ReadValueAsButton();
    }

    private void HandleAnimations()
    {
        bool isRunning = _animator.GetBool(_isRunningHash);   
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _currentMovement.z;


        if(_isMovementPressed) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        if(_isRunningPressed) 
        {
            _currentMovement = _currentRunMovement;
        }
        else
        {
            _currentMovement = _currentWalkingMovement;
        }
        _characterController.SimpleMove(_currentMovement * _movementSpeed);
    }

    private void OnEnable()
    {
        _playerInput.CharacterControlls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControlls.Disable();
    }
}

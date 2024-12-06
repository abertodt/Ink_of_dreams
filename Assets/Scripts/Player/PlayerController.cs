using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PausableMonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _runMultiplier;
    [SerializeField] private float _rotationSpeed;


    private CharacterController _characterController;
    private Animator _animator;

    private Vector3 _currentWalkingMovement;
    private Vector3 _currentMovement;


    int _isRunningHash;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        _isRunningHash = Animator.StringToHash("isRunning");
    }

    private void Move()
    {
        _currentWalkingMovement = new Vector3(InputManager.Instance.MovementInput.x, 0f, InputManager.Instance.MovementInput.y);

        _currentMovement = InputManager.Instance.IsRunning
            ? _currentWalkingMovement * _runMultiplier
            : _currentWalkingMovement;
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


        if(InputManager.Instance.IsMoving && positionToLookAt != Vector3.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        
    }

    protected override void OnPausableUpdate()
    {
        HandleRotation();
        Move();
        _characterController.SimpleMove(_currentMovement * _movementSpeed);
    }
}

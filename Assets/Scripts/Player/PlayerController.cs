using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _movementSpeed;


    private CharacterController _characterController;
    float _horizontalInput;
    float _verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_horizontalInput, 0, _verticalInput) * _movementSpeed;
        movement.Normalize();
        _characterController.SimpleMove(movement);
    }
}

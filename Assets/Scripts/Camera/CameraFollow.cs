using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float _FOV;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private float _followHeight;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _minPitchRotation;
    [SerializeField] private float _maxPitchRotation;

    private float _yaw = 0f;
    private float _pitch = 0f;

    private void Start()
    {
        Camera.main.fieldOfView = _FOV;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
        transform.position = _target.position - _target.forward * _distanceToTarget + Vector3.up * _followHeight;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = _target.position - _target.forward * _distanceToTarget + Vector3.up * _followHeight;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);

        _yaw += InputManager.Instance.CameraRotation.x * _rotationSpeed;
        _pitch -= InputManager.Instance.CameraRotation.y * _rotationSpeed;
        _pitch = Mathf.Clamp(_pitch, _minPitchRotation, _maxPitchRotation);



        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);

        Vector3 directionToTarget = _target.position - _camera.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(directionToTarget);

        Quaternion pitchRotation = Quaternion.Euler(_pitch, 0f, 0f);
        _camera.localRotation = lookAtRotation * pitchRotation;
    }
}

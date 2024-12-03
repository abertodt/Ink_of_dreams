using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CameraSettings")]
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _FOV;

    private void Start()
    {
        transform.position = transform.parent.position + new Vector3(0, transform.position.y, -_distanceToPlayer);
        Camera.main.fieldOfView = _FOV;
    }

    private void Update()
    {
    }
}

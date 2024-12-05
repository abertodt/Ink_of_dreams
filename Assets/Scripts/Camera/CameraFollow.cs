using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("CameraSettings")]
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _height;
    [SerializeField] private float _FOV;

    private void Start()
    {
        //transform.localPosition = transform.parent.position + new Vector3(0, _height, -_distanceToPlayer); 
        Camera.main.fieldOfView = _FOV;
    }

    private void Update()
    {
    }
}

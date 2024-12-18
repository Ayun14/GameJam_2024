using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [Header("Meter")]
    [SerializeField] private Transform _playerTrm;
    [SerializeField] private int _meterY; // 얼마의 y값이 1M가 될 것 인지.

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _camera;

    private int _meter = 0;
    public int Meter => _meter;

    private void Start()
    {
        _meter = 0;
    }

    private void FixedUpdate()
    {
        MeterCalculation();
    }

    private void MeterCalculation()
    {
        _meter = (int)_playerTrm.position.y / _meterY;
    }
}

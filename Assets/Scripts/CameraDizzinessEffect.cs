using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraDizzinessEffect : MonoBehaviour
{
    public float EffectIntensity = 1f;
    private CinemachineVirtualCamera _virtualCamera;
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        float dizziness = GameManager.Instance.Player.Dizziness * EffectIntensity * -1;
        float dutchValue = Utils.Remap(-0.5f, 0.5f, -180f, 180f, dizziness);
        _virtualCamera.m_Lens.Dutch = dutchValue;
    }
}

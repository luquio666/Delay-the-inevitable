using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class PeeMeterController
{
    public float StartingFillSpeed = 0.01f;
    public float FillAcceleration = 0.001f;
    public float PeeHoldDelta = 0.05f;

    [Range(0f, 1f)] public float _currentValue = 0;
    private float _currentFillSpeed;
    private bool _initialized = false;
    private float _currentPeeParticlesRatePerSecond = 0;
    private PeeParticlesController _peeParticlesController;

    public void Initialize()
    {
        _currentValue = 0;
        _currentFillSpeed = StartingFillSpeed;
        _peeParticlesController = GameManager.Instance.Player.GetComponent<PeeParticlesController>();
        _peeParticlesController.SetRateOverTime(0);
        _initialized = true;
    }

    public void Update(float deltaTime)
    {
        if (!_initialized)
            return;

        HandleBarFill(deltaTime);
        HandlePeeParticlesAndSound();
    }

    private void HandlePeeParticlesAndSound()
    {
        if (_currentValue >= 1f)
        {
            if (_currentPeeParticlesRatePerSecond < 100)
            {
                _currentPeeParticlesRatePerSecond = 100;
                _peeParticlesController.SetRateOverTime(_currentPeeParticlesRatePerSecond);
            }
        }
        else if (_currentValue > 0.7f)
        {
            if (_currentPeeParticlesRatePerSecond < 4)
            {
                _currentPeeParticlesRatePerSecond = 4;
                _peeParticlesController.SetRateOverTime(_currentPeeParticlesRatePerSecond);
            }
        }
        else if (_currentValue > 0.5f)
        {
            if (_currentPeeParticlesRatePerSecond < 2)
            {
                _currentPeeParticlesRatePerSecond = 2;
                _peeParticlesController.SetRateOverTime(_currentPeeParticlesRatePerSecond);
            }
        }
        else if (_currentPeeParticlesRatePerSecond > 0)
        {
            _currentPeeParticlesRatePerSecond = 0;
            _peeParticlesController.SetRateOverTime(_currentPeeParticlesRatePerSecond);
        }
    }

    private void HandleBarFill(float deltaTime)
    {
        _currentValue = Mathf.Min(_currentValue + _currentFillSpeed * deltaTime, 1);
        _currentFillSpeed += FillAcceleration * deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            _currentValue = Mathf.Max(_currentValue - PeeHoldDelta, 0);
        }

        if (_currentValue >= 1)
        {
            Stop();
            HandlePeeParticlesAndSound();
            GameEvents.GameOver();
        }
    }

    public void Stop()
    {
        this._initialized = false;
    }
}
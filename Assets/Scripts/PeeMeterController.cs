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

    [Range(0f,1f)] public float _currentValue = 0;
    private float _currentFillSpeed;
    private bool _initialized = false;

    public void Initialize()
    {
        _currentValue = 0;
        _currentFillSpeed = StartingFillSpeed;
        _initialized = true;
    }

    public void Update(float deltaTime)
    {
        if (!_initialized)
            return;

        HandleBarFill(deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            _currentValue = Mathf.Max(_currentValue - PeeHoldDelta, 0);
        }
    }

    private void HandleBarFill(float deltaTime)
    {
        _currentValue = Mathf.Min(_currentValue + _currentFillSpeed * deltaTime, 1);
        _currentFillSpeed += FillAcceleration * deltaTime;

        if (_currentValue >= 1)
        {
            GameEvents.GameOver();
            Stop();
        }
    }

    public void Stop()
    {
        this._initialized = false;
    }
}
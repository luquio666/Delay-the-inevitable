using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoolToPee : MonoBehaviour
{
    public float TriggerRange = 2f;
    private bool _isInside = false;
    private GameObject _player;

    private void Awake()
    {
        _player = GameManager.Instance.Player;
    }

    private void Update()
    {
        HandlePlayerDetection();
    }

    private void HandlePlayerDetection()
    {
        if (!_isInside && Vector3.Distance(transform.position, _player.transform.position) < TriggerRange)
        {
            _isInside = true;
        }
        else if (_isInside && Vector3.Distance(transform.position, _player.transform.position) > TriggerRange)
        {
            _isInside = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoolToPeeActions
{
   PICK_UP,
   DROP,
   PLACE_IN_TARGET
}

public class StoolToPee : MonoBehaviour
{
    public float TriggerRange = 2f;
    public Transform TargetMarker;
    
    private bool _isInside = false;
    private RagdollMovement _player;
    private bool _isPickedUp = false;
    private Rigidbody _rigidbody;
    
    public static Action<StoolToPeeActions> OnStoolInteraction;

    private void Awake()
    {
        _player = GameManager.Instance.Player;
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        HandlePlayerDetection();
        HandlePickUpDetection();
    }

    private void HandlePickUpDetection()
    {
        if (_isInside && Input.GetKeyDown(KeyCode.F))
        {
            if (!_isPickedUp)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
    }

    private void PickUp()
    {
        _isPickedUp = true;
        _player.Grab(_rigidbody);
        GameEvents.ClearHeadBubbleMsg();
        OnStoolInteraction?.Invoke(StoolToPeeActions.PICK_UP);
    }
    
    private void Drop()
    {
        _isPickedUp = false;
        _player.Drop(_rigidbody);
        OnStoolInteraction?.Invoke(StoolToPeeActions.DROP);
        if (Vector3.Distance(_rigidbody.transform.position, TargetMarker.position) < 3)
        {
            OnStoolInteraction?.Invoke(StoolToPeeActions.PLACE_IN_TARGET);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isInside ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, TriggerRange);
    }

    private void HandlePlayerDetection()
    {
        if (_isPickedUp) return;
        if (!_isInside && Vector3.Distance(_rigidbody.transform.position, GameManager.Instance.PlayerPosition) < TriggerRange)
        {
            _isInside = true;
            GameEvents.SendHeadBubbleMsg("Press to pick");
        }
        else if (_isInside && Vector3.Distance(_rigidbody.transform.position, GameManager.Instance.PlayerPosition) > TriggerRange)
        {
            _isInside = false;
            GameEvents.ClearHeadBubbleMsg();
        }
    }
}
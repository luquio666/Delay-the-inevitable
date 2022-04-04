using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorTriggerActions
{
    NOTHING,
    OPEN_DOOR,
    CLOSE_DOOR,
    PRESS_F_TO_OPEN
}

public class DoorTrigger : MonoBehaviour
{
    public static Action<DoorTriggerActions, string> OnDoorTriggerAction;

    public string Id;
    public DoorTriggerActions DoorAction;
    public bool Locked;
    public Animator DoorAnimation;
    public string DoorAnimName;

    private bool _actionAlreadyTriggered;
    private bool _buttonAlreadyPressed;
    private bool _inside = false;

    private void OnEnable()
    {
        GameEvents.OnLockDoor += LockDoor;
        GameEvents.OnUnlockDoor += UnlockDoor;
    }

    private void OnDisable()
    {
        GameEvents.OnLockDoor -= LockDoor;
        GameEvents.OnUnlockDoor -= UnlockDoor;
    }

    private void LockDoor(string targetId)
    {
        if (targetId == Id)
        {
            Locked = true;
        }
    }

    private void UnlockDoor(string targetId)
    {
        if (targetId == Id)
        {
            Locked = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.PLAYER_TAG))
        {
            if (!_actionAlreadyTriggered)
                TriggerActions();
            _inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.PLAYER_TAG))
        {
            _inside = false;
        }
    }

    private void TriggerActions()
    {
        TriggerActionsInternal(DoorAction);
    }

    private void TriggerActionsInternal(DoorTriggerActions action)
    {
        switch (action)
        {
            case DoorTriggerActions.NOTHING:
                break;
            case DoorTriggerActions.OPEN_DOOR:
                _actionAlreadyTriggered = true;
                DoorAnimation.Play(DoorAnimName, 0, 0f);
                GameEvents.PlaySound("door opening", false);
                break;
            case DoorTriggerActions.CLOSE_DOOR:
                _actionAlreadyTriggered = true;
                DoorAnimation.Play(DoorAnimName, 0, 0f);
                GameEvents.PlaySound("door opening 2", false);
                break;
            case DoorTriggerActions.PRESS_F_TO_OPEN:
                if (Locked) break;
                _actionAlreadyTriggered = true;
                if (!_buttonAlreadyPressed)
                    StartCoroutine(HideHeadBubbleCo());
                break;
            default:
                break;
        }

        OnDoorTriggerAction?.Invoke(action, Id);
    }

    private void Update()
    {
        if (_inside && Input.GetKeyDown(KeyCode.F) && !_buttonAlreadyPressed &&
            DoorTriggerActions.PRESS_F_TO_OPEN == DoorAction)
        {
            if (Locked)
            {
                GameEvents.PlaySound("hit door", false);
                return;
            }

            _buttonAlreadyPressed = true;
            TriggerActionsInternal(DoorTriggerActions.OPEN_DOOR);
        }
    }

    private IEnumerator HideHeadBubbleCo()
    {
        GameEvents.SendHeadBubbleMsg("Press to Open");
        yield return new WaitForSeconds(3f);
        GameEvents.ClearHeadBubbleMsg();
    }
}
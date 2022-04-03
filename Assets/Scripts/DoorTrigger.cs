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

    bool _actionAlreadyTriggered;
    bool _buttonAlreadyPressed;

    private void OnEnable()
    {
        GameEvents.OnDoorUnlocked += OnDoorUnlocked;
        GameEvents.OnDoorLocked += OnDoorLocked;
    }

    private void OnDisable()
    {
        GameEvents.OnDoorUnlocked -= OnDoorUnlocked;
        GameEvents.OnDoorLocked -= OnDoorLocked;
    }

    private void OnDoorUnlocked(string id)
    {
        if (id == Id)
        {
            Locked = false;
            Debug.Log($"{id} door unlocked!".Color(Color.yellow));
        }
    }
    
    private void OnDoorLocked(string id)
    {
        if (id == Id)
        {
            Locked = true;
            Debug.Log($"{id} door locked!".Color(Color.yellow));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.PLAYER_TAG) && !_actionAlreadyTriggered)
        {
            TriggerActions();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DoorAction == DoorTriggerActions.PRESS_F_TO_OPEN)
        {
            GameEvents.ClearUIMessages();
        }
    }

    private void TriggerActions()
    {
        TriggerActionsInternal(DoorAction);
    }

    private void TriggerActionsInternal(DoorTriggerActions action)
    {
        _actionAlreadyTriggered = true;
        switch (action)
        {
            case DoorTriggerActions.NOTHING:
                break;
            case DoorTriggerActions.OPEN_DOOR:
                if (!Locked)
                    DoorAnimation.Play(DoorAnimName, 0, 0f);
                break;
            case DoorTriggerActions.CLOSE_DOOR:
                DoorAnimation.Play(DoorAnimName, 0, 0f);
                break;
            case DoorTriggerActions.PRESS_F_TO_OPEN:
                if (!_buttonAlreadyPressed && !Locked)
                    GameEvents.SendUIMessage("Press F to open.", UIMessageMode.TOOLTIP);
                break;
            default:
                break;
        }

        OnDoorTriggerAction?.Invoke(action, Id);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_buttonAlreadyPressed && !Locked && DoorTriggerActions.PRESS_F_TO_OPEN == DoorAction)
        {
            _buttonAlreadyPressed = true;
            TriggerActionsInternal(DoorTriggerActions.OPEN_DOOR);
        }
    }
}
using UnityEngine;
using System.Collections;
using System;

public enum UIMessageMode
{
    TOOLTIP,
    BABY_SPEAKING
}
public static class GameEvents {

    public static Action<string, bool> OnPlaySound;
    public static void PlaySound(string soundName, bool loop = false)
    {
        OnPlaySound?.Invoke(soundName, loop);
    }

    public static Action<string> OnStopSound;
    public static void StopSound(string soundName)
    {
        OnStopSound?.Invoke(soundName);
    }

    public static Action<string> OnChangeObjectState;
    public static void ChangeObjectState(string id)
    {
        OnChangeObjectState?.Invoke(id);
    }

    public static Action<string> OnSendUIMessage;
    public static void SendUIMessage(string msg, UIMessageMode mode)
    {
        OnSendUIMessage?.Invoke(msg);
    }

    public static Action OnClearUIMessages;
    public static void ClearUIMessages()
    {
        OnClearUIMessages?.Invoke();
    }

    public static Action<string> OnDoorUnlocked;
    public static void UnlockDoor(string id)
    {
        OnDoorUnlocked?.Invoke(id);
    }
    
    public static Action<string> OnDoorLocked;
    public static void LockDoor(string id)
    {
        OnDoorLocked?.Invoke(id);
    }
    
}

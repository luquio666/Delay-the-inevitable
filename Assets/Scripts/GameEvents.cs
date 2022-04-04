using UnityEngine;
using System.Collections;
using System;

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

    public static Action<string> OnSendHintsMsg;
    public static void SendHintsMsg(string msg)
    {
        OnSendHintsMsg?.Invoke(msg);
    }

    public static Action OnClearHintsMsg;
    public static void ClearHintsMsg()
    {
        OnClearHintsMsg?.Invoke();
    }

    public static Action<string> OnSendHeadBubbleMsg;
    public static void SendHeadBubbleMsg(string msg)
    {
        OnSendHeadBubbleMsg?.Invoke(msg);
    }

    public static Action OnClearHeadBubbleMsg;
    public static void ClearHeadBubbleMsg()
    {
        OnClearHeadBubbleMsg?.Invoke();
    }

    public static Action<string> OnLockDoor;
    public static void LockDoor(string target)
    {
        OnLockDoor?.Invoke(target);
    }

    public static Action<string> OnUnlockDoor;
    public static void UnlockDoor(string target)
    {
        OnUnlockDoor?.Invoke(target);
    }

    public static Action OnGameOver;
    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static Action OnGameWon;
    public static void GameWon()
    {
        OnGameWon?.Invoke();
    }

}

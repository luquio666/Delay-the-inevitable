using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaTriggerActions
{
    ENTER,
    STAY,
    EXIT
}
public class AreaTrigger : MonoBehaviour
{
    public string Id;
    public AreaTriggerActions AreaAction;
    public static Action<AreaTriggerActions, string> OnAreaTrigger;

    private void OnTriggerInternal(Collider other, AreaTriggerActions areaAction)
    {
        if (AreaAction == areaAction && other.CompareTag(GameConstants.PLAYER_TAG))
        {
            OnAreaTrigger?.Invoke(AreaAction, Id);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerInternal(other, AreaTriggerActions.ENTER);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerInternal(other, AreaTriggerActions.STAY);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerInternal(other, AreaTriggerActions.EXIT);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        DoorTrigger.OnDoorTriggerAction += OnDoorTriggerAction;
        AreaTrigger.OnAreaTrigger += OnAreaTrigger;
    }

    private void OnDisable()
    {
        DoorTrigger.OnDoorTriggerAction -= OnDoorTriggerAction;
        AreaTrigger.OnAreaTrigger -= OnAreaTrigger;
    }

    private void OnAreaTrigger(AreaTriggerActions areaAction, string id)
    {
        throw new NotImplementedException();
    }

    private void OnDoorTriggerAction(DoorTriggerActions doorAction, string id)
    {
        throw new NotImplementedException();
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameRequirement
{
    public Action CompleteAction;
    public abstract bool Evaluate(string id, string eventName);
}

public class GameRequirementDoor : GameRequirement
{
    public DoorTriggerActions DoorTriggerAction;
    public string Id;

    public GameRequirementDoor(string id, DoorTriggerActions action, Action complete = null)
    {
        Id = id;
        DoorTriggerAction = action;
        CompleteAction = complete;
    }

    public override bool Evaluate(string id, string eventName)
    {
        return Id == id && DoorTriggerAction.ToString() == eventName;
    }

    public override string ToString()
    {
        return $"Door {Id} {DoorTriggerAction}";
    }
}

public class GameRequirementArea : GameRequirement
{
    public string Id;
    public AreaTriggerActions AreaTriggerAction;
    
    public GameRequirementArea(string id, AreaTriggerActions action, Action complete = null)
    {
        CompleteAction = complete;
        Id = id;
        AreaTriggerAction = action;
    }
    
    public override bool Evaluate(string id, string eventName)
    {
        return Id == id && AreaTriggerAction.ToString() == eventName;
    }
    
    public override string ToString()
    {
        return $"Trigger area {Id} {AreaTriggerAction}";
    }
}

public class GameRequirementStoolToPee : GameRequirement
{
    public StoolToPeeActions StoolToPeeAction;
    
    public GameRequirementStoolToPee(StoolToPeeActions action, Action complete = null)
    {
        CompleteAction = complete;
        StoolToPeeAction = action;
    }
    
    public override bool Evaluate(string id, string eventName)
    {
        return StoolToPeeAction.ToString() == eventName;
    }
    
    public override string ToString()
    {
        return $"Stool to pee {StoolToPeeAction}";
    }
}

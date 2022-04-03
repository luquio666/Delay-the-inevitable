using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameRequirement
{
}

public class GameRequirementDoor : GameRequirement
{
    public DoorTriggerActions DoorTriggerAction;
}

public class GameRequirementArea : GameRequirement
{
    public AreaTriggerActions AreaTriggerAction;
}

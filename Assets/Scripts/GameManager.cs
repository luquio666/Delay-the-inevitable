using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Queue<GameRequirement> _gameRequirements = new Queue<GameRequirement>(new List<GameRequirement>()
    {
        new GameRequirementDoor("Bathroom", DoorTriggerActions.CLOSE_DOOR),
        new GameRequirementDoor("Baby'sRoom", DoorTriggerActions.OPEN_DOOR),
        new GameRequirementArea("Baby'sToilet", AreaTriggerActions.ENTER, () => GameEvents.UnlockDoor("ParentsRoom")),
        new GameRequirementDoor("ParentsRoom", DoorTriggerActions.OPEN_DOOR),
        new GameRequirementStoolToPee(StoolToPeeActions.PICK_UP),
        new GameRequirementStoolToPee(StoolToPeeActions.PLACE_IN_TARGET)
    });

    private GameRequirement _currentGameRequirement;


    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }


    public RagdollMovement Player;
    public PeeMeterController PeeMeter;
    public Vector3 PlayerPosition => Player.PositionMarker.position;
    private bool _gameover = false;

    private void OnEnable()
    {
        DoorTrigger.OnDoorTriggerAction += OnDoorTriggerAction;
        AreaTrigger.OnAreaTrigger += OnAreaTrigger;
        StoolToPee.OnStoolInteraction += OnStoolInteraction;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        DoorTrigger.OnDoorTriggerAction -= OnDoorTriggerAction;
        AreaTrigger.OnAreaTrigger -= OnAreaTrigger;
        StoolToPee.OnStoolInteraction -= OnStoolInteraction;
        GameEvents.OnGameOver -= OnGameOver;
    }

    private void Start()
    {
        GameEvents.SendUIMessage("I need to make pee!!!!!!", UIMessageMode.BABY_SPEAKING);
        GameEvents.LockDoor("ParentsRoom");
        GetNextGameRequirement();
        PeeMeter.Initialize();
        _gameover = false;
    }

    private void Update()
    {
        PeeMeter.Update(Time.deltaTime);
    }

    private void OnAreaTrigger(AreaTriggerActions areaAction, string id)
    {
        CheckForRequirementCompletion(areaAction.ToString(), id);
    }

    private void OnDoorTriggerAction(DoorTriggerActions doorAction, string id)
    {
        CheckForRequirementCompletion(doorAction.ToString(), id);
    }

    private void OnStoolInteraction(StoolToPeeActions stoolAction)
    {
        CheckForRequirementCompletion(stoolAction.ToString(), null);
    }

    private void CheckForRequirementCompletion(string areaAction, string id)
    {
        if (_currentGameRequirement != null && _currentGameRequirement.Evaluate(id, areaAction))
        {
            Debug.Log("Requirement completed".Color(Color.green));
            _currentGameRequirement.CompleteAction?.Invoke();
            GetNextGameRequirement();
        }
    }

    private void GetNextGameRequirement()
    {
        if (_gameRequirements.Count > 0)
        {
            _currentGameRequirement = _gameRequirements.Dequeue();
            Debug.Log("Current requirement: ".Color(Color.cyan) + _currentGameRequirement);
        }
        else
        {
            _currentGameRequirement = null;
            Debug.Log("You Won!!!!".Color(Color.green));
        }
    }

    private void OnGameOver()
    {
        _currentGameRequirement = null;
        _gameover = true;
        Debug.Log("Game Over".Color(Color.red));
    }
}
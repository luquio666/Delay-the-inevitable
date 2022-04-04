using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Queue<GameRequirement> _gameRequirements = new Queue<GameRequirement>(new List<GameRequirement>()
    {
        new GameRequirementDoor("Bathroom", DoorTriggerActions.CLOSE_DOOR, ()=>
        {
            GameEvents.SendHintsMsg("Bathroom is occupied!! No!! I need to go to the one in my room!!");
            GameEvents.UnlockDoor("Baby'sRoom");
        }),
        new GameRequirementDoor("Baby'sRoom", DoorTriggerActions.OPEN_DOOR, ()=> GameEvents.ClearHintsMsg()),
        new GameRequirementArea("Baby'sToilet", AreaTriggerActions.ENTER, () =>
        {
            GameEvents.UnlockDoor("ParentsRoom");
            GameEvents.SendHintsMsg("Oh no!! My toilet stool is not here!! I need to find it!! QUICK!!");
        }),
        new GameRequirementDoor("ParentsRoom", DoorTriggerActions.OPEN_DOOR, ()=>  GameEvents.SendHintsMsg("HERE IT IS!!!")),
        new GameRequirementStoolToPee(StoolToPeeActions.PICK_UP, ()=>  GameEvents.SendHintsMsg("YES YES YES!!!")),
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
        GameEvents.SendHintsMsg("I need to make pee!!!!!!");
        GameEvents.LockDoor("Baby'sRoom");
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
            YouWon();
        }
    }

    private void YouWon()
    {
        _currentGameRequirement = null;
        PeeMeter.Stop();
        Debug.Log("You Won!!!!".Color(Color.green));
    }

    private void OnGameOver()
    {
        _currentGameRequirement = null;
        _gameover = true;
        Debug.Log("Game Over".Color(Color.red));
    }
}
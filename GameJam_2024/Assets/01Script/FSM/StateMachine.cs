using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateMachine
{
    public EntityState CurrentState { get; private set; }

    private Dictionary<string, EntityState> _states;

    public StateMachine(List<StateSO> stateList, Entity entity)
    {
        _states = new Dictionary<string, EntityState>();
        foreach (var state in stateList)
        {
            try
            {
                Type type = Type.GetType(state.className);

                var entityState = Activator.CreateInstance(type, entity, state.stateParam)
                                        as EntityState;
                _states.Add(state.stateName, entityState);
            }
            catch (Exception e)
            {
                Debug.LogError($"{state.className} is loading error : {e.Message}");
            }
        }
    } // end of constructor

    public EntityState GetState(string stateName)
        => _states.GetValueOrDefault(stateName);

    public void InitStateMachine(string stateName)
    {
        EntityState startState = GetState(stateName);
        Debug.Assert(startState != null, $"Start state {stateName} not found");

        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeStateMachine(string newStateName)
    {
        EntityState newState = GetState(newStateName);
        Debug.Assert(newState != null, $"Start state {newState} not found");

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void UpdateFSM()
    {
        CurrentState.Update();
    }
}

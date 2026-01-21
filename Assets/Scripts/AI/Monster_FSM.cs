using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Monster_FSM
{
    [SerializeField] NavMeshAgent agent;
    public Monster_State currentState { get; private set; }
    private Dictionary<Type, Monster_State> states = new Dictionary<Type, Monster_State>();
    public void AddState(Monster_State state)
    {
        states.Add(state.GetType(), state);
    }
    public void SetState<T>() where T : Monster_State
    {
        var type = typeof(T);
        if (currentState?.GetType() == type) { return; }
        if(states.TryGetValue(type, out var newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }
    public void Update()
    {
        currentState?.Update();
    }
}

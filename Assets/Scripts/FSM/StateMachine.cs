using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    public BaseState CurrentState { get; private set; }

    public void Initialize(BaseState state)
    {
        CurrentState = state;
        state.OnEnter();

    }

    public void ChangeState (BaseState newState) {
        CurrentState.OnExit();
        CurrentState = newState;
        newState.OnEnter();
    }
}

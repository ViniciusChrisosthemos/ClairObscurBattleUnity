using System;
using UnityEngine;

public class StateMachineController
{
    public event Action<IState> OnStateChanged;

    public StateMachineController() {}

    public void Setup(IState state)
    {
        CurrentState = state;
        CurrentState.Enter();

        OnStateChanged?.Invoke(CurrentState);
    }

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();

        OnStateChanged?.Invoke(CurrentState);
    }

    public void Update()
    {
        CurrentState.UpdateState();
    }

    public IState CurrentState { get; private set; }
}

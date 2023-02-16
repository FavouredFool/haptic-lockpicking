using UnityEngine;
using System.Collections;

public abstract class StateMachine : MonoBehaviour
{
    protected State _state;

    public void SetState(State state)
    {
        _state = state;
        _state.StartState();
    }

    public void UpdateState(State state)
    {
        state.UpdateState();
    }

    public State GetState()
    {
        return _state;
    }
}
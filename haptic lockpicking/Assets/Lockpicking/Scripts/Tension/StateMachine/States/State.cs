using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected TensionManager _tensionManager;

    public State(TensionManager tensionManager)
    {
        _tensionManager = tensionManager;
    }

    public virtual void StartState()
    {

    }

    public virtual void UpdateState()
    {

    }
}

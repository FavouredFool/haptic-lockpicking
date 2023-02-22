using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected TensionForceManager _tensionManager;

    public State(TensionForceManager tensionManager)
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

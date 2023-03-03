using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TensionForceManager;

public class FinishedState : State
{
    public FinishedState(TensionForceManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered FinishedState");

        StaticTensionState = TensionState.FINISHED;
    }

    public override void UpdateState()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TensionForceManager;

public class LooseState : State
{
    public LooseState(TensionForceManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered LooseState");

        StaticTensionState = TensionState.LOOSE;
    }

    public override void UpdateState()
    {
        if (_tensionManager.GetFingerPositionX() < _tensionManager.GetLineNearerBound())
        {
            _tensionManager.SetState(new MovableState(_tensionManager));
        }
    }
}

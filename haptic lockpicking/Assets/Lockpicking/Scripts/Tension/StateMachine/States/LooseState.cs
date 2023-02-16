using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseState : State
{
    public LooseState(TensionManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered LooseState");
        Debug.Log("pins loose");
    }

    public override void UpdateState()
    {
        if (_tensionManager.GetFingerPositionX() < _tensionManager.GetLineNearerBound())
        {
            _tensionManager.SetState(new TensionState(_tensionManager));
        }
    }
}

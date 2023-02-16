using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionState : State
{
    public TensionState(TensionManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered TensionState");
        Debug.Log("pins movable");
    }

    public override void UpdateState()
    {
        _tensionManager.SetHapticActive(true);

        // Has a Driverpin just been set?
        // -> No tension, waittime

        if (_tensionManager.GetFingerPositionX() > _tensionManager.GetLineNearerBound())
        {
            _tensionManager.SetState(new LooseState(_tensionManager));
        }
        else if (_tensionManager.GetFingerPositionX() > _tensionManager.GetLineFurtherBound())
        {

        }
        else
        {
            _tensionManager.SetState(new LockState(_tensionManager));
        }
    }
}

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

        
    }

    public override void UpdateState()
    {
        StaticTensionState = TensionState.LOOSE;

        if (LockManager.Lock.GetCoreController().GetLockFinished())
        {
            _tensionManager.SetState(new FinishedState(_tensionManager));
        }

        if (_tensionManager.GetFingerPositionX() < _tensionManager.GetLineBoundsAdjusted()[0])
        {
            _tensionManager.SetState(new MovableState(_tensionManager));
        }
    }
}

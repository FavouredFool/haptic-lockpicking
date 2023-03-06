using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TensionForceManager;


public class LockState : State
{
    float _startTime = float.PositiveInfinity;

    public LockState(TensionForceManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered LockState");

        
    }

    public override void UpdateState()
    {

        StaticTensionState = TensionState.LOCKED;

        if (LockManager.Lock.GetCoreController().GetLockFinished())
        {
            _tensionManager.SetState(new FinishedState(_tensionManager));
        }

        if (_startTime < Time.time)
        {
            if (Time.time - _startTime > _tensionManager.GetLockedResetTime())
            {
                _tensionManager.SetState(new MovableState(_tensionManager));
            }

            return;
        }

        if (_tensionManager.GetFingerPositionX() > _tensionManager.GetLineBoundsAdjusted()[1])
        {
            _startTime = Time.time;
        }
        else
        {
            _tensionManager.SetHapticActive(true);
        }
    }
}

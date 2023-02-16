using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockState : State
{
    float _startTime = float.PositiveInfinity;

    public LockState(TensionManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered LockState");
        Debug.Log("pins locked");
    }

    public override void UpdateState()
    {
        if (_startTime < Time.time)
        {
            if (Time.time - _startTime > _tensionManager.GetLockedResetTime())
            {
                _tensionManager.SetState(new TensionState(_tensionManager));
            }

            return;
        }

        if (_tensionManager.GetFingerPositionX() > _tensionManager.GetLineFurtherBound() + _tensionManager.GetStateTransitionOverflowLockToTension())
        {
            _startTime = Time.time;
        }
        else
        {
            _tensionManager.SetHapticActive(true);
        }
    }
}

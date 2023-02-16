using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionState : State
{
    int _skipFrameRemainingAmount = 0;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            _skipFrameRemainingAmount = _tensionManager.GetAmountOfSkippedFrames();
        }

        // Has a Driverpin just been set?
        // -> No tension, waittime
        if (_skipFrameRemainingAmount > 0)
        {
            _skipFrameRemainingAmount -= 1;
            Debug.Log("skip");
            return;
        }


        _tensionManager.SetHapticActive(true);
        
        if (_tensionManager.GetFingerPositionX() > _tensionManager.GetLineNearerBound() + _tensionManager.GetStateTransitionOverflowTensionToLoose())
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

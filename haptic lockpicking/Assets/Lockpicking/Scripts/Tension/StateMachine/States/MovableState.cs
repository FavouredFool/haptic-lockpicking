using UnityEngine;
using static TensionForceManager;

public class MovableState : State
{
    int _skipFrameRemainingAmount = 0;

    public MovableState(TensionForceManager tensionManager) : base(tensionManager)
    {
    }

    public override void StartState()
    {
        Debug.Log("entered TensionState");

        StaticTensionState = TensionState.MOVABLE;
    }

    public override void UpdateState()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            _skipFrameRemainingAmount = _tensionManager.GetAmountOfSkippedFrames();
        }

        if (_skipFrameRemainingAmount > 0)
        {
            _skipFrameRemainingAmount -= 1;
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

    public void SkipTensionFramesAfterSet()
    {
        _skipFrameRemainingAmount = _tensionManager.GetAmountOfSkippedFrames();
    }
}

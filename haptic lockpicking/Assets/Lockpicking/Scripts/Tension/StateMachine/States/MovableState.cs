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

        
    }

    public override void UpdateState()
    {

        StaticTensionState = TensionState.MOVABLE;

        if (LockManager.Lock.GetCoreController().GetLockFinished())
        {
            _tensionManager.SetState(new FinishedState(_tensionManager));
        }

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

        float[] bounds = _tensionManager.GetLineBoundsAdjusted();

        if (_tensionManager.GetFingerPositionX() > bounds[0])
        {
            _tensionManager.SetState(new LooseState(_tensionManager));
        }
        else if (_tensionManager.GetFingerPositionX() > bounds[1])
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

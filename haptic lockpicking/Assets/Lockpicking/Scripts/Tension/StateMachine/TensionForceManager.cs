using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore;
using SGCore.Haptics;
using SG;

public class TensionForceManager : StateMachine
{
    public enum TensionState { LOOSE, MOVABLE, LOCKED };

    [Header("Dependencies")]
    [SerializeField]
    private SG_HapticGlove _tensionGlove;

    [SerializeField]
    private Transform _indexFingerLastJointTransform;

    [SerializeField]
    private Transform _indexFingerEndpointTransform;

    [SerializeField]
    private TensionVibrationManager _tensionVibrationManager;

    [SerializeField]
    PinManager _pinManager;

    [Header("TensionLine")]
    [SerializeField, Range(-1, 1)]
    private float _linePositionAlongX = 0.1f;

    [SerializeField, Range(0, 1)]
    private float _lineOffsetNearer = 0.2f;

    [SerializeField, Range(0, 1)]
    private float _lineOffsetFurtherBase = 0.2f;


    [SerializeField, Range(0, 0.1f)]
    float _lineOffsetFurtherIncrease = 0.025f;

    [Header("TensionState")]
    [SerializeField, Range(0, 100)]
    private int _amountOfSkippedFrames = 1;

    [SerializeField, Range(0, 1)]
    private float _stateTransitionOverflowTensionToLoose = 0.1f;

    [Header("LockState")]
    [SerializeField, Range(0, 1)]
    private float _lockedResetTime = 0.1f;

    [SerializeField, Range(0, 1)]
    private float _stateTransitionOverflowLockToTension = 0.1f;

    [Header("TensionWrench Visual")]
    [SerializeField]
    private Transform _tensionWrench;

    [SerializeField]
    private Transform _touchPoint;



    public static TensionState StaticTensionState = TensionState.LOOSE;

    private Vector3 _fingerPosition = Vector3.zero;

    private SG_FFBCmd _noForce;
    private SG_FFBCmd _fullForce;

    private SG_HandPose _latestPose;

    private void Start()
    {
        _noForce = new(Finger.Index, 0);
        _fullForce = new(Finger.Index, 100);
    }

    private void Update()
    {
        if (!_tensionGlove.GetHandPose(out _latestPose))
        {
            Debug.Log("Could not retrieve from " + _tensionGlove.name);
            return;
        }

        _fingerPosition = _indexFingerLastJointTransform.position + (_indexFingerEndpointTransform.position - _indexFingerLastJointTransform.position) / 2;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetState(new LooseState(this));
        }

        if (_state != null)
        {
            UpdateState(_state);
        }

        CalculateTensionWrenchVisual();
    }

    public void CalculateTensionWrenchVisual()
    {
        if (CoreController.LockFinished)
        {
            return;
        }

        Vector2 originPoint2D = new Vector2(_tensionWrench.position.x, _tensionWrench.position.y);
        Vector2 touchPoint2D = new Vector2(_touchPoint.position.x, _touchPoint.position.y);
        Vector2 fingerPoint2D = new Vector2(_fingerPosition.x, _fingerPosition.y);

        Vector2 originToTouch = touchPoint2D - originPoint2D;
        Vector2 originToFinger = fingerPoint2D - originPoint2D;

        float angle = Vector2.SignedAngle(originToTouch, originToFinger);
        if (angle < 0) angle = 0;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _tensionWrench.rotation = rotation;
    }



    public int CalculateResponseForce(float disp, int maxForce, float maxForceDist, ref AnimationCurve ffbCurve)
    {
        if (maxForceDist > 0)
        {
            float mappedDispl = disp / maxForceDist;
            
            float forceMagn = ffbCurve.Evaluate(mappedDispl);
            return (int)SGCore.Kinematics.Values.Clamp(forceMagn * maxForce, 0, maxForce);
        }
        else if (disp > 0)
        {
            return maxForce;
        }
        return 0;
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void OnDrawGizmos()
    {
        Vector3 difference = _indexFingerEndpointTransform.position - _indexFingerLastJointTransform.position;
        Vector3 center = _indexFingerLastJointTransform.position + difference / 2;

        Gizmos.DrawWireSphere(center, 1f);
    }

    public void SetHapticActive(bool active)
    {
        _tensionGlove.SendCmd(active ? _fullForce : _noForce);
    }

    private float GetLineFurtherBoundAdjusted()
    {
        float farBound = _linePositionAlongX - (_lineOffsetFurtherBase + _lineOffsetFurtherIncrease * _pinManager.GetAmountOfSetPins());

        if (StaticTensionState == TensionState.LOCKED)
        {
            farBound += _stateTransitionOverflowLockToTension;
        }

        return farBound;
    }

    private float GetLineNearerBoundAdjusted()
    {
        float nearBound = _linePositionAlongX + (_lineOffsetNearer);

        if (StaticTensionState == TensionState.MOVABLE)
        {
            nearBound += _stateTransitionOverflowTensionToLoose;
        }

        return nearBound;
    }

    public float[] GetLineBoundsAdjusted()
    {
        return new[] { GetLineNearerBoundAdjusted(), GetLineFurtherBoundAdjusted() };
    }

    public float GetFingerPositionX()
    {
        return _fingerPosition.x;
    }

    public float GetLockedResetTime()
    {
        return _lockedResetTime;
    }

    public int GetAmountOfSkippedFrames()
    {
        return _amountOfSkippedFrames;
    }

    public float GetStateTransitionOverflowTensionToLoose()
    {
        return _stateTransitionOverflowTensionToLoose;
    }

    public float GetStateTransitionOverflowLockToTension()
    {
        return _stateTransitionOverflowLockToTension;
    }

    public void PinHasBeenSet()
    {
        SkipTensionFramesAfterSet();
        _tensionVibrationManager.SkipVibrationAfterSet();
    }

    public void SkipTensionFramesAfterSet()
    {
        if (_state is not MovableState)
        {
            return;
        }

        ((MovableState)_state).SkipTensionFramesAfterSet();

    }

    public SG_HapticGlove GetTensionGlove()
    {
        return _tensionGlove;
    }

    public float GetFingerPosition01()
    {
        float[] bounds = GetLineBoundsAdjusted();
        return Mathf.Clamp01(MathLib.Remap(GetFingerPositionX(), bounds[0], bounds[1], 0, 1));
    }


}

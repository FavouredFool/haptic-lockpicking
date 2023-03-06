using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore;
using SGCore.Haptics;
using SG;

public class TensionForceManager : StateMachine
{
    public static TensionForceManager Instance { get; private set; }

    public enum TensionState { LOOSE, MOVABLE, LOCKED, FINISHED };

    [Header("Dependencies")]
    [SerializeField]
    private SG_HapticGlove _tensionGlove;

    [SerializeField]
    private Transform _indexFingerLastJointTransform;

    [SerializeField]
    private Transform _indexFingerEndpointTransform;

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
    private Transform _touchPoint;



    public static TensionState StaticTensionState = TensionState.LOOSE;

    private Vector3 _fingerPosition = Vector3.zero;

    private SG_FFBCmd _noForce;
    private SG_FFBCmd _fullForce;

    private SG_HandPose _latestPose;

    bool _hasTension = false;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.Exception();
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _noForce = new(Finger.Index, 0);
        _fullForce = new(Finger.Index, 100);
    }

    private void Update()
    {
        if (LockManager.Lock == null)
        {
            return;
        }

        if (!_hasTension)
        {
            StaticTensionState = TensionState.MOVABLE;
            return;
        }

        if (!_tensionGlove.GetHandPose(out _latestPose))
        {
            Debug.Log("Could not retrieve from " + _tensionGlove.name);
            return;
        }

        _fingerPosition = _indexFingerLastJointTransform.position + (_indexFingerEndpointTransform.position - _indexFingerLastJointTransform.position) / 2;

        if (_state != null)
        {
            UpdateState(_state);
        }

        CalculateTensionWrenchVisual();
    }

    public void CalculateTensionWrenchVisual()
    {
        if (LockManager.Lock.GetCoreController().GetLockFinished())
        {
            return;
        }

        Vector2 originPoint2D = new Vector2(LockManager.Lock.GetCoreController().GetTensionTool().position.x, LockManager.Lock.GetCoreController().GetTensionTool().position.y);
        Vector2 touchPoint2D = new Vector2(LockManager.Lock.GetCoreController().GetTouchPoint().position.x, LockManager.Lock.GetCoreController().GetTouchPoint().position.y);
        Vector2 fingerPoint2D = new Vector2(_fingerPosition.x, _fingerPosition.y);

        Vector2 originToTouch = touchPoint2D - originPoint2D;
        Vector2 originToFinger = fingerPoint2D - originPoint2D;

        float angle = Vector2.SignedAngle(originToTouch, originToFinger);
        if (angle < 0) angle = 0;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        LockManager.Lock.GetCoreController().GetTensionTool().rotation = rotation;
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
        float farBound = _linePositionAlongX - (_lineOffsetFurtherBase + _lineOffsetFurtherIncrease * PinManager.Instance.GetAmountOfSetPins());

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
        TensionVibrationManager.Instance.SkipVibrationAfterSet();
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

    public Transform GetTensionTool()
    {
        return LockManager.Lock.GetCoreController().GetTensionTool();
    }

    public void SetTensionToolActive(bool active)
    {
        LockManager.Lock.GetCoreController().GetTensionTool().gameObject.SetActive(active);
    }

    public void SetHasTension(bool hasTension)
    {
        _hasTension = hasTension;
    }

    public bool GetHasTension()
    {
        return _hasTension;
    }

}

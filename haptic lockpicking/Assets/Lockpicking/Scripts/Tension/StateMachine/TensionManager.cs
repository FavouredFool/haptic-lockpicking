using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore;
using SGCore.Haptics;
using SG;
using SGCore.Nova;

public class TensionManager : StateMachine
{
    [SerializeField]
    private SG_HapticGlove _tensionGlove;

    [SerializeField]
    private Transform _indexFingerLastJointTransform;

    [SerializeField]
    private Transform _indexFingerEndpointTransform;

    [SerializeField]
    private float _linePositionAlongX = 0.1f;

    [SerializeField]
    private float _lineOffsetBidirectional = 0.1f;

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetState(new LooseState(this));
        }

        if (_state != null)
        {
            UpdateState(_state);
        }
        

        _fingerPosition = _indexFingerLastJointTransform.position + (_indexFingerEndpointTransform.position - _indexFingerLastJointTransform.position) / 2;
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

    public float GetLineNearerBound()
    {
        return _linePositionAlongX + _lineOffsetBidirectional;
    }

    public float GetLineFurtherBound()
    {
        return _linePositionAlongX - _lineOffsetBidirectional;
    }

    public float GetFingerPositionX()
    {
        return _fingerPosition.x;
    }

    

}

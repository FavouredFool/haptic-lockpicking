using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore;
using SGCore.Haptics;
using SG;
using static TensionForceManager;

public class TensionVibrationManager : MonoBehaviour
{
    [SerializeField]
    TensionForceManager _tensionForceManager;

    [SerializeField, Range(0, 1)]
    float _setStopDuration;

    [SerializeField, Range(0, 100)]
    int _lockedIntensity;

    [SerializeField, Range(0, 100)]
    int _movableStartIntensity;

    [SerializeField, Range(0, 100)]
    int _movableEndIntensitiy;

    SG_HapticGlove _tensionGlove;

    float _setStopStart = float.NegativeInfinity;


    private void Start()
    {
        _tensionGlove = _tensionForceManager.GetTensionGlove();
    }

    private void Update()
    {
        if (Time.time - _setStopStart < _setStopDuration)
        {
            return;
        }

        CalculateAndSendVibration();
    }

    void CalculateAndSendVibration()
    {
        int intensity = StaticTensionState switch
        {
            TensionState.LOOSE => 0,
            TensionState.MOVABLE => CalculateMovableTension(),
            TensionState.LOCKED => _lockedIntensity,
            _ => 0,
        };

        SG_BuzzCmd buzzCmd = new(Finger.Index, intensity);
        SG_TimedBuzzCmd timedBuzzCmd = new(buzzCmd, Time.deltaTime);
        _tensionGlove.SendCmd(timedBuzzCmd);
    }

    private int CalculateMovableTension()
    {
        float[] bounds = _tensionForceManager.GetLineBoundsAdjusted();
        return (int) MathLib.Remap(_tensionForceManager.GetFingerPositionX(), bounds[0], bounds[1], _movableStartIntensity, _movableEndIntensitiy);
    }

    public void SkipVibrationAfterSet()
    {
        _setStopStart = Time.time;
    }


}

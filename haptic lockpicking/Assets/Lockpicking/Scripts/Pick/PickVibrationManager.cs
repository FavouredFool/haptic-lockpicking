using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore;
using SG;
using SGCore.Haptics;

public class PickVibrationManager : MonoBehaviour
{
    public static PickVibrationManager Instance { get; private set; }

    [SerializeField]
    SG_HapticGlove _pickGlove;

    [SerializeField]
    int _intensity;

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

    public void SetVibrationThisFrame(int intensity)
    {
            SG_BuzzCmd buzzCmd = new(new[] { true, true, false, false, false }, intensity);
            SG_TimedBuzzCmd timedBuzzCmd = new(buzzCmd, Time.fixedDeltaTime*4);
            _pickGlove.SendCmd(timedBuzzCmd);
    }

    public void SetInsidePinVibration()
    {
        SG_BuzzCmd buzzCmd = new(new[] { true, true, false, false, false }, 100);
        SG_TimedBuzzCmd timedBuzzCmd = new(buzzCmd, Time.fixedDeltaTime * 4, 0.1f);
        _pickGlove.SendCmd(timedBuzzCmd);

        TimedThumpCmd thumperCmd = new(40, Time.fixedDeltaTime * 4);
        _pickGlove.SendCmd(thumperCmd);
    }



}

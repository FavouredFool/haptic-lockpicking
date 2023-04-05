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
    int _springyIntensity = 15;

    [SerializeField]
    int _bindingIntensity = 40;

    [SerializeField]
    int _thumperIntensity = 20;

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
        if (TensionForceManager.StaticTensionState == TensionForceManager.TensionState.LOOSE || TensionForceManager.StaticTensionState == TensionForceManager.TensionState.FINISHED)
        {
            return;
        }

        SG_BuzzCmd buzzCmd = new(new[] { true, true, false, false, false }, intensity);
        SG_TimedBuzzCmd timedBuzzCmd = new(buzzCmd, Time.fixedDeltaTime*4);
        _pickGlove.SendCmd(timedBuzzCmd);
    }

    public void SetInsidePinVibration()
    {
        //SG_BuzzCmd buzzCmd = new(new[] { true, true, false, false, false }, 100);
        //SG_TimedBuzzCmd timedBuzzCmd = new(buzzCmd, Time.fixedDeltaTime * 4, 0.1f);
        //_pickGlove.SendCmd(timedBuzzCmd);

        if (TensionForceManager.StaticTensionState == TensionForceManager.TensionState.LOOSE || TensionForceManager.StaticTensionState == TensionForceManager.TensionState.FINISHED)
        {
            return;
        }

        TimedThumpCmd thumperCmd = new(_thumperIntensity, Time.fixedDeltaTime * 4);
        _pickGlove.SendCmd(thumperCmd);
    }

    public int GetSpringyIntensity()
    {
        return _springyIntensity;
    }

    public int GetBindingIntensity()
    {
        return _bindingIntensity;
    }


}

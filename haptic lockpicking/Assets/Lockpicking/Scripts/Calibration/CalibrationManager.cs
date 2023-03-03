using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager Instance { get; private set; }

    bool _isCalibrated = false;


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

        // Stuff to test for in the calibration manager
        // - are the hands in a proper position and rotation relative to each other?
        // - Is a very specific pose for both hands being held for a period of time with supporting visual feedback? Or has a button been pressed?

        // Calibration works like this:
        // 1. Button is pressed / gesture is made
        // 2. The screen says something like, position and hold your hands in the prefered position
        // 3. If the position is valid, a loading screen / countdown appears
        // 4. If the position is held until the loading screen finishes, the last position is seen as your new default calibration.
    }

    void Update()
    {
        if (LockManager.Lock == null)
        {
            return;
        }

        UpdateVisual();


        if (Input.GetKeyDown(KeyCode.R))
        {

            TensionForceManager.Instance.SetState(new LooseState(TensionForceManager.Instance));
            PickManager.Instance.GetPickController().Recalibrate();

            _isCalibrated = true;
        }
        
    }

    public void UpdateVisual()
    {
        TensionForceManager.Instance.GetTensionTool().gameObject.SetActive(_isCalibrated);
        PickManager.Instance.GetPickController().gameObject.SetActive(_isCalibrated);
    }

    public bool GetIsCalibrated()
    {
        return _isCalibrated;
    }

    public void SetIsCalibrated(bool isCalibrated)
    {
        _isCalibrated = isCalibrated;
        UpdateVisual();
    }
}

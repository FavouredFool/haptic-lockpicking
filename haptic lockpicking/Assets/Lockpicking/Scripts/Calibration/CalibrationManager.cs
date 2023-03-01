using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    [SerializeField]
    PickController _pickController;

    [SerializeField]
    TensionForceManager _tensionForceManager;

    bool _isInitialized = false;


    void Awake()
    {
        _pickController.gameObject.SetActive(false);
        _tensionForceManager.SetTensionWrenchActive(false);
        


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
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_isInitialized)
            {
                _pickController.gameObject.SetActive(true);
                _tensionForceManager.SetTensionWrenchActive(true);
                _tensionForceManager.SetState(new LooseState(_tensionForceManager));
            }

            _pickController.Recalibrate();
            _isInitialized = true;
        }
    }
}

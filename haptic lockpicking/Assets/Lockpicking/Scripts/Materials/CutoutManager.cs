using System.Collections.Generic;
using UnityEngine;

public class CutoutManager : MonoBehaviour
{
    public enum CutoutState { NONE, PARTIAL, FULL };

    [Header("Core")]
    [SerializeField]
    GameObject _core;

    [SerializeField]
    GameObject _coreBackHalf;

    [SerializeField]
    GameObject _coreFrontHalf;

    [Header("Hull")]
    [SerializeField]
    GameObject _hull;

    [SerializeField]
    GameObject _hullBackHalf;

    [SerializeField]
    GameObject _hullFrontHalf;

    public void Start()
    {
        SetCutoutFromState(CutoutState.FULL);
    }

    public void SetPreset1()
    {
        _core.SetActive(true);
        _coreBackHalf.SetActive(false);
        _coreFrontHalf.SetActive(false);

        _hull.SetActive(true);
        _hullBackHalf.SetActive(false);
        _hullFrontHalf.SetActive(false);
    }

    public void SetPreset2()
    {
        _core.SetActive(true);
        _coreBackHalf.SetActive(false);
        _coreFrontHalf.SetActive(false);

        _hull.SetActive(false);
        _hullBackHalf.SetActive(true);
        _hullFrontHalf.SetActive(true);
    }
    public void SetPreset3()
    {
        _core.SetActive(false);
        _coreBackHalf.SetActive(true);
        _coreFrontHalf.SetActive(true);

        _hull.SetActive(false);
        _hullBackHalf.SetActive(true);
        _hullFrontHalf.SetActive(true);
    }

    public void SetCutoutFromState(CutoutState state)
    {
        switch (state)
        {
            case CutoutState.NONE:
                SetPreset1();
                break;
            case CutoutState.PARTIAL:
                SetPreset2();
                break;
            case CutoutState.FULL:
                SetPreset3();
                break;
        }
            
    }
}

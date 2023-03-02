using System.Collections.Generic;
using UnityEngine;

public class CutoutManager : MonoBehaviour
{
    public enum CutoutState { NONE, PARTIAL, FULL };

    [SerializeField]
    LockController _lock;

    List<GameObject> _coreList;

    List<GameObject> _hullList;

    public void Start()
    {
        _coreList = _lock.GetCoreParts();
        _hullList = _lock.GetHullParts();

        SetCutoutFromState(CutoutState.FULL);
    }

    public void SetPreset1()
    {
        _coreList[0].SetActive(true);
        _coreList[2].SetActive(false);
        _coreList[1].SetActive(false);

        _hullList[0].SetActive(true);
        _hullList[2].SetActive(false);
        _hullList[1].SetActive(false);
    }

    public void SetPreset2()
    {
        _coreList[0].SetActive(true);
        _coreList[2].SetActive(false);
        _coreList[1].SetActive(false);

        _hullList[0].SetActive(false);
        _hullList[2].SetActive(true);
        _hullList[1].SetActive(true);
    }
    public void SetPreset3()
    {
        _coreList[0].SetActive(false);
        _coreList[2].SetActive(true);
        _coreList[1].SetActive(true);

        _hullList[0].SetActive(false);
        _hullList[2].SetActive(true);
        _hullList[1].SetActive(true);
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

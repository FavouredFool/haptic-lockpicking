using System.Collections.Generic;
using UnityEngine;

public class CutoutManager : MonoBehaviour
{
    public static CutoutManager Instance { get; private set; }

    public enum CutoutState { NONE, PARTIAL, FULL };

    List<GameObject> _coreList;

    List<GameObject> _hullList;

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
        if (LockManager.Lock == null)
        {
            return;
        }

        _coreList = LockManager.Lock.GetCoreParts();
        _hullList = LockManager.Lock.GetHullParts();

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

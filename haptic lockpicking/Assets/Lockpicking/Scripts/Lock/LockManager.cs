using UnityEngine;
using static CutoutManager;
using System.Collections.Generic;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance { get; private set; }

    public static LockController Lock { get; private set; }

    [SerializeField]
    LockBuilder _lockBuilder;

    private List<int> _notNulledPinOrder;

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

    public void CreateNewLock(TutorialSectionInformation _information)
    {
        if (Lock != null)
        {
            Destroy(Lock.gameObject);
        }

        Lock = _lockBuilder.BuildLock();

        _notNulledPinOrder = _information.PinOrder ?? PinManager.Instance.GetRandomPinOrder(_information.PinCount);

        _lockBuilder.SetParameters(_information.PinCount, _notNulledPinOrder, _information.RespectOrder, _information.HasPick, _information.HasTension, _information.ColorCodePins, _information.ShowTensionIndicator, _information.ShowPinPositionIndicator, _information.CutoutState, _information.EnableCustomization, _information.KeyAnimation);
    }
}

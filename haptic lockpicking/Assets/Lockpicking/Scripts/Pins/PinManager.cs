using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PinController;

public class PinManager : MonoBehaviour
{
    public static PinManager Instance { get; private set; }

    [SerializeField, Range(0, 1)]
    private float _maxVelocityForSet = 0.25f;

    [SerializeField, Range(0, 1)]
    private float _setThreshold = 0.25f;

    List<int> _pinOrder = new() { 0, 1, 2, 3, 4 };
    private static readonly System.Random random = new();

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

    public void Update()
    {
        if (LockManager.Lock == null)
        {
            return;
        }

        UpdatePinLogic();
    }

    public bool UpdatePinLogic()
    {
        bool allSet = true;

        for (int i = 0; i < _pinOrder.Count; i++)
        {
            PinController activePins = LockManager.Lock.GetPinControllers()[_pinOrder[i]];

            if (allSet)
            {
                activePins.SetNonSetState(PinState.BINDING);

                if (activePins.GetPinState() != PinState.SET)
                {
                    allSet = false;
                }
            }
            else
            {
                activePins.SetNonSetState(PinState.SPRINGY);
            }
        }

        return allSet;
    }

    public List<PinController> GetPinControllers()
    {
        if (LockManager.Lock == null)
        {
            return new() { };
        }

        return LockManager.Lock.GetPinControllers();
    }

    public int GetAmountOfSetPins()
    {
        for (int i = 0; i < _pinOrder.Count; i++)
        {
            PinController activePins = LockManager.Lock.GetPinControllers()[_pinOrder[i]];

            if (activePins.GetPinState() != PinState.SET)
            {
                return i;
            }
            
        }

        return 5;
    }

    public float GetMaxVelocityForSet()
    {
        return _maxVelocityForSet;
    }


    public float GetSetThreshold()
    {
        return _setThreshold;
    }

    public void SetPinOrder(List<int> pinOrder)
    {
        _pinOrder = pinOrder;
    }

    public List<int> GetRandomPinOrder(int size)
    {
        List<int> list = new() { 0, 1, 2, 3, 4 };

        return list.Take(size).OrderBy(a => random.Next()).ToList();
    }
}

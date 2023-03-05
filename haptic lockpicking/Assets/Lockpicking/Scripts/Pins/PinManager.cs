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

    List<int> _pinOrder;
    private static readonly System.Random random = new();

    private int _pinAmount;

    private bool _respectOrder;

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

        for (int i = 0; i < _pinAmount; i++)
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
        for (int i = 0; i < _pinAmount; i++)
        {
            PinController activePins = LockManager.Lock.GetPinControllers()[_pinOrder[i]];

            if (activePins.GetPinState() != PinState.SET)
            {
                return i;
            }
            
        }

        return _pinAmount;
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
        list = list.Take(size).OrderBy(a => random.Next()).ToList();
        return list;
    }

    public void SetPinAmount(int pinAmount)
    {
        _pinAmount = pinAmount;

        int i = 0;

        foreach (PinController pin in LockManager.Lock.GetPinControllers())
        {
            pin.gameObject.SetActive(i < pinAmount);
            i++;
        }
    }

    public int GetPinAmount()
    {
        return _pinAmount;
    }

    public void SetRespectOrder(bool respectOrder)
    {
        _respectOrder = respectOrder;
    }

    public bool GetRespectOrder()
    {
        return _respectOrder;
    }
}

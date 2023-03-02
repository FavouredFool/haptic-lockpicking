using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PinController;

public class PinManager : MonoBehaviour
{
    [SerializeField]
    LockController _lockController;

    [SerializeField, Range(0, 1)]
    private float _maxVelocityForSet = 0.25f;

    [SerializeField, Range(0, 1)]
    private float _setThreshold = 0.25f;

    List<int> _pinOrder = new() { 0, 1, 2, 3, 4 };
    private static readonly System.Random random = new();

    public void Start()
    {
        RandomizePins();
    }

    public void Update()
    {
        UpdatePinLogic();
    }

    public bool UpdatePinLogic()
    {
        bool allSet = true;

        for (int i = 0; i < _pinOrder.Count; i++)
        {
            PinController activePins = _lockController.GetPinControllers()[_pinOrder[i]];

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
        return _lockController.GetPinControllers();
    }

    public int GetAmountOfSetPins()
    {
        for (int i = 0; i < _pinOrder.Count; i++)
        {
            PinController activePins = _lockController.GetPinControllers()[_pinOrder[i]];

            if (activePins.GetPinState() != PinState.SET)
            {
                return i;
            }
            
        }

        return 5;
    }

    public void RandomizePins()
    {
        _pinOrder = _pinOrder.OrderBy(a => random.Next()).ToList();
        Debug.Log("Order: " + _pinOrder[0] + ", " + _pinOrder[1] + ", " + _pinOrder[2] + ", " + _pinOrder[3] + ", " + _pinOrder[4]);
    }

    public float GetMaxVelocityForSet()
    {
        return _maxVelocityForSet;
    }


    public float GetSetThreshold()
    {
        return _setThreshold;
    }
}

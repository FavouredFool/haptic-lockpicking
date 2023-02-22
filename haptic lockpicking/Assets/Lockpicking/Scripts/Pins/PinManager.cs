using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static PinController;

public class PinManager : MonoBehaviour
{
    [SerializeField]
    List<PinController> _pinControllers;

    List<int> _pinOrder = new() { 0, 1, 2, 3, 4 };
    private static readonly System.Random random = new();

    public void Start()
    {
        _pinOrder = _pinOrder.OrderBy(a => random.Next()).ToList();
        Debug.Log(_pinOrder[0]);
    }

    public bool UpdatePinLogic()
    {
        bool allSet = true;

        for (int i = 0; i < _pinOrder.Count; i++)
        {
            PinController activePins = _pinControllers[_pinOrder[i]];

            if (allSet)
            {
                activePins.SetNonSetState(SetState.BINDING);

                if (activePins.GetSetState() != SetState.SET)
                {
                    allSet = false;
                }
            }
            else
            {
                activePins.SetNonSetState(SetState.SPRINGY);
            }
        }

        return allSet;
    }

    public List<PinController> GetPinControllers()
    {
        return _pinControllers;
    }

}

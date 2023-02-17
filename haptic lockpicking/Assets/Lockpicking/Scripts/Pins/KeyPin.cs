using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyPin : Pin
{
    public bool IsBelowSheer()
    {
        return transform.position.y < PinController.SHEERLINE_HEIGHT;
    }
}

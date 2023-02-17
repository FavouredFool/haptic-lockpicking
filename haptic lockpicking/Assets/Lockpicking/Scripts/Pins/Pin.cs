using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pin : MonoBehaviour
{
    public abstract void AnyStateUpdate(PinController pinController);

    public abstract void LooseUpdate(PinController pinController);

    public abstract void MovableUpdate(PinController pinController);

    public abstract void LockedUpdate(PinController pinController);


}

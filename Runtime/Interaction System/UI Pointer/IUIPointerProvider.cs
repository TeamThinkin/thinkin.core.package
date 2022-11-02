using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPointerProvider
{
    Ray GetRay();
    event System.Action PrimaryButtonStart;
    event System.Action PrimaryButtonEnd;
    event System.Action SecondaryButtonStart;
    event System.Action SecondaryButtonEnd;
}
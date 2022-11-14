using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIManager 
{
    event System.Action<GameObject> OnMakeGrabbable;

    void MakeGrabbable(GameObject Item);
}

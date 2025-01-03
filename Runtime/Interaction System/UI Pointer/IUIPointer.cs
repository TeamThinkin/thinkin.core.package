using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPointer
{
    Transform transform { get; }
    Ray Ray { get; }
    void SetProvider(IUIPointerProvider Provider);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabber
{
    GameObject gameObject { get; }
    Transform transform { get; }
}

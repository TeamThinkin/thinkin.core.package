using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public class UnityHandGrabEvent : UnityEvent<IGrabber, IGrabbable> { }

public interface IGrabbable
{  
    delegate void GrabEventDelegate(IGrabber Grabber, IGrabbable Grabbable);

    event GrabEventDelegate OnBeforeGrab;
    event GrabEventDelegate OnGrab;
    event GrabEventDelegate OnRelease;

    GameObject gameObject { get; }
    Transform transform { get; }
}

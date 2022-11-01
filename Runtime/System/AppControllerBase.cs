using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AppControllerBase : MonoBehaviour
{
    public static AppControllerBase Instance { get; private set; }

    public TabletOptions TabletSettings;

    public abstract Transform PlayerTransform { get; }
    public abstract Rigidbody PlayerBody { get; }
    public abstract Camera MainCamera { get; }
    public abstract string BundleVersionCode { get; }
    public abstract IUIManager UIManager { get; }

    public abstract void SetPlayerPosition(Vector3 WorldPosition);
    public abstract void SetPlayerPosition(Vector3 WorldPosition, Quaternion WorldRotation);
    public abstract void SetPlayerRotation(Quaternion WorldRotation);
    

    public virtual void Awake()
    {
        Instance = this;
    }
}

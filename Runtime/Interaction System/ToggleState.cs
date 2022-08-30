using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleState : MonoBehaviour
{
    [SerializeField] private Material OnMaterial;
    [SerializeField] private Material OffMaterial;
    [SerializeField] private Renderer Renderer;

    public bool CurrentState { get; private set; }

    public void SetState(bool IsOn)
    {
        CurrentState = IsOn;
        Renderer.sharedMaterial = IsOn ? OnMaterial : OffMaterial;
    }
}

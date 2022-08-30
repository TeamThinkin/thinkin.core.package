using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumVector3
{
    public Vector3 Value;
    public float DecayRate;
    public float SmoothingRate;

    private Vector3 smoothValue;

    public MomentumVector3(float DecayRate = 0.5f, float SmoothingRate = 0.1f)
    {
        this.Value = Vector3.zero;
        this.smoothValue = this.Value;
        this.DecayRate = DecayRate;
        this.SmoothingRate = SmoothingRate;
    }

    public void Set(Vector3 Value)
    {
        //this.Value = Value;
        smoothValue = Vector3.Lerp(Value, smoothValue, SmoothingRate);
        this.Value = smoothValue;
    }

    public void Update()
    {
        smoothValue = Value = Vector3.Lerp(smoothValue, Vector3.zero, DecayRate);
    }
}

public class MomentumFloat
{
    public float Value;
    public float DecayRate;
    public float SmoothingRate;

    private float smoothValue;

    public MomentumFloat(float DecayRate = 0.5f, float SmoothingRate = 0.1f)
    {
        this.Value = 0;
        this.smoothValue = this.Value;
        this.DecayRate = DecayRate;
        this.SmoothingRate = SmoothingRate;
    }

    public void Set(float Value)
    {
        //this.Value = Value;
        smoothValue = Mathf.Lerp(Value, smoothValue, SmoothingRate);
        this.Value = smoothValue;
    }

    public void Update()
    {
        smoothValue = Value = Mathf.Lerp(smoothValue, 0, DecayRate);
    }
}
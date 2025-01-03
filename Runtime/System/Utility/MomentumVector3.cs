﻿using System.Collections;
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
    public float HighDecayThreshold;
    public float HighDecayRate;

    public MomentumFloat(float DecayRate = 0.5f, float SmoothingRate = 0.1f, float HighDecayThresholdValue = 0.01f, float HighDecayRate = 0.2f)
    {
        this.Value = 0;
        this.DecayRate = DecayRate;
        this.SmoothingRate = SmoothingRate;
        this.HighDecayThreshold = HighDecayThresholdValue;
        this.HighDecayRate = HighDecayRate;
    }

    public void Set(float NewValue)
    {
        Value = Mathf.Lerp(NewValue, Value, SmoothingRate);
    }

    public void Update()
    {
        Value = Mathf.Lerp(Value, 0, Value > HighDecayThreshold ? DecayRate : HighDecayRate);
    }
}
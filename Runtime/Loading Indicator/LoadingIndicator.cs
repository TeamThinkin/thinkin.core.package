using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{
    [SerializeField] private Transform[] Actors;

    public float amplitude = 1;
    public float frequency = 1;
    public float width = 1;
    private float phase;

    private void Start()
    {
        phase = Random.Range(0, Mathf.PI);
    }

    void Update()
    {
        float v;

        for(int i = 0; i < Actors.Length; i++)
        {
            var actor = Actors[i];
            v = Mathf.Sin(Time.time * frequency + (actor.localPosition.x * width) + phase) * amplitude;
            actor.localPosition = actor.localPosition.SetY(v);
        }
    }
}

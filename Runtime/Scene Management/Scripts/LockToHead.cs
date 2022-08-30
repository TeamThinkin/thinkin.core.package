using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToHead : MonoBehaviour
{
    private Transform head;

    void Start()
    {
        head = Camera.main.transform;
    }

    void Update()
    {
        transform.position = head.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] private Transform SourceTransform;
    
    void Update()
    {
        copy();
    }

    private void LateUpdate()
    {
        copy();
    }

    private void copy()
    {
        transform.position = SourceTransform.position;
        transform.rotation = SourceTransform.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingLine : MonoBehaviour
{
    public Transform TargetItem;

    [SerializeField] private LineRenderer lineRenderer;

    private Vector3[] points = new Vector3[2];
    private float baseWidth;

    private void Start()
    {
        baseWidth = lineRenderer.widthMultiplier;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        points[0] = transform.position;
        points[1] = Vector3.Lerp(transform.position, TargetItem.position, 0.995f);
        lineRenderer.SetPositions(points);
    }

    public void SetSize(float Size)
    {
        lineRenderer.widthMultiplier = Size * baseWidth;
    }
}

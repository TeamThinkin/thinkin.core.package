using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtentions
{
    /// <summary>
    /// Get a bounding box that encapsulates the mesh of object along with any of its children in the hierarchy
    /// </summary>
    public static Bounds GetHierachyBounds(this GameObject rootObject)
    {
        Bounds bounds = new Bounds(rootObject.transform.position, Vector3.zero);
        bool isFirst = true;
        foreach (Renderer renderer in rootObject.GetComponentsInChildren<Renderer>())
        {
            if (isFirst)
            {
                bounds = renderer.bounds;
                isFirst = false;
            }
            else bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

    public static Tuple<Vector3, Vector3> GetMinMax(this IEnumerable<Vector3> points)
    {
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;
        bool isFirstItem = true;
        foreach (var point in points)
        {
            if (isFirstItem)
            {
                min = max = point;
            }
            else
            {
                if (point.x < min.x) min.x = point.x;
                if (point.x > max.x) max.x = point.x;
                if (point.y < min.y) min.y = point.y;
                if (point.y > max.y) max.y = point.y;
                if (point.z < min.z) min.z = point.z;
                if (point.z > max.z) max.z = point.z;
            }
            isFirstItem = false;
        }
        return new Tuple<Vector3, Vector3>(min, max);
    }
}

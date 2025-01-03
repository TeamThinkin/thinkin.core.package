﻿using AngleSharp.Dom;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.ResourceManagement.AsyncOperations;

public static class Extensions
{
    public static Color Solid(this Color color)
    {
        color.a = 1;
        return color;
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static Task GetTask(this AsyncOperation asyncOperation)
    {
        var tcs = new TaskCompletionSource<object>();

        asyncOperation.completed += (AsyncOperation e) =>
        {
            tcs.SetResult(null);
        };

        return tcs.Task;
    }

    //public static Task<T> GetTask<T>(this AsyncOperationHandle<T> asyncOpHandle)
    //{
    //    var tcs = new TaskCompletionSource<T>();

    //    asyncOpHandle.Completed += (AsyncOperationHandle<T> e) =>
    //    {
    //        tcs.SetResult(e.Result);
    //    };

    //    return tcs.Task;
    //}


    /// <summary>
    /// Resets the transforms local position, rotation and scale properties to default values
    /// </summary>
    /// <param name="transform"></param>
    public static void Reset(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Remaps the value from one number range to another
    /// </summary>
    public static float Remap(this float value, float originalMin, float originalMax, float targetMin, float targetMax, bool isClamped = false)
    {
        var newValue = (value - originalMin) / (originalMax - originalMin) * (targetMax - targetMin) + targetMin;
        if (isClamped) newValue = Mathf.Clamp(newValue, Mathf.Min(targetMax, targetMin), Mathf.Max(targetMax, targetMin));
        return newValue;
    }

    public static IEnumerable<Transform> GetChildren(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            yield return child;
        }
    }

    public static void ClearChildren(this Transform parent)
    {
        if (parent == null) return;
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void ClearChildrenImmediate(this Transform parent)
    {
        if (parent == null) return;
        int count = parent.childCount;
        for(int i=0;i<count;i++)
        {
            GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }

    public static void SetActive(this IEnumerable<GameObject> items, bool value)
    {
        foreach (var item in items) item.SetActive(value);
    }

    public static Vector3 FlattenX(this Vector3 vector)
    {
        vector.x = 0;
        return vector;
    }

    public static Vector3 FlattenY(this Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }

    public static Vector3 FlattenZ(this Vector3 vector)
    {
        vector.z = 0;
        return vector;
    }

    public static Vector3 IsolateX(this Vector3 vector)
    {
        vector.y = 0;
        vector.z = 0;
        return vector;
    }

    public static Vector3 IsolateY(this Vector3 vector)
    {
        vector.x = 0;
        vector.z = 0;
        return vector;
    }

    public static Vector3 IsolateZ(this Vector3 vector)
    {
        vector.x = 0;
        vector.y = 0;
        return vector;
    }

    public static Vector3 SetX(this Vector3 vector, float value)
    {
        vector.x = value;
        return vector;
    }

    public static Vector3 SetY(this Vector3 vector, float value)
    {
        vector.y = value;
        return vector;
    }

    public static Vector3 SetZ(this Vector3 vector, float value)
    {
        vector.z = value;
        return vector;
    }

    public static Vector3 Scale(this Vector3 vector, float x, float y, float z)
    {
        vector.x *= x;
        vector.y *= y;
        vector.z *= z;
        return vector;
    }

    public static Vector3 Divide(this Vector3 left, Vector3 right)
    {
        left.x /= right.x;
        left.y /= right.y;
        left.z /= right.z;
        return left;
    }

    public static void Draw(this Ray ray, Color color)
    {
        Debug.DrawRay(ray.origin, ray.direction, color);
    }
    public static void Print(this Vector3 vector, string prefix = null)
    {
        Debug.Log(prefix + vector.x.ToString("0.0000") + ", " + vector.y.ToString("0.0000") + ", " + vector.z.ToString("0.0000"));
    }

    public static Quaternion Straighten(this Quaternion rot)
    {
        return Quaternion.Euler(0, rot.eulerAngles.y, 0);
    }

    public static Vector3 Absolute(this Vector3 vector)
    {
        vector.x = Mathf.Abs(vector.x);
        vector.y = Mathf.Abs(vector.y);
        vector.z = Mathf.Abs(vector.z);
        return vector;
    }

    public static string ToJSON(this object o)
    {
        return JsonConvert.SerializeObject(o);
    }

    public static Vector3 GetRaycastPoint(this Plane plane, Ray ray)
    {
        plane.Raycast(ray, out float enter);
        return ray.origin + ray.direction * enter;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> list)
    {
        return list.Where(i => i != null);
    }

    public static IEnumerable<TResult> SelectNotNull<T, TResult>(this IEnumerable<T> list, Func<T, TResult> selector)
    {
        return list.Select(selector).WhereNotNull();
    }

    public static string StripQuotes(this string s)
    {
        return s?.Substring(1, s.Length - 2);
    }

    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static bool InvolvesPrimaryFingerTip(this Collision collision)
    {
        return collision.contacts.Any(i => i.otherCollider.gameObject.tag == "PointerFingerTip" || i.thisCollider.gameObject.tag == "PointerFingerTip");
    }

    //public static string GetPath(this GameObject item)
    //{
    //    return string.Join("/", item.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());
    //}

    //public static string GetPath(this GameObject item)
    //{
    //    string path = "/" + item.name;
    //    while (item.transform.parent != null)
    //    {
    //        item = item.transform.parent.gameObject;
    //        path = "/" + item.name + path;
    //    }
    //    return path;  
    //}

    public static string GetPath(this GameObject Item)
    {
        return string.Join("/", Item.Ancestors().Select(i => i.name).Reverse().ToArray());
    }

    public static IEnumerable<GameObject> Ancestors(this GameObject Item)
    {
        yield return Item;
        while(Item.transform.parent != null)
        {
            Item = Item.transform.parent.gameObject;
            yield return Item;
        }
    }

    public static string TransformRelativeUrlToAbsolute(this string RelativeUrl, IElement ElementData)
    {
        return TransformRelativeUrlToAbsolute(RelativeUrl, ElementData.BaseUri);
    }

    public static string TransformRelativeUrlToAbsolute(this string RelativeUrl, string BaseUrl)
    {
        return new Uri(new Uri(BaseUrl), RelativeUrl).AbsoluteUri;
    }

    public static T? ToEnum<T>(this string text) where T : struct
    {
        if (text == null) return null;

        text = text?.Replace("-", "");

        if (Enum.TryParse<T>(text, true, out T value))
            return value;
        else
            return null;
    }

    public static float? ToFloat(this string text)
    {
        if (text == null) return null;

        if (float.TryParse(text.Trim(), out float value))
            return value;
        else
            return null;

    }

    public static Vector3? ToVector3(this string text)
    {
        // Assumed format: {0.1,0.2,0.3}
        if (text == null) return null;

        try
        {
            text = text.Trim();

            if (text.Length <= 2 || text[0] != '{')
            {
                Debug.LogError("Invalid vector string (Expecting {0.1,0.2,0.3}): " + text);
                return null;
            }

            text = text.Substring(1, text.Length - 2);
            var pieces = text.Split(',');

            if (pieces.Length < 3)
            {
                Debug.LogError("Invalid vector string (Expecting {0.1,0.2,0.3}): " + text);
                return null;
            }

            var x = float.Parse(pieces[0].Trim());
            var y = float.Parse(pieces[1].Trim());
            var z = float.Parse(pieces[2].Trim());
            return new Vector3(x, y, z);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Invalid vector string (Expecting {0.1,0.2,0.3}): " + text);
            return null;
        }
    }

    public static Quaternion? ToQuaternion(this string text)
    {
        // Assumed format: {0.1,0.2,0.3} OR {0.1,0.2,0.3,0.4}
        if (text == null) return null;

        try
        {
            text = text.Trim();

            if (text.Length <= 3 || text[0] != '{')
            {
                Debug.LogError("Invalid vector string (Expecting {0.1,0.2,0.3,0.4}): " + text);
                return null;
            }

            text = text.Substring(1, text.Length - 2);
            var pieces = text.Split(',');

            if (pieces.Length < 3)
            {
                Debug.LogError("Invalid vector string (Expecting {0.1,0.2,0.3} or {0.1,0.2,0.3,0.4}): " + text);
                return null;
            }

            if(pieces.Length == 4)
            {
                return new Quaternion(
                    float.Parse(pieces[0].Trim()),
                    float.Parse(pieces[1].Trim()),
                    float.Parse(pieces[2].Trim()),
                    float.Parse(pieces[3].Trim())
                );
            }
            else
            {
                return Quaternion.Euler(
                    float.Parse(pieces[0].Trim()),
                    float.Parse(pieces[1].Trim()),
                    float.Parse(pieces[2].Trim())
                );
            }

            
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Invalid quaternion string (Expecting {0.1,0.2,0.3,0.4}): " + text);
            return null;
        }
    }

    public static string Capitalize(this string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        return char.ToUpper(text[0]) + text.Substring(1);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleMonitor : MonoBehaviour
{
    private const float abandonTimeSeconds = 2 * 60; //2 Minutes
    private const float minIdleTimeSeconds = 3; //If you change this, you will need to change threshold
    private const float threshold = 0.000001f;

    private Transform playerTransform;
    private static Vector3 lastMousePosition;
    private static Vector3 lastPlayerPosition;
    private static Vector3 lastPlayerForward;
    private static float timeOfLastMovement;

    public static event Action OnIdleEnd;
    public static event Action OnIdleStart;
    public static event Action OnAbandonStart;
    public static event Action OnAbandonEnd;
    public static bool IsIdle { get; private set; }
    public static bool IsAbandoned { get; private set; }

    public static float IdleDurationSeconds
    {
        get
        {
            if (!IsIdle) return -1;
            return Time.time - timeOfLastMovement;
        }
    }

    private void Start()
    {
        playerTransform = AppControllerBase.Instance.MainCamera.transform;
        InvokeRepeating("monitorIdleState", 0, 1);
    }

    private void monitorIdleState()
    {
        var isPositionChanged = (playerTransform.position - lastPlayerPosition).sqrMagnitude > threshold;
        var isForwardChanged = (playerTransform.forward - lastPlayerForward).sqrMagnitude > threshold;
        var isMouseChanged = (Input.mousePosition - lastMousePosition).sqrMagnitude > threshold;

        if (isPositionChanged || isForwardChanged || isMouseChanged)
        {
            //The player has moved 
            timeOfLastMovement = Time.time;

            if (IsIdle)
            {
                IsIdle = false;
                OnIdleEnd?.Invoke();
            }

            if(IsAbandoned)
            {
                IsAbandoned = false;
                OnAbandonEnd?.Invoke();
            }
        }
        else
        {
            //The player has not moved
            if (!IsIdle && Time.time - timeOfLastMovement >= minIdleTimeSeconds)
            {
                IsIdle = true;
                OnIdleStart?.Invoke();
            }

            if(!IsAbandoned && IdleDurationSeconds >= abandonTimeSeconds)
            {
                IsAbandoned = true;
                OnAbandonStart?.Invoke();
            }
        }

        lastMousePosition = Input.mousePosition;
        lastPlayerPosition = playerTransform.position;
        lastPlayerForward = playerTransform.forward;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSpawnPoint : MonoBehaviour
{
    private void Start()
    {
        AppSceneManager.OnEnvironmentLoaded += AppSceneManager_OnEnvironmentLoaded;
    }

    private void OnDestroy()
    {
        AppSceneManager.OnEnvironmentLoaded -= AppSceneManager_OnEnvironmentLoaded;
    }

    private void AppSceneManager_OnEnvironmentLoaded()
    {
        moveToSpawn();
    }

    private void moveToSpawn()
    {
        Vector3 targetPoint;
        Quaternion targetRot;

        var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            targetPoint = Vector3.zero;
            targetRot = Quaternion.identity;
        }
        else
        {
            var spawnPoint = spawnPoints.RandomItem();
            targetPoint = spawnPoint.transform.position;
            targetRot = spawnPoint.transform.rotation;
        }

        AppControllerBase.Instance.PlayerBody.velocity = Vector3.zero;
        AppControllerBase.Instance.PlayerBody.angularVelocity = Vector3.zero;
        AppControllerBase.Instance.SetPlayerPosition(targetPoint, targetRot);
    }
}

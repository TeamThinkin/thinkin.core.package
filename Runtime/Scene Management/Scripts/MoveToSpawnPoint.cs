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

            //Debug.DrawRay(targetPoint, Vector3.up * 10, Color.yellow, 60);
            //Debug.DrawRay(targetPoint, spawnPoint.transform.forward * 10, Color.yellow, 60);
        }



        //transform.rotation = targetRot;

        //var playerOffset = (transform.position - Camera.main.transform.position).FlattenY();
        //transform.position = targetPoint + playerOffset;

        //TrackerOffsets.localPosition = Vector3.zero;
        //TrackerOffsets.localRotation = Quaternion.identity;
        //Player.SetPosition(transform.position, transform.rotation);

        AppControllerBase.Instance.PlayerBody.velocity = Vector3.zero;
        AppControllerBase.Instance.PlayerBody.angularVelocity = Vector3.zero;
        AppControllerBase.Instance.SetPlayerPosition(targetPoint, targetRot);
    }
}

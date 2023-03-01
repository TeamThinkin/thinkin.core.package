using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomListPresenter : MonoBehaviour
{
    [SerializeField] GameObject PortalPrefab;
    [SerializeField] Transform PortalContainer;
    [SerializeField] Transform[] SpawnPoints;

    //private void Start()
    //{
    //    RoomManager.OnRoomLoaded += RoomManager_OnRoomLoaded;
    //}

    //private void OnDestroy()
    //{
    //    RoomManager.OnRoomLoaded -= RoomManager_OnRoomLoaded;
    //}

    //private void OnEnable()
    //{
    //    RoomManager_OnRoomLoaded();
    //}


    //private void RoomManager_OnRoomLoaded()
    //{
    //    var roomLinks = RoomManager.ContentItems.Where(i => i.DtoTypes.Contains(typeof(RoomLinkContentItemDto)));

    //    int i = 0;
    //    foreach(var roomLink in roomLinks)
    //    {
    //        roomLink.GameObject.transform.position = SpawnPoints[i].position;
    //        roomLink.GameObject.transform.rotation = SpawnPoints[i].rotation;
    //        i++;
    //    }
    //}
}

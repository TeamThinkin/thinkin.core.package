using System.Collections;
using UnityEngine;


public static class ItemSpawnObserver
{
    public static event System.Action<GameObject> OnItemSpawned;

    public static void NotifyItemSpawned(GameObject SpawnedItem)
    {
        OnItemSpawned?.Invoke(SpawnedItem);
    }
}

public interface ISpawnableItem
{
    string PrefabPath { get; }
    GameObject gameObject { get; }
    Transform transform { get; }
}
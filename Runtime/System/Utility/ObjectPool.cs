using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TypedObjectPool<T> : ObjectPool<T> where T : MonoBehaviour
{
    private GameObject itemPrefab;

    public TypedObjectPool(GameObject ItemPrefab, Action<T> OnGetAction = null, Action<T> OnReleaseAction = null)
    {
        this.itemPrefab = ItemPrefab;
        this.onGetAction = OnGetAction;
        this.onReleaseAction = OnReleaseAction;
    }

    protected override T CreatedNewItem()
    {
        return GameObject.Instantiate(itemPrefab).GetComponent<T>();
    }

    protected override void PrepareItemForGet(T Item)
    {
        base.PrepareItemForGet(Item);
        Item.gameObject.SetActive(true);
    }

    protected override void PrepareItemForRelease(T Item)
    {
        base.PrepareItemForRelease(Item);
        if(Item.gameObject != null) Item.gameObject.SetActive(false);
    }
}

public class ObjectPool<T> where T : class
{
    private class PoolItem<ItemT>
    {
        public ItemT Item;
        public bool IsActive;
    }

    protected Func<T> createNewFunction;
    protected Action<T> onGetAction;
    protected Action<T> onReleaseAction;

    private List<PoolItem<T>> pool = new List<PoolItem<T>>();

    public ObjectPool() { }
    public ObjectPool(Func<T> CreateNewFunction, Action<T> OnGetAction, Action<T> OnReleaseAction)
    {
        this.createNewFunction = CreateNewFunction;
        this.onGetAction = OnGetAction;
        this.onReleaseAction = OnReleaseAction;
    }

    protected virtual T CreatedNewItem()
    {
        var item = createNewFunction();
        return item;
    }

    protected virtual void PrepareItemForGet(T Item)
    {
        onGetAction?.Invoke(Item);
    }

    protected virtual void PrepareItemForRelease(T Item)
    {
        onReleaseAction?.Invoke(Item);
    }

    public void Clear()
    {
        foreach(var item in pool)
        {
            if (item.IsActive)
            {
                PrepareItemForRelease(item.Item);
                item.IsActive = false;
            }
        }
    }

    public IEnumerable<T> ActiveItems
    {
        get
        {
            return pool.Where(i => i.IsActive).Select(i => i.Item);
        }
    }

    public T Get()
    {
        T item = null;

        foreach (var poolItem in pool)
        {
            if (!poolItem.IsActive)
            {
                poolItem.IsActive = true;
                item = poolItem.Item;
                break;
            }
        }

        if (item == null)
        {
            var poolItem = new PoolItem<T>();
            poolItem.IsActive = true;
            poolItem.Item = CreatedNewItem();
            pool.Add(poolItem);
            item = poolItem.Item;
        }

        PrepareItemForGet(item);

        return item;
    }

    public void Release(T Item)
    {
        foreach (var poolItem in pool)
        {
            if (poolItem.Item == Item)
            {
                PrepareItemForRelease(Item);
                poolItem.IsActive = false;
                break;
            }
        }
    }

    public void DumpDebugInfo()
    {
        Debug.Log("Active pool items: " + pool.Where(i => i.IsActive).Count());
        Debug.Log("Inactive pool items: " + pool.Where(i => !i.IsActive).Count());
    }
}
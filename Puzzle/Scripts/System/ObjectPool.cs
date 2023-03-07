using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem
{
    public GameObject BaseItem;
    public Queue<GameObject> ItemQueue = new Queue<GameObject>();
    public List<GameObject> ItemList = new List<GameObject>();

    public GameObject GetItem()
    {
        GameObject item = ItemQueue.Dequeue();
        ItemList.Add(item);
        return item;
    }

    public void DeleteItem(GameObject obj)
    {
        ItemList.Remove(obj);
        ItemQueue.Enqueue(obj);
        obj.SetActive(false);
    }
}

public class ObjectPool : Singleton<ObjectPool> {

    private Dictionary<string, PoolItem> PoolHash = new Dictionary<string, PoolItem>();

	public void CreateObject(string name, GameObject obj, int count)
    {
        PoolItem pool;

        if (!PoolHash.ContainsKey(name))
        {
            pool = new PoolItem();
            PoolHash.Add(name, pool);
        }
        else
            pool = PoolHash[name];

        pool.BaseItem = obj;

        for (int i = pool.ItemQueue.Count; i < count; i++)
        {
            GameObject item = Instantiate(obj, transform);

            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.SetActive(false);

            pool.ItemQueue.Enqueue(item);
        }

    }

    public GameObject GetPoolItem(string name)
    {
        if(PoolHash.ContainsKey(name))
        {
            PoolItem pool = PoolHash[name];

            if(pool.ItemQueue.Count == 0)
            {
                CreateObject(name, pool.BaseItem, 1);
            }

            return pool.GetItem();
        }
        return null;
    }

    public void DeleteItem(string name, GameObject obj)
    {
        if (PoolHash.ContainsKey(name))
        {
            PoolItem pool = PoolHash[name];
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            pool.DeleteItem(obj);
        }
    }
}

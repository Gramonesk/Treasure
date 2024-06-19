using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
<<<<<<< Updated upstream

=======
using UnityEngine.Pool;
using Fusion;
>>>>>>> Stashed changes

public class DropPool : NetworkBehaviour
{
    /*[System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int limit;
    }*/

    [SerializeField] private Item prefab;
    [SerializeField] private Transform placeHolder;

    #region Instance
    public static DropPool Instance;

    public void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #endregion

<<<<<<< Updated upstream
    public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDick;
=======
    /*public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDick;*/
    public ObjectPool<Item> ItemPool;

>>>>>>> Stashed changes

    public void Start()
    {
        /*PoolDick = new Dictionary<string, Queue<GameObject>>();
        
        foreach (pool pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i< pool.limit; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            PoolDick.Add(pool.tag, queue);


        }*/

        ItemPool = new ObjectPool<Item>(CreateObj, TakeObj, ReturnToPool, DestroyObj, false, 20, 100);

    }

    /*public GameObject SpawnTrash(string tag, Vector3 position , Quaternion rotation)
    {

        if (PoolDick.ContainsKey(tag))
        {
            return null;
        }


        GameObject objectSpawn = PoolDick[tag].Dequeue();

        objectSpawn.SetActive(true);
        objectSpawn.transform.position = position;
        objectSpawn.transform.rotation = rotation;

        PoolDick[tag].Enqueue(objectSpawn);

        return objectSpawn;
    }*/

    private Item CreateObj()
    {
        Item instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        /*instance.onDisable += ReturnObj;*/
        instance.gameObject.SetActive(false);

        return instance;
    }
    
    private void ReturnObj(Item Instance)
    {
        ItemPool.Release(Instance);
    }


    private void TakeObj(Item Instance)
    {
        Instance.gameObject.SetActive(true);
        SpawnItem(Instance);
        Instance.transform.SetParent(transform, false);
    }

    private void ReturnToPool(Item Instance)
    {
        Instance.gameObject.SetActive(false);
    }

    private void DestroyObj(Item Instance)
    {
        Destroy(Instance.gameObject);
    }


    private void SpawnItem(Item Instance)
    {

    }


}

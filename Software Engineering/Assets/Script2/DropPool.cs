using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Pool;

public class DropPool : MonoBehaviour
{
    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int limit;
    }

    #region Instance
    public static DropPool Instance;

    public void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #endregion

    public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDick;
    /*public PooledObject<>*/

    public void Start()
    {
        PoolDick = new Dictionary<string, Queue<GameObject>>();
        
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


        }

    }

    public GameObject SpawnTrash(string tag, Vector3 position , Quaternion rotation)
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
    }














}
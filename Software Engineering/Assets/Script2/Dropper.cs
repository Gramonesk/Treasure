using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    DropPool drop;

    private void Start()
    {
        drop = DropPool.Instance;
    }


    public void DropTrash()
    {
      drop.SpawnTrash("Raw", transform.position, Quaternion.identity);
    }



}

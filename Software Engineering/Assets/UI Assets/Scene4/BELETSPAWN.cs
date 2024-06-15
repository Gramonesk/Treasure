using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BELETSPAWN : MonoBehaviour
{
    public float spawnrate;
    public float timer;
    public GameObject belet;
    Transform spawnbelet;

    private void Start()
    {
        timer = spawnrate;
    }
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Instantiate(belet, spawnbelet);
            timer = spawnrate;
        }
    }
}

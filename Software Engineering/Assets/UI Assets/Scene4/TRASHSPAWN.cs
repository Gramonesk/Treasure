using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRASHSPAWN : MonoBehaviour
{
    public float spawnrate;
    public float timer;
    public GameObject TRASH;
    public Transform spawntrash;

    private void Start()
    {
        timer = spawnrate;
    }
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Instantiate(TRASH, spawntrash);
            timer = spawnrate;
        }
    }
}

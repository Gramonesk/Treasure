using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Start_EndFlag : MonoBehaviour
{
    public bool Start = true;
    public float time = 0;
    private void Update()
    {
        if (!Start)
        {
            time += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Start)
        {
            time = 0;
        }else
        {
            Debug.Log("Time Elapsed :" + time);
        }
        Start = !Start;
    }
}

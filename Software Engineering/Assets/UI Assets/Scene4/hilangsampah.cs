using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hilangsampah : MonoBehaviour
{
    void Update()
    {
        //Invoke("ded", 20);
    }

    private void ded()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("bunuh"))
        {
            ded();
        }
    }
}

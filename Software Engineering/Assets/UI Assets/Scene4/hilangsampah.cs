using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hilangsampah : MonoBehaviour
{
    public static ConveyorBelt yes;
    void Update()
    {
        //Invoke("ded", 20);
    }

    private void ded()
    {
        yes.onBelt.Remove(this.gameObject);
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

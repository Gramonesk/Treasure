using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Player Config")]
    public Transform Hand;
    public Transform Ray;
    public FloatValue raydist;
    public FloatValue throwpower;
    public LayerMask layer;
    private PlayerMovement pmovement;
    private Item prevselected;
    private void Awake()
    {
        pmovement = GetComponentInParent<PlayerMovement>();   
    }
    public void Update()
    {
        if (Physics.Raycast(Ray.position, this.transform.forward, out RaycastHit hit, raydist.value, layer) == true)
        {
            GameObject item = hit.collider.gameObject;
            Item obj = item.GetComponent<Item>();
            if (obj == null) return;
            //obj.Highlight();
            if (Input.GetKeyDown(KeyCode.G) && Hand.childCount == 0)
            {
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.parent = Hand;
                item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
            else if (Input.GetKeyDown(KeyCode.F) && Hand.childCount == 1)
            {
                drop(item);
            }
            else if(Input.GetKeyDown(KeyCode.E) && Hand.childCount == 1)
            {
                drop(item);
                //obj.ApplyForce(this.transform.forward * throwpower.value);
                Debug.Log(obj + "throw");
            }
        }
    }
    public void drop(GameObject item)
    {
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.parent = null;
    }
}

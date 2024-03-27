using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour, IThrowable
{
    [Tooltip("Weight Multiplier Affects the force of thrown item")]
    [SerializeField] private MaterialSO highlight_material;
    [SerializeField] private float Weight_Multiplier;
    public Rigidbody rb;
    private Material default_material;
    private Renderer renderer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        default_material = GetComponent<Material>();
        renderer = GetComponent<Renderer>();
    }
    public void ApplyForce(Vector3 force) //include force power
    {
        rb.AddForce(force * (1/Weight_Multiplier));
    }
    public void Highlight()
    {
        renderer.materials[2] = highlight_material.material;
    }
    public void UnHighlight()
    {
        renderer.materials[2] = null;
    }
 
}

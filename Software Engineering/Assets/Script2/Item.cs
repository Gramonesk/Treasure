using Fusion;
using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Fusion.NetworkBehaviour;

[Serializable]
public class State
{
    public TrashState state;
    public Mesh mesh;
    public Material material;
}
[RequireComponent(typeof(Rigidbody), typeof(NetworkRigidbody3D))]
public class Item : NetworkBehaviour
{
    [Header("Item Settings")]
    public List<State> ItemStates;
    public GameObject mat;

    public Dictionary<TrashState, State> statemap = new Dictionary<TrashState, State>();

    public Action OnPicked;
    public Action<Item> OnDropped;
    //[Networked] private MeshRenderer mrender { get; set; }
    //[Networked] private MeshFilter mfilter { get; set; }
    private MeshFilter mfilter;
    private MeshRenderer mrender;
    private void Start()
    {
        mfilter = mat.GetComponent<MeshFilter>();
        mrender = mat.GetComponent<MeshRenderer>();
        foreach(State s in ItemStates)
        {
            statemap.Add(s.state, s);
        }
        mfilter.mesh = ItemStates[0].mesh;
        mrender.material = ItemStates[0].material;
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_ChangeTo(TrashState state)
    {
        RPC_Change(state);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Change(TrashState state)
    {
        if (statemap.ContainsKey(state))
        {
            mfilter.mesh = statemap[state].mesh;
            mrender.material = statemap[state].material;
        }
    }
    //public void ChangeTo(TrashState state)
    //{
    //    if (!statemap.ContainsKey(state)) return;
    //    mfilter.mesh = statemap[state].mesh;
    //    mrender.material = statemap[state].material;
    //}

    //[Tooltip("Weight Multiplier Affects the force of thrown item")]
    //[SerializeField] private MaterialSO highlight_material;
    //[SerializeField] private float Weight_Multiplier;
    //public Rigidbody rb;
    //private Material default_material;
    //private Renderer renderer;
    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    default_material = GetComponent<Material>();
    //    renderer = GetComponent<Renderer>();
    //}
    //public void ApplyForce(Vector3 force) //include force power
    //{
    //    rb.AddForce(force * (1/Weight_Multiplier));
    //}
    //public void Highlight()
    //{
    //    renderer.materials[2] = highlight_material.material;
    //}
    //public void UnHighlight()
    //{
    //    renderer.materials[2] = null;
    //}
}

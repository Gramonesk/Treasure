using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum TrashState{
    raw,
    crushed,
    burned,
    incinerated,
    grinded,
    sold
}
[Serializable]
public class Convertion
{
    [Tooltip("After a Certain Duration, change the state")]
    public float Duration;
    public List<TrashState> FromState;
    public TrashState ChangeTo;
}
public class Facility : NetworkBehaviour
{
    [Header("These settings should be used on a child object as\n the IsTrigger to add lever functions or interactable\n functions dont forget to use Event Listener\n with the function here on the (OnRaise) event")]
    [Header("====================================================================")]

    [Header("Main Settings")]
    //public List<Convertion> convertions;
    public Convertion Convertion;
    public bool UseInteractables;
    public bool CanGrabOnProcess;

    [Networked] private TickTimer delay { get; set; }
    //[Networked] private Action action { get; set; }

    [Networked] private Item item { get; set; }
    Action action;
    private int index = 0;
    public void FixedUpdate()
    {
        action?.Invoke();
    }
    public void StartProcess()
    {
        delay = TickTimer.CreateFromSeconds(Runner, Convertion.Duration);
        action = Process;
    }
    public void Process()
    {
        if (delay.ExpiredOrNotRunning(Runner))
        {
            action = null;
            item.RPC_ChangeTo(Convertion.ChangeTo);
            index++;
            StopProcess();
        }
    }
    public void StopProcess()
    {
        index = 0;
        item.OnPicked = null;
        item.OnDropped = null;
        item = null;
        action = null;
        return;
    }
    private void Initiate(Item obj)
    {
        Debug.LogWarning(obj);
        item = obj;
        item.OnPicked = StopProcess;
        StartProcess();
    }
    private void OnTriggerStay(Collider other)
    {
        Item obj = other.GetComponent<Item>();
        if (obj == null) return;
        //ntar isi pake highlight (ini pake networked)
        if (!UseInteractables && item == null)
        {
            if (Convertion.FromState.Contains(obj.Current_State) && obj.statemap.ContainsKey(Convertion.ChangeTo))
            {
                Debug.Log("I can be activated");
                obj.OnDropped = Initiate;
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Item obj = other.GetComponent<Item>();
        if(obj == null) return;
        if (!UseInteractables && item == null)
        {
            item = obj;
            StopProcess();
        }
    }
}

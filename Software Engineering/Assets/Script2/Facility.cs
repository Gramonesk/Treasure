using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum TrashState{
    raw,
    crushed,
    incinerated,
    grinded, Composted,
    sold
}
[Serializable]
public class Convertion
{
    [Tooltip("After a Certain Duration, change the state")]
    public float Duration;
    public List<TrashState> FromState;
    /// <summary>
    /// BUAT MARIO
    /// ==================================
    /// CHANGETONYA DI ALTER AJA LEWAT SCRIPT BARU LAIN KALO MAU
    /// JADI PAS MILIH DI PROCESSOR CHANGETONYA DI SET,
    /// TERGANTUGN SI KALO SEMISAL CHANGETONYA INI JADI ITEM BEDA LG
    /// SILAHKAN SCRIPT BARU CMN GANTI KE PREFAB ITEM LAIN UDH YA
    /// ==================================
    /// </summary>
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
            StopProcess();
        }
    }
    public void StopProcess()
    {
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
        Debug.Log(other.name);
        if (!other.TryGetComponent<Item>(out var obj)) return;
        //ntar isi pake highlight (ini pake networked)
        if (!UseInteractables && item == null)
        {
            if (Convertion.FromState.Contains(obj.Current_State) && obj.statemap.ContainsKey(Convertion.ChangeTo))
            {
                Debug.Log("I can be activated");
                obj.CantPicked = true;
                obj.OnDropped = Initiate;
            }
            
        } else
        {
            obj.CantPicked = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Item>(out var obj)) return;
        if (!UseInteractables && item == null)
        {
            item = obj;
            StopProcess();
        }
    }
}

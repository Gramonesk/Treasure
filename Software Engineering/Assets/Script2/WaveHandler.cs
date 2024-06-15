using Fusion;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    public TextMeshProUGUI TimeUI;

    [Networked] public TickTimer timer { get;set; }

    public int GameTime;
    public int TimeStart;
    [Networked] public int GameTimeLeft {  get; set; }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]

    public void StartTimer()
    {
            StopCoroutine(UpdateTimer());
            StartCoroutine(UpdateTimer());

    }
    public IEnumerator UpdateTimer()
    {
        Debug.Log("Start Timer");
        while (true)
        {

            yield return new WaitForSeconds(1f);
            /*timer = TickTimer.CreateFromSeconds(Runner ,30);*/
            GameTime++;
            GameTimeLeft = TimeStart - GameTime;

            UpdateTimerUI(GameTimeLeft);

        }
        
    }

    public void UpdateTimerUI(int timeleft)
    {

        TimeUI.text = string.Format("{0:00}:{1:00}", timeleft / 60, timeleft % 60);

    }

    




    /*public override void FixedUpdateNetwork()
    {
       
        if (timer.Expired(Runner))
        {
            // Execute Logic

            // Reset timer
            timer = TickTimer.None;
            // alternatively: timer = default.

            Debug.Log("Timer Expired");
        }

        *//*TimeUI.text = string.Format("{0:00}:{1:00}", timer);*//*
    }*/



}

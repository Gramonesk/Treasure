using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveHandler : NetworkBehaviour
{
    public TextMeshProUGUI TimeUI;

    [Networked] public TickTimer timer { get;set; }


    public void StartTimer()
    {
        Debug.Log("Start Timer");
        timer = TickTimer.CreateFromSeconds(Runner ,30);
    }


    public override void FixedUpdateNetwork()
    {
        if (timer.Expired(Runner))
        {
            // Execute Logic

            // Reset timer
            timer = TickTimer.None;
            // alternatively: timer = default.

            Debug.Log("Timer Expired");
        }

        /*TimeUI.text = string.Format("{0:00}:{1:00}", timer);*/
    }



}

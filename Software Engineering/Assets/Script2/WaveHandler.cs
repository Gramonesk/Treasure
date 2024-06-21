using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class WaveHandler : MonoBehaviour
{
    public TextMeshProUGUI TimeUI;

    [Networked] public TickTimer timer { get; set; }

    public int GameTime;
    public int TimeStart;
    [Networked] public int GameTimeLeft { get; set; }


    [SerializeField] private RectTransform panelplayer;
    [SerializeField] private RectTransform panelInfo;


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]


    private void Awake()
    {
        panelInfo.DOAnchorPosY(130, 0f);
    }


    public void StartTimer()
    {
        StopCoroutine(UpdateTimer());
        StartCoroutine(UpdateTimer());
        PanelAnimation();
        StartSpawn();
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

    public void StopTimer()
    {
        StopCoroutine(UpdateTimer());
    }

    public void UpdateTimerUI(int timeleft)
    {

        TimeUI.text = string.Format("{0:00}:{1:00}", timeleft / 60, timeleft % 60);

    }

    private void PanelAnimation()
    {
        panelplayer.DOAnchorPosX(500, 1).SetEase(Ease.InOutSine);
        panelInfo.DOAnchorPosY(-85, 1.3f).SetEase(Ease.InOutSine);
    }
    
    public void StartSpawn()
    {
        DropPool spawning = GameObject.FindAnyObjectByType<DropPool>().GetComponent<DropPool>();
        spawning.CreateObj();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class HomeButton : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] RectTransform PausePanel;
    [SerializeField] CanvasGroup PauseDarkPanel;
    [SerializeField] GameObject SettingPanel;

    public void Awake()
    {
        SettingPanel.SetActive(false);
    }


    public void Pause()
    {
        PauseMenu.SetActive(true);
        PauseIntro();
    }

    public async void Resume()
    {
        await PauseOutro();
        PauseMenu.SetActive(false);
    }
    
    void PauseIntro()
    {
        PauseDarkPanel.DOFade(1, 0.5f);
        PausePanel.DOAnchorPosY(0, 0.5f).SetEase(Ease.InOutSine);
    }

    async Task PauseOutro()
    {
        PauseDarkPanel.DOFade(0, 0.5f);
        await PausePanel.DOAnchorPosY(900, 0.5f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }

    public void Setting()
    {
        SettingPanel.SetActive(true);
    }




}

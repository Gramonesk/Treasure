using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class setingsc : MonoBehaviour
{
    public Animator animator;
    private Animator panelanim;
    private bool isopen;
    public GameObject Panel;
    public GameObject nungguon;

    private void Awake()
    {
        isopen = false;

        panelanim = Panel.GetComponent<Animator>();
    }
    public void goseting()
    {
        if (!isopen)
        {
            animator.SetBool("openseting", true);
            panelanim.SetBool("PANELup", true);
            isopen = true;
        }

        else
        {
            animator.SetBool("openseting", false);
            panelanim.SetBool("PANELup", false);
            isopen = false;
        }
    }

    public void nunggu()
    {
        nungguon.SetActive(true);
    }
}
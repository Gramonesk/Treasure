using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button Submit;

    public string Playername = null;
    public GameObject InputNameCanvas;

    public void Awake()
    {
/*        InputNameCanvas.SetActive(true);*/
        Submit.interactable = false;
    }

 /*   public void SubmitName()
    {

        Playername = nameInput.text;
        *//*BasicSpawner.instance.ConnectToRunner(nameInput.text);*//*
    }*/

    public void ActivateButton()
    {
        Debug.Log("NicknameGanti");
        Submit.interactable = true;
    }
}

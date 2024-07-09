using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        /*inputField.text = PlayerPrefs.GetString("PlayerNickname");*/
        /*BasicSpawner.instance.ConnectToRunner(inputField.text);*/
    }

    public void PlayScene(string name)
    {
        PlayerPrefs.SetString("PlayerNickname", inputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene(name);
    }

/*    public void SubmitName()
    {
        BasicSpawner.instance.ConnectToRunner(inputField.text);
    }*/


    public void Exit()
    {
        Application.Quit();
    }

}
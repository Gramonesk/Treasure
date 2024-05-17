using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject Panel;

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Setting()
    {
        Panel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back_Setting()
    {
        Panel.SetActive(false);
    }
}

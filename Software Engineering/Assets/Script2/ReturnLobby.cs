using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnLobby : MonoBehaviour
{
    public void ReturnToTheLobby()
    {
        BasicSpawner.ReturnToLobby();
    }
    public void LoadTheScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

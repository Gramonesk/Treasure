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
    public void returnScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

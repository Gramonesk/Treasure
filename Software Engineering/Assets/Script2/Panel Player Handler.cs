using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPlayerHandler : MonoBehaviour
{
    BasicSpawner spawner;

    public static PanelPlayerHandler instance;

    [Header("MainMenu Scene")]
    public static Transform panelPlayer;
    public static GameObject PanelPlayerPrefab;

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {
        if (instance == null) instance = this;
        spawner = GetComponent<BasicSpawner>();
    }

    public void UpdatePlayerPanel()
    {
        GameObject newEntry = GameObject.Instantiate(PanelPlayerPrefab);
        newEntry.transform.parent = panelPlayer;

        PanelPlayerPrefab entryPlayer = GetComponent<PanelPlayerPrefab>();
        entryPlayer.PlayerName.text = spawner._playername.ToString();
    }
    public void DeletePlayerPanel()
    {

        Destroy(PanelPlayerPrefab);
        /*GameObject newEntry = GameObject.Destroy(PanelPlayerPrefab);*/

    }
}

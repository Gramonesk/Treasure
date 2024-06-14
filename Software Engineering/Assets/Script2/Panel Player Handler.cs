using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPlayerHandler : MonoBehaviour
{
    [Header("MainMenu Scene")]
    public Transform panelPlayer;
    public GameObject PanelPrefab;

    public void Start()
    {
    }

    public void UpdatePlayerPanel()
    {
        GameObject newEntry = GameObject.Instantiate(PanelPrefab);
        newEntry.transform.parent = panelPlayer;

        PanelPlayerPrefab entryName = newEntry.GetComponent<PanelPlayerPrefab>();

        BasicSpawner player = GameObject.FindObjectOfType<BasicSpawner>().GetComponent<BasicSpawner>();
        entryName.PlayerName.text = player._playername.ToString();
    }
    public void DeletePlayerPanel()
    {

        /*Destroy(PanelPlayerPrefab);*/
        /*GameObject newEntry = GameObject.Destroy(PanelPlayerPrefab);*/

    }
}

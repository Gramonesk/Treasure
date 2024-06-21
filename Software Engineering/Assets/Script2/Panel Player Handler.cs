using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class PanelPlayerHandler : MonoBehaviour
{
    [Header("MainMenu Scene")]
    public Transform panelPlayer;
    public GameObject PanelPrefab;
    public bool _isReady = false;
    public string status = "Ready";
    public PanelPlayerPrefab _PanelPlayerPrefab;


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Start()
    {
        _PanelPlayerPrefab = GetComponent<PanelPlayerPrefab>();
    }

    public void UpdatePlayerPanel()
    {
        GameObject newEntry = GameObject.Instantiate(PanelPrefab);
        Debug.Log("Create Player Status");
        /*Runner.Spawn(PanelPrefab, panelPlayer.parent.position);*/
        newEntry.transform.parent = panelPlayer;
        PanelPlayerPrefab entryName = newEntry.GetComponent<PanelPlayerPrefab>();
        entryName.PlayerName_.text = FindAnyObjectByType<Player>().GetComponent<Player>().Nickname.ToString();

    }
    public void DeletePlayerPanel()
    {

        Destroy(PanelPrefab);
        /*GameObject newEntry = GameObject.Destroy(PanelPlayerPrefab);*/

    }

    public void UpdatePlayerStatus()
    {
        Debug.Log("Update Ready");
        PanelPlayerPrefab readyEntry = PanelPrefab.GetComponent<PanelPlayerPrefab>();

        if (_isReady == false)
        {
            Debug.Log("READYYYYYYYYYYY SIRRRRRRRRR");
            readyEntry.ReadyStatus_.text = status.ToString();
            readyEntry.ReadyStatus_.color = Color.green;
            Debug.Log($"Status = {readyEntry.ReadyStatus_.text}");
        }
        else
        {
            readyEntry.ReadyStatus_.text = "Not Ready";
            readyEntry.ReadyStatus_.color = Color.red;
        }
        _isReady = !_isReady;
    }


}

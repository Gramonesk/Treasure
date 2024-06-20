using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PanelPlayerHandler : MonoBehaviour
{
    [Header("MainMenu Scene")]
    public Transform panelPlayer;
    public GameObject PanelPrefab;
    public bool _isReady = false;
    public string status = "Ready";


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
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

    public void UpdatePlayerStatus()
    {
        Debug.Log("Update Ready");
        PanelPlayerPrefab readyEntry = PanelPrefab.GetComponent<PanelPlayerPrefab>();

        if (_isReady == false)
        {
            Debug.Log("READYYYYYYYYYYY SIRRRRRRRRR");
            readyEntry.ReadyStatus.text = status.ToString();
            readyEntry.ReadyStatus.color = Color.green;
            Debug.Log($"Status = {readyEntry.ReadyStatus.text}");
        }
        else
        {
            readyEntry.ReadyStatus.text = "Not Ready";
            readyEntry.ReadyStatus.color = Color.red;
        }
        _isReady = !_isReady;
    }


}

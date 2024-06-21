using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PanelPlayerHandler : NetworkBehaviour
{
    [Header("MainMenu Scene")]
    public Transform panelPlayer;
    public GameObject PanelPrefab;
    public bool _isReady = false;
    public string status = "Ready";
    public PanelPlayerPrefab _PanelPlayerPrefab;
    

    /*[Rpc(RpcSources.StateAuthority, RpcTargets.All)]*/
    public void Start()
    {
        _PanelPlayerPrefab = GetComponent<PanelPlayerPrefab>();
    }

    public void UpdatePlayerPanel()
    {
        /*GameObject newEntry = GameObject.Instantiate(PanelPrefab);*/

        Runner.Spawn(PanelPrefab, panelPlayer.position);
        /*newEntry.transform.parent = panelPlayer;
        PanelPlayerPrefab entryName = newEntry.GetComponent<PanelPlayerPrefab>();*/

        

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

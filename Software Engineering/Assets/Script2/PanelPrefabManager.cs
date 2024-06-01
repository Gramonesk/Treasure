using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelPrefabManager : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playerCount;
    public Button joinButton;
    /*public bool isJoin = false;*/

    private void Awake()
    {
        joinButton.onClick.AddListener(JoinSession);
    }

    private void Start()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    private void JoinSession()
    {
        /*isJoin = true;*/
        BasicSpawner.instance.StartGame(Fusion.GameMode.Client);
    }






}

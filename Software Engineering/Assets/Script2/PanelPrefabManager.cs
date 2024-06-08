using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelPrefabManager : MonoBehaviour
{
    public TextMeshProUGUI SessionName;
    public TextMeshProUGUI playerCount;
    public Button joinButton;
    public GameObject PanelCanvas;
    public static PanelPrefabManager Instance;

    /*public bool isJoin = false;*/

    private void Awake()
    {
        joinButton.onClick.AddListener(JoinSession);
        if (Instance == null) { Instance = this; }
    }

    private void Start()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    private void JoinSession()
    {
        /*isJoin = true;*/
        BasicSpawner.instance.JoinSession(SessionName.text);
        PanelCanvas.SetActive(false);

    }






}

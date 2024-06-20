using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelPlayerPrefab : MonoBehaviour
{
    public TextMeshProUGUI PlayerName_;
    public TextMeshProUGUI ReadyStatus_;
    [Networked] public TextMeshProUGUI PlayerName { get => PlayerName_; set => PlayerName_ = value; }
    [Networked] public TextMeshProUGUI ReadyStatus {  get => ReadyStatus_; set => ReadyStatus_ = value; }

}

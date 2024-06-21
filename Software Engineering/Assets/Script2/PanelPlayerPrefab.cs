using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Fusion.NetworkBehaviour;

public class PanelPlayerPrefab : NetworkBehaviour
{
    public TextMeshProUGUI PlayerName_;
    public TextMeshProUGUI ReadyStatus_;
    // [Networked] public TextMeshProUGUI PlayerName { get => PlayerName_; set => PlayerName_ = value; }
    // [Networked] public TextMeshProUGUI ReadyStatus {  get => ReadyStatus_; set => ReadyStatus_ = value; }
    public ChangeDetector _changeDetector;


    [Networked] public NetworkString<_16> PlayerNickname { get; set; }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (this.HasStateAuthority)
        {
            PlayerNickname = Runner.GetComponent<Player>().Nickname;
        }
        PlayerNickname = Runner.GetComponent<Player>().Nickname;


    }


    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(PlayerNickname):
                    PlayerName_.text = PlayerNickname.ToString();
                    break;
            }
        }
    }













}

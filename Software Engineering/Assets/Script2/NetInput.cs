using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputButton
{
    Left_Click,
    Right_Click,
    Jump,

}
//INetwork = allows struct to feed to fusion so it can be replicated properly to server
public struct NetInput : INetworkInput
{   
    public NetworkButtons buttons;
    public Vector3 direction;
}
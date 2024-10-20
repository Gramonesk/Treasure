using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//attached to networkrunner prefab so we cant make it a networkobject
//as runner is not a networkobject

//SimulationBehaviour = in order to make fusion callbacks work outside a network 

//Simulation = Tick vs Frame
//Framerate how frequently screen updated
//tick how frequently the simulation has updated with each tick representing a fixed amount of time passing

public class InputManager : SimulationBehaviour, IBeforeUpdate, INetworkRunnerCallbacks
{
    private NetInput accumulatedInput;
    private bool resetInput;
    //runs before fusion executes it network loop
    public void BeforeUpdate()
    {
        if (resetInput)
        {
            resetInput = false;
            accumulatedInput = default;
        }
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Cursor.lockState != CursorLockMode.Locked) return;

        NetworkButtons buttons = default;
        if (keyboard != null)
        {
            Vector2 moveDirection = Vector2.zero;
            //if (keyboard.wKey.isPressed) moveDirection += Vector2.up;
            //if (keyboard.sKey.isPressed) moveDirection += Vector2.down;
            //if (keyboard.aKey.isPressed) moveDirection += Vector2.left;
            //if (keyboard.dKey.isPressed) moveDirection += Vector2.right;

            //accumulatedInput.Direction += moveDirection;
            buttons.Set(InputButton.Jump, keyboard.spaceKey.isPressed);
        }

        accumulatedInput.buttons = new NetworkButtons(accumulatedInput.buttons.Bits | buttons.Bits);

    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    //fusion takes input then make it available for local player and host
    //allows client to predict the local player movement, while also let the host calculate
    //movement and rotate authoratively
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        accumulatedInput.direction.Normalize();
        input.Set(accumulatedInput);
        resetInput = true;
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
        {
            //disconnected + tell menu to put main menu back on screen
        }
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}

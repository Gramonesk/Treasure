using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks, IBeforeUpdate
{
    public static BasicSpawner instance;

    private NetInput accumulatedInput;
    private bool resetInput;
    public NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    //Keyboard keyboard = Keyboard.current;

    [Header("Session List")]
    public GameObject RoomCanvas;
    private List<SessionInfo> _sessions = new List<SessionInfo>();
    public Button refreshButton;
    public Transform sessionListContent;
    public GameObject panelPrefab;


    Mouse mouse = Mouse.current;


    /*public PanelPrefabManager panelManager;*/

    private void Awake()
    {
        if (instance == null) { instance = this; }

    }


    void IBeforeUpdate.BeforeUpdate()
    {
        //Debug.Log("AKu Before Update");
        //if (resetInput)
        //{
        //    resetInput = false;
        //    accumulatedInput = default;
        //}
        //Keyboard keyboard = Keyboard.current;
        //Mouse mouse = Mouse.current;
        //if (keyboard != null && (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame))
        //{
        //    if (Cursor.lockState == CursorLockMode.Locked)
        //    {
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    }
        //    else
        //    {
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //    }
        //}

        //if (Cursor.lockState != CursorLockMode.Locked) return;

        //NetworkButtons buttons = default;
        //if (keyboard != null)
        //{
        //    Vector3 moveDirection = Vector3.zero;
        //    if (keyboard.wKey.isPressed) moveDirection += Vector3.forward;
        //    if (keyboard.sKey.isPressed) moveDirection += Vector3.back;
        //    if (keyboard.aKey.isPressed) moveDirection += Vector3.left;
        //    if (keyboard.dKey.isPressed) moveDirection += Vector3.right;

        //    accumulatedInput.direction += moveDirection;
        //    buttons.Set(InputButton.Jump, keyboard.spaceKey.isPressed);
        //}
        //if (mouse != null)
        //{
        //    buttons.Set(InputButton.Left_Click, mouse.leftButton.isPressed);
        //    buttons.Set(InputButton.Right_Click, mouse.rightButton.isPressed);
        //}
        //accumulatedInput.buttons = new NetworkButtons(accumulatedInput.buttons.Bits | buttons.Bits);

    }
    public async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        
        RunnerSimulatePhysics3D phys = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        phys.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateForward;
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            //Address = NetAddress.Any
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }

            /* if (panelManager.isJoin == true)
             {
                 StartGame(GameMode.Host);
             }*/

        }

    }

    public void joinSession()
    {
        StartGame(GameMode.Host);
        RoomCanvas.SetActive(false);
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner _runner, List<SessionInfo> sessionList)
    {
        _sessions.Clear();
        _sessions = sessionList;
        Debug.Log("Session Updated");
    }

    public void RefreshSessionListUI()
    {
        Debug.Log("refresh ke 2");
        // Clears Session List UI ( Biar gk ada clone )
        foreach (Transform child in sessionListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (SessionInfo session in _sessions)
        {
            Debug.Log("Cekin");
            if (session.IsVisible)
            {
                Debug.Log("sesi ada, refresh");
                GameObject entry = GameObject.Instantiate(panelPrefab, sessionListContent);
                PanelPrefabManager script = entry.GetComponent<PanelPrefabManager>();
                script.roomName.text = entry.name;
                script.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                {
                    script.joinButton.interactable = false;
                }
                else
                {
                    script.joinButton.interactable = true;
                }
            }
        }
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
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        //accumulatedInput.direction.Normalize();
        //input.Set(accumulatedInput);
        //resetInput = true;
        var data = new NetInput();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        data.buttons.Set(InputButton.Left_Click, mouse.leftButton.isPressed);
        data.buttons.Set(InputButton.Right_Click, mouse.rightButton.isPressed);
        input.Set(data);
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
        if (runner.IsServer)
        {
            // Create a unique position for the player
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
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


    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
        {
        }
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }


}

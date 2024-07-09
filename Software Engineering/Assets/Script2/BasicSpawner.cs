using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks, IBeforeUpdate
{
    public static BasicSpawner instance;

    /*NetworkSceneManagerDefault networkSceneManager;*/


    private NetInput accumulatedInput;
    private bool resetInput;
    public static NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    //Keyboard keyboard = Keyboard.current;

    
/*    void addScene()
    {
        networkSceneManager.
    }*/

    public interface INetworkSceneManager
    {

    }

    public string lobbyName = "default";
    [Networked]public static int playerCountNow { get;set; }

    
    public Dictionary<string, GameObject> sessionListUI = new Dictionary<string, GameObject>();
    [Header("Session List")]
/*  public Button refreshButton;*/
    public Transform sessionListContent;
    public GameObject panelPrefab;
    public GameObject PanelCanvas;

    Mouse mouse = Mouse.current;

    [Header("Name Input")]
    public GameObject NameCanvas;
    public TMP_InputField nameinputfield;
    public string _playername = null;


    // Ini Gak bisa di Build karena pakai namespace Unity.Editor
    [Header("Game Scene")]
    /*public SceneAsset GameScene;*/
    public string SceneMultiplier;
    

    [Header("Canvas inGame")]
    public GameObject uiInGame;
    /*public Scene GameScenes;*/


    /* [Header("MainMenu Scene")]
     public Transform panelPlayer;*/

    [Header("Room Name")]
    public TextMeshProUGUI Roomname;

    // public GameObject PanelPlayerPrefab;

    // public SceneAsset LobbyScene;





    /*public Button join;*/


    /*public PanelPrefabManager panelManager;*/

    private void Awake()
    {
        if (instance == null) { instance = this; }

        _runner = gameObject.GetComponent<NetworkRunner>();

        if (!_runner)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }

        DontDestroyOnLoad(_runner);

    }

    private void Start()
    {
        _runner.JoinSessionLobby(SessionLobby.Shared, lobbyName);

        /*var sceneManager = _runner.GetComponent<INetworkSceneManager>();*/

    }

    private void TryGetSceneRef(out SceneRef sceneRef)
    {
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.buildIndex < 0 || activeScene.buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            sceneRef = default;
        }
        else
        {
            sceneRef = SceneRef.FromIndex(activeScene.buildIndex);
        }
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner _runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Session Updated");
        DeleteOldSession(sessionList);

        CompareList(sessionList);

    }

    #region Create & Update List Session UI

    private void DeleteOldSession(List<SessionInfo> sessionList)
    {
        /*bool isContained = false;*/
        GameObject uiToDelete = null;

        foreach (KeyValuePair<string, GameObject> kvp in sessionListUI)
        {
            bool isContained = false;
            string sessionKey = kvp.Key;
            
            foreach (SessionInfo sessionInfo in sessionList)
            {
                if (sessionInfo.Name == sessionKey)
                {
                    Debug.Log("Tidak ada Sesi Yang Sama");
                    isContained = true;
                    break;
                }
                
            }

            if (!isContained)
            {
                uiToDelete = kvp.Value;
                sessionListUI.Remove(sessionKey);
                Destroy(uiToDelete);
            }
        }
    }

    private void CompareList(List<SessionInfo> sessionList)
    {
        foreach (SessionInfo session in sessionList)
        {
            if (sessionListUI.ContainsKey(session.Name))
            {
                UpdatePanelUI(session);
            } else
            {
                CreatePanelUI(session);
            }
        }
    }

    private void UpdatePanelUI(SessionInfo session)
    {
        Debug.Log("UpdatedPanel");
        sessionListUI.TryGetValue(session.Name, out GameObject newEntry);

        PanelPrefabManager entryScript = newEntry.GetComponent<PanelPrefabManager>();

        entryScript.SessionName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;
        /*playerCountNow = session.PlayerCount;*/

        // Optional 
        newEntry.SetActive(session.IsVisible);
    }

    private void CreatePanelUI(SessionInfo session)
    {
        Debug.Log("CreatedPanel");
        GameObject newEntry = GameObject.Instantiate(panelPrefab);
        newEntry.transform.parent = sessionListContent;
        PanelPrefabManager entryScript = newEntry.GetComponent<PanelPrefabManager>();
        sessionListUI.Add(session.Name, newEntry);

        entryScript.SessionName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;
        /*playerCountNow = session.PlayerCount;*/

        // Optional
        newEntry.SetActive(session.IsVisible);
    }

    #endregion

    public static void ReturnToLobby()
    {
        Debug.Log("ReturnToLobby");

        BasicSpawner._runner.Shutdown(true, ShutdownReason.Ok);
    }

    public int ReturnPlayerCount(SessionInfo session)
    {
        return session.PlayerCount;
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
    public void RunnerName()
    {
        Debug.Log("Name Input Terisi");
        _playername = nameinputfield.text;

    }

    public int GetSceneIndex(string SceneName)
    {
        // Loop Through all scenes in the build Settings
        for (int i = 0; i <SceneManager.sceneCountInBuildSettings; i++)
        {
            // Get The Scene Path
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            // Extract the Name of the scene from the path
            string name = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            // Check if the names match
            if (name == SceneName)
            {
                return i;
            }
        }
        return -1;
    }


    public void startCreateSession()
    {
        Invoke("CreateSession", 3f);
    }


    async public void CreateSession()
    {
        int randomInt = UnityEngine.Random.Range(100, 999);
        string _SessionName = "Room" + randomInt;
        Debug.Log(_SessionName);
        Debug.Log(sessionListUI);
        Roomname.text = _SessionName;


        // Create the Fusion runner and let it know that we will be providing user input
        if (!_runner)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
        }
        
        RunnerSimulatePhysics3D phys = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        phys.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateForward;
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        //var scene = SceneRef.FromIndex(1);
        //var scene = SceneRef.FromIndex(SceneManager.GetSceneByName("Scene4Conveyor1").buildIndex);

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            /*GameMode = GameMode.Shared,*/
            SessionName = _SessionName,
            //Address = NetAddress.Any
            Scene = scene,
            EnableClientSessionCreation = true,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 4,
        });
        /*StartCoroutine(Load("TestMultiplayer", _SessionName));*/
    }
    /*IEnumerator Load(string sceneName, string _SessionName)
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneName);
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var scene = SceneRef.FromIndex(GetSceneIndex(GameScene.name)); // Jangan Dipakai
        // var scene = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/TestMultiplayer.unity"));
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        // Start or join (depends on gamemode) a session with a specific name
        _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = _SessionName,
            //Address = NetAddress.Any
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 4,
        });
    }*/
    async public void JoinSession(string _SessionName)
    {

        Roomname.text = _SessionName;
        PanelCanvas.SetActive(false);
        if(_runner == null)
        {
            _runner= gameObject.AddComponent<NetworkRunner>();
        }

        RunnerSimulatePhysics3D phys = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        phys.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateForward;
        _runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            /*GameMode = GameMode.Shared,*/
            SessionName = _SessionName,
            //Address = NetAddress.Any
            Scene = scene,
            EnableClientSessionCreation = false,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 4,
        });

        /*StartCoroutine(Load("TestMultiplayer", _SessionName));*/
    }

    /*public void StartTheWave()
    {
        WaveHandler wave = GameObject.FindObjectOfType<WaveHandler>().GetComponent<WaveHandler>();
        wave.StartTimer();
    }*/

    /*private void OnGUI()
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

             if (panelManager.isJoin == true)
             {
                 StartGame(GameMode.Host);
             }

        }

    }*/



    /*public void RefreshSessionListUI()
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
    }*/

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
        Debug.Log("Player Joined");
        if (runner.IsServer)
        {
            // Masukin Canvas UI in Game

            
            // Create a unique position for the player
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
            uiInGame.SetActive(true);
        }
        /*if (player == _runner.LocalPlayer)
        {

            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
            _spawnedCharacters.Add(player, networkPlayerObject);
        }*/
        playerCountNow++;
        Debug.Log($"Total Player = {playerCountNow}");

        PanelPlayerHandler panelPHand = GameObject.FindObjectOfType<PanelPlayerHandler>().GetComponent<PanelPlayerHandler>();
        panelPHand.UpdatePlayerPanel();
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
        playerCountNow--;
        Debug.Log($"Total Player = {playerCountNow}");
        PanelPlayerHandler panelPHand = GameObject.FindObjectOfType<PanelPlayerHandler>().GetComponent<PanelPlayerHandler>();
        panelPHand.DeletePlayerPanel();
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
        Debug.Log("ShutDown");
        playerCountNow = 0;
        /*Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (shutdownReason == ShutdownReason.DisconnectedByPluginLogic)
        {
        }*/
        /*NameCanvas.SetActive(true);*/
        /*SceneManager.LoadScene(LobbyScene.name);*/
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void LoadTheScene(string name)
    {
        SceneManager.LoadScene(name);
    }



}

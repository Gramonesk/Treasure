//using Fusion;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

////[Networked] Property is a property that copies the data to all client
////[Capacity(int)] -> absolute limit capacity
////NetworkBehaviour handles network properties and rpcs
//public class GameLogic : NetworkBehaviour, IPlayerJoined, IPlayerLeft
//{
//    private NetworkRunner _runner;
//    [SerializeField] private NetworkPrefabRef playerPrefab;
//    [SerializeField] private NetworkPrefabRef Runner;

//    [Networked, Capacity(4)] private NetworkDictionary<PlayerRef, Player> Players => default;
//    private void OnGUI()
//    {
//        if (_runner == null)
//        {
//            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
//            {
//                StartGame(GameMode.Host);
//            }
//            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
//            {
//                StartGame(GameMode.Client);
//            }
//        }
//    }
//    async void StartGame(GameMode mode)
//    {
//        _runner = gameObject.AddComponent<NetworkRunner>();
//        gameObject.AddComponent<InputManager>();
//        _runner.ProvideInput = true;
//        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
//        var sceneInfo = new NetworkSceneInfo();
//        if (scene.IsValid)
//        {
//            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
//        }

//        // Start or join (depends on gamemode) a session with a specific name
//        await _runner.StartGame(new StartGameArgs()
//        {
//            GameMode = mode,
//            SessionName = "TestRoom",
//            Scene = scene,
//            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
//        });
//    }
//    public void PlayerJoined(PlayerRef player)
//    {
//        //is the host(responsible for spawning other players (spawn on our host and replicate
//        //spawn to other client so everyone see the player object
//        if (HasStateAuthority)
//        {
//            NetworkObject playerObject = _runner.Spawn(playerPrefab, Vector3.up, Quaternion.identity, player);
//            Players.Add(player, playerObject.GetComponent<Player>());
//        }
//    }

//    public void PlayerLeft(PlayerRef player)
//    {
//        if (HasStateAuthority) return;
//        if (Players.TryGet(player, out Player playerBehaviour))
//        {
//            Players.Remove(player);
//            _runner.Despawn(playerBehaviour.Object);
//        }
//    }
//}

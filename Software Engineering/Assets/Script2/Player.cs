using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.KCC;
using TMPro;
using static Fusion.NetworkBehaviour;
using System;
using Unity.VisualScripting;

public class Player : NetworkBehaviour
{
    public static Player Instance; 


    [Header("Player Settings")]
    public Transform hand;
    public Transform detector;
    public float distance;
    public LayerMask layermask;
    public TextMeshProUGUI playernickname;

    public NetworkPrefabRef prefab;

    [Networked/*, OnChangedRender(nameof(UpdatePlayerName))*/] public NetworkString<_16> Nickname { get; set; }

    [Header("Player Ready")]
    public bool _isReady = false;

    [Networked] public int ReadyCount {  get; set; }
    [Networked] public TickTimer CountDown {  get; set; }



    public bool allReady = false;

    private Material _material;
    [HideInInspector][Networked] public bool spawnedProjectile { get; set; }
    private ChangeDetector _changeDetector;
    //[Networked] private TickTimer delay { get; set; }
    

    private RaycastHit ray;
    //[SerializeField] private Ball _prefabBall;
    //[SerializeField] private PhysxBall _prefabPhysxBall;

    //private Vector3 _forward = Vector3.forward;
    private NetworkCharacterController _cc;
    public Animator PlayerAnimation; 
    private TMP_Text _messages;

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    public void awake()
    {
        if (Instance == null) Instance = this;
    }


    private void Start()
    {
        
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]



    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (_messages == null)
            _messages = FindObjectOfType<TMP_Text>();

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

        _messages.text = message;
    }
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _material = GetComponentInChildren<MeshRenderer>().material;
    }
    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        ReadyCount = 0;
        /*if (HasInputAuthority)
        {
            Nickname = PlayerPrefs.GetString("PlayerNickname");
            RPC_PlayerName(Nickname);
        } else
        {
            Nickname = PlayerPrefs.GetString("PlayerNickname");
            RPC_PlayerName(Nickname);
        }*/

        PlayerAnimation = GetComponent<Animator>();
        if (this.HasStateAuthority)
        {
            Debug.Log("PUNYA STATE AUTHORITY");
            /*Nickname = BasicSpawner.instance._playername;*/
            Nickname = Runner.GetComponent<BasicSpawner>()._playername;
        }
        Nickname = Runner.GetComponent<BasicSpawner>()._playername;
        transform.gameObject.name = playernickname.text;


        if(this.HasStateAuthority && allReady == true)
        {
            WaveHandler wave = GameObject.FindObjectOfType<WaveHandler>().GetComponent<WaveHandler>();
            wave.StartTimer();
        }

    }
    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R) && allReady == false)
        {
            PanelPlayerHandler panel = GameObject.FindObjectOfType<PanelPlayerHandler>().GetComponent<PanelPlayerHandler>();
            panel.UpdatePlayerStatus();
            /*RPC_SendMessage("Im Ready!");*/
            Debug.Log(BasicSpawner.playerCountNow);
            /*Runner.Spawn(prefab, Vector3.up);*/
            _isReady = !_isReady;
            if (_isReady)
            {
                Debug.Log("Aku Ready");
                ReadyCount++;
                Debug.Log(ReadyCount);

            }
            else
            {
                Debug.Log("Aku Tidak Ready");
                ReadyCount--;
                Debug.Log(ReadyCount);
            }
        }
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(ReadyCount):
                    if (ReadyCount == BasicSpawner.playerCountNow)
                    {
                        Debug.Log("Ready Semua");
                        /*BasicSpawner basicSpawner = GameObject.FindObjectOfType<BasicSpawner>().GetComponent<BasicSpawner>();
                        basicSpawner.StartTheWave();*/
                        WaveHandler wave = GameObject.FindObjectOfType<WaveHandler>().GetComponent<WaveHandler>();
                        wave.StartTimer();
                        allReady = true;
                        break;

                    }
                    break;
                case nameof(Nickname):
                    playernickname.text = Nickname.ToString();
                    transform.gameObject.name = playernickname.text;
                    break;
            }
        }


    }
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    _material.color = Color.white;
                    break;
                case nameof(Nickname):
                    playernickname.text = Nickname.ToString();
                    transform.gameObject.name = playernickname.text;
                    break;
                case nameof(ReadyCount):
                    if (ReadyCount == BasicSpawner.playerCountNow)
                    {
                        Debug.Log("Ready Semua");
                    }
                    break;
            }
        }
        // PlayerAnimation.SetFloat("Speed", _cc.Velocity.magnitude);

        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }


    private NetworkObject item;
    private Item obj;
    public override void FixedUpdateNetwork()
    {
        PlayerAnimation.SetFloat("Speed", _cc.Velocity.magnitude);

        if (GetInput(out NetInput data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
            
            Debug.Log(_cc.Velocity.magnitude);
            if (HasStateAuthority)
            {
                if (data.buttons.IsSet(InputButton.Left_Click))
                {
                    if (hand.transform.childCount == 1)
                    {
                        item.GetComponent<Rigidbody>().isKinematic = false;
                        item.transform.parent = null;
                        obj.OnDropped?.Invoke(obj);
                        //obj.ApplyForce(this.transform.forward);
                        spawnedProjectile = !spawnedProjectile;
                    }
                }
                else if (data.buttons.IsSet(InputButton.Right_Click))
                {
                    if (hand.transform.childCount == 0)
                    {
                        if (Physics.BoxCast(transform.position, transform.localScale * 0.5f, detector.forward, out ray, transform.rotation, distance, layermask))
                        {
                            item = ray.collider.GetComponent<NetworkObject>();
                            obj = item.GetComponent<Item>();
                            if (obj == null) return;
                            ray.collider.GetComponent<Rigidbody>().isKinematic = true;
                            item.transform.parent = hand;
                            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                            obj.OnPicked?.Invoke();
                            spawnedProjectile = !spawnedProjectile;
                        }
                    }
                }
            }
            //if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            //{
            //    if (data.buttons.IsSet(InputButton.Left_Click))
            //    {
            //        delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            //        Runner.Spawn(_prefabBall,
            //        transform.position + _forward, Quaternion.LookRotation(_forward),
            //        Object.InputAuthority, (runner, o) =>
            //        {
            //            o.GetComponent<Ball>().Init();
            //        });
            //        spawnedProjectile = !spawnedProjectile;
            //    }
            //    else if (data.buttons.IsSet(InputButton.Right_Click))
            //    {
            //        delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            //        Runner.Spawn(_prefabPhysxBall,
            //        transform.position + _forward, Quaternion.LookRotation(_forward),
            //        Object.InputAuthority, (runner, o) =>
            //        {
            //            o.GetComponent<PhysxBall>().Init(10 * _forward);
            //        });
            //        spawnedProjectile = !spawnedProjectile;
            //    }
            //}
        }
    }


    public void CountDownStart()
    {
        CountDown = TickTimer.CreateFromSeconds(Runner, 5f);


    }


/*    static void OnNicknameChanged()
    {

    }*/

    /*private void onNicknameChanged()
    {
        playernickname.text = Nickname.ToString();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_PlayerName(string name)
    {
        Nickname = name;
        Debug.Log($"Nickname changed for to {Nickname} with {name}");
        playernickname.text = name.ToString();
    }*/



}

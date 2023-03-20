using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Collections;
using System.Text;

[RequireComponent(typeof(NetworkBroker))]
public class NetworkClientBroker : MonoBehaviour
{
    public static NetworkClientBroker Instance { get; private set; }

    //private static ClientGameNetPortal instance;
    //public DisconnectReason DisconnectReason { get; private set; } = new DisconnectReason();

    //public event Action<ConnectionStatus> OnConnectionFinished;

    //public event Action OnNetworkTimedOut;

    //private GameNetPortal gameNetPortal;

    //private static int counter;
    //private int count;


    public enum Status
    {
        Undefined,
        Connected,
        ServerFull,
        GameInProgress,
        //LoggedInAgain,
        //UserRequestedDisconnect,
        //GenericDisconnect
    }

    #region init => destroy

    private void Awake()
    {
        //Debug.Log("ClientAwake: " + count);
        //count = counter++;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    private void Start()
    {
        //gameNetPortal = GetComponent<GameNetPortal>();

        //NetworkBroker.Instance.OnNetworkStatusChanged += HandleNetworkReadied;

        //gameNetPortal.OnNetworkReadied += HandleNetworkReadied;
        //gameNetPortal.OnConnectionFinished += HandleConnectionFinished;
        //gameNetPortal.OnDisconnectReasonReceived += HandleDisconnectReasonReceived;

        //NetworkManager.Singleton.OnServerStarted += HandleServerStarted;        //NetworkManager.Singleton.OnServerStarted += HandleServerStarted;

        //NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void OnDestroy()
    {
        //Debug.Log("ClientDestroy: " + count);

        //if (gameNetPortal == null) { return; }

        //gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;
        //gameNetPortal.OnConnectionFinished -= HandleConnectionFinished;
        //gameNetPortal.OnDisconnectReasonReceived -= HandleDisconnectReasonReceived;

        if (NetworkManager.Singleton == null) { return; }

        //NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;

        //NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;

        if (Instance == this) Instance = null;
    }

    //private void HandleServerStarted()
    //{
    //    Debug.Log("NW: HandleServerStarted");
    //    if (NetworkManager.Singleton.IsClient == false) return;

    //    if (NetworkManager.Singleton.IsHost == false)
    //    {
    //        //gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
    //    }
    //}

    //private void HandleClientDisconnect(ulong clientId)
    //{
    //    Debug.Log("HandleClientDisconnect" + clientId);

    //    //if (NetworkManager.Singleton.IsConnectedClient == false && NetworkManager.Singleton.IsHost == false)
    //    //{
    //    //    //gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;

    //    //    if (SceneManager.GetActiveScene().name != "MainScene")
    //    //    {
    //    //        //if (!DisconnectReason.HasTransitionReason)
    //    //        //{
    //    //        //    DisconnectReason.SetDisconnectReason(ConnectStatus.GenericDisconnect);
    //    //        //}

    //    //        SceneManager.LoadScene("MainScene");
    //    //    }
    //    //    else
    //    //    {
    //    //        //OnNetworkTimedOut?.Invoke();
    //    //    }
    //    //}
    //}

    #endregion

    public void StartClient()
    {
        var payload = JsonUtility.ToJson(new NetworkHostBroker.Payload {
            //clientGUID = Guid.NewGuid().ToString(),
            //clientScene = SceneManager.GetActiveScene().buildIndex,
            playerID = Guid.NewGuid().ToString(),
            playerName = PlayerPrefs.GetString("PlayerName", "Missing Name Too")
        });
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        Debug.Log("StartClient: " + payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartClient();

        //Debug.Log(NetworkManager.Singleton.IsClient + " " + NetworkManager.Singleton.IsHost + " " + NetworkManager.Singleton.IsServer);
    }

    public void StopClient()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("HomeScene");
        }
    }


        //private void HandleNetworkReadied()
        //{
        //    if (!NetworkManager.Singleton.IsClient) { return; }

        //    if (!NetworkManager.Singleton.IsHost)
        //    {
        //        //gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
        //    }
        //}

        //private void HandleUserDisconnectRequested()
        //{
        //    //DisconnectReason.SetDisconnectReason(ConnectStatus.UserRequestedDisconnect);
        //    NetworkManager.Singleton.Shutdown();

        //    HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

        //    SceneManager.LoadScene("Scene_Menu");
        //}

        //private void HandleConnectionFinished(ConnectionStatus status)
        //{
        //    if (status != ConnectionStatus.Connected)
        //    {
        //        //DisconnectReason.SetDisconnectReason(status);
        //    }

        //    OnConnectionFinished?.Invoke(status);
        //}

        //private void HandleDisconnectReasonReceived(ConnectionStatus status)
        //{
        //    //DisconnectReason.SetDisconnectReason(status);
        //}

        //private void HandleClientDisconnect(ulong clientId)
        //{
        //    if (!NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost)
        //    {
        //        //gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;

        //        if (SceneManager.GetActiveScene().name != "Scene_Menu")
        //        {
        //            //if (!DisconnectReason.HasTransitionReason)
        //            //{
        //            //    DisconnectReason.SetDisconnectReason(ConnectStatus.GenericDisconnect);
        //            //}

        //            SceneManager.LoadScene("Scene_Menu");
        //        }
        //        else
        //        {
        //            OnNetworkTimedOut?.Invoke();
        //        }
        //    }
        //}
    }

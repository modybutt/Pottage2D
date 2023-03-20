using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Collections;

[RequireComponent(typeof(NetworkManager))]
public class NetworkBroker : MonoBehaviour
{
    public static NetworkBroker Instance { get; private set; }

    //private GameManager gameManager;

    //public event Action OnNetworkReadied;

    //public event Action<ConnectionStatus> OnConnectionFinished;
    //public event Action<ConnectionStatus> OnDisconnectReasonReceived;

    //public event Action<ulong, int> OnClientSceneChanged;

    //public event Action OnUserDisconnectRequested;



    #region init => destroy

    private void Awake()
    {
        //Debug.Log("BrokerAwake: " + Instance);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        //NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void OnDestroy()
    {
        //Debug.Log("BrokerDestroy: " + Instance);

        //if (NetworkManager.Singleton != null)
        {
            //NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            //NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;

            //if (NetworkManager.Singleton.SceneManager != null)
            //{
            //    NetworkManager.Singleton.SceneManager.OnSceneEvent -= HandleSceneEvent;
            //}

            //if (NetworkManager.Singleton.CustomMessagingManager == null) return;

            //UnregisterClientMessageHandlers();
        }

        if (Instance == this) Instance = null;
    }

    //private void HandleServerStarted()
    //{
    //    Debug.Log("HandleServerStarted");

    //    if (NetworkManager.Singleton.IsHost)
    //    {
    //        //OnConnectionFinished?.Invoke(ConnectionStatus.Connected);
    //    }

    //    //OnNetworkReadied?.Invoke();
    //}

    //private void HandleClientConnected(ulong clientId)
    //{
    //    Debug.Log("HandleClientConnected " + clientId);

    //    if (clientId != NetworkManager.Singleton.LocalClientId) { return; }

    //    HandleServerStarted();
    //    //NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleSceneEvent;
    //}

    #endregion

    public void Disconnect()
    {
        /* ServerGameNetPortal#HandleUserDisconnectRequest
         */
        //HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);
        NetworkManager.Singleton.Shutdown();
        //ClearData();
        
        /* ClientGameNetPortal#HandleUserDisconnectRequest
         */
        //DisconnectReason.SetDisconnectReason(ConnectStatus.UserRequestedDisconnect);
        //HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);
        SceneManager.LoadScene("HomeScene");
    }





    //public void StartHost()
    //{
    //    NetworkManager.Singleton.StartHost();

    //    //RegisterClientMessageHandlers();
    //}

    //public void RequestDisconnect()
    //{
    //    OnUserDisconnectRequested?.Invoke();
    //}

    //#region Message Handlers

    //private void RegisterClientMessageHandlers()
    //{
    //    NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientConnectResult", (senderClientId, reader) =>
    //    {
    //        reader.ReadValueSafe(out ConnectionStatus status);
    //        OnConnectionFinished?.Invoke(status);
    //    });

    //    NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientSetDisconnectReason", (senderClientId, reader) =>
    //    {
    //        reader.ReadValueSafe(out ConnectionStatus status);
    //        OnDisconnectReasonReceived?.Invoke(status);
    //    });
    //}

    //private void UnregisterClientMessageHandlers()
    //{
    //    NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientConnectResult");
    //    NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientSetDisconnectReason");
    //}

    //#endregion

    //#region Message Senders

    //public void ServerToClientConnectResult(ulong netId, ConnectionStatus status)
    //{
    //    var writer = new FastBufferWriter(sizeof(ConnectionStatus), Allocator.Temp);
    //    writer.WriteValueSafe(status);
    //    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ServerToClientConnectResult", netId, writer);
    //}

    //public void ServerToClientSetDisconnectReason(ulong netId, ConnectionStatus status)
    //{
    //    var writer = new FastBufferWriter(sizeof(ConnectionStatus), Allocator.Temp);
    //    writer.WriteValueSafe(status);
    //    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ServerToClientSetDisconnectReason", netId, writer);
    //}

    //#endregion
}

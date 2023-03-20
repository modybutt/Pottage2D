using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Collections;
using System.Text;
using static Unity.Netcode.NetworkManager;

[RequireComponent(typeof(NetworkBroker))]
public class NetworkHostBroker : MonoBehaviour
{
    public static NetworkHostBroker Instance { get; private set; }

    [SerializeField] private int MaxConnectionPayload = 1024;

    private Dictionary<string, NetworkPlayer> players;
    private Dictionary<ulong, string> playersIdToGuid;

    [Serializable]
    public class Payload
    {
        public string playerID;
        public string playerName;
    }

    #region init => destroy

    private void Awake()
    {
        //Debug.Log("HostAwake: " + Instance);
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
        players = new Dictionary<string, NetworkPlayer>();
        playersIdToGuid = new Dictionary<ulong, string>();
        //clientSceneMap = new Dictionary<ulong, int>();

        //gameNetPortal.OnNetworkReadied += HandleNetworkReadied;
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        //NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnect;
        //NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
    }

    private void OnDestroy()
    {
        //Debug.Log("HostDestroy: " + Instance);
        //if (gameNetPortal == null) { return; }

        //gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;

        if (NetworkManager.Singleton == null) return;

        //NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnect;
        //NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;

        NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;

        if (Instance == this) Instance = null;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {
        //Debug.Log("ApprovalCheck: " + req);

        if (req.Payload.Length > MaxConnectionPayload)
        {
            Debug.Log("approval: " + req.Payload.Length + ">" + MaxConnectionPayload);
            //callback(false, 0, false, null, null);
            return;
        }

        if (req.ClientNetworkId == NetworkManager.Singleton.LocalClientId)
        {
            //Debug.Log("approval: " + req.ClientNetworkId +"=="+ NetworkManager.Singleton.LocalClientId);
            res.Approved = true;
            //callback(false, null, true, null, null);
            return;
        }

        string payload = Encoding.UTF8.GetString(req.Payload);
        var connectionPayload = JsonUtility.FromJson<Payload>(payload);
        //Debug.Log("approval-p: " + connectionPayload);

        NetworkClientBroker.Status clientStatus = NetworkClientBroker.Status.Connected;

        //    // This stops us from running multiple standalone builds since 
        //    // they disconnect eachother when trying to join
        //    //
        //    // if (clientData.ContainsKey(connectionPayload.clientGUID))
        //    // {
        //    //     ulong oldClientId = clientData[connectionPayload.clientGUID].ClientId;
        //    //     StartCoroutine(WaitToDisconnectClient(oldClientId, ConnectStatus.LoggedInAgain));
        //    // }

        //    if (gameInProgress)
        //    {
        //        gameReturnStatus = ConnectionStatus.GameInProgress;
        //    }
        //    else if (clientData.Count >= maxPlayers)
        //    {
        //        gameReturnStatus = ConnectionStatus.ServerFull;
        //    }

        if (clientStatus == NetworkClientBroker.Status.Connected)
        {
            Debug.Log("approval: " + connectionPayload.playerID + "@" + connectionPayload.playerName);

            //clientSceneMap[clientId] = connectionPayload.clientScene;
            players[connectionPayload.playerID] = new NetworkPlayer(req.ClientNetworkId, connectionPayload.playerName);
            playersIdToGuid[req.ClientNetworkId] = connectionPayload.playerID;
            res.Approved = true;
        }

        //    //callback(false, 0, true, null, null);

        //    //gameNetPortal.ServerToClientConnectResult(clientId, gameReturnStatus);

        //    if (gameReturnStatus != ConnectionStatus.Connected)
        //    {
        //        StartCoroutine(WaitToDisconnectClient(clientId, gameReturnStatus));
        //    }
    }

    private void HandleServerStarted()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            //Debug.Log(this + " isServer");
            NetworkManager.Singleton.SceneManager.LoadScene("GameLobby", LoadSceneMode.Single);
        }

        if (NetworkManager.Singleton.IsHost)
        {
            //Debug.Log(this + " isHost");
            string playerGuid = Guid.NewGuid().ToString();
            string playerName = PlayerPrefs.GetString("PlayerName", "Missing Name");

            Debug.Log("addPlayer " + playerGuid +","+ NetworkManager.Singleton.LocalClientId + "," + playerName);
            players[playerGuid] = new NetworkPlayer(NetworkManager.Singleton.LocalClientId, playerName);
            playersIdToGuid[NetworkManager.Singleton.LocalClientId]  = playerGuid;
        }
    }

    //private void HandleClientConnect(ulong clientId)
    //{
    //    if (NetworkManager.Singleton.IsHost == false) return;

    //    Debug.Log("HandleClientConnect " + clientId);
    //}

    //private void HandleClientDisconnect(ulong clientId)
    //{
    //    if (NetworkManager.Singleton.IsHost == false) return;

    //    Debug.Log("HandleClientDisconnect " + clientId);
    //}

    #endregion

    #region Message Handlers

    //private void RegisterClientMessageHandlers()
    //{
    //    NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientConnectResult", (senderClientId, reader) =>
    //    {
    //        reader.ReadValueSafe(out ConnectionStatus status);
    //        //OnConnectionFinished?.Invoke(status);
    //    });

    //    NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientSetDisconnectReason", (senderClientId, reader) =>
    //    {
    //        reader.ReadValueSafe(out ConnectionStatus status);
    //        //OnDisconnectReasonReceived?.Invoke(status);
    //    });
    //}

    private void UnregisterClientMessageHandlers()
    {
        NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientConnectResult");
        NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientSetDisconnectReason");
    }

    #endregion

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        //Debug.Log(NetworkManager.Singleton.IsClient + " " + NetworkManager.Singleton.IsHost + " " + NetworkManager.Singleton.IsServer);

        //RegisterClientMessageHandlers();
    }

    public void StopHost()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("HomeScene");
        }
    }

    public NetworkPlayer? GetNetworkPlayerById(ulong playerId)
    {
        if (playersIdToGuid.TryGetValue(playerId, out string playerGuid))
        {
            //Debug.Log(playersIdToGuid);
            if (players.TryGetValue(playerGuid, out NetworkPlayer player))
            {
                return player;
            }
        }

        return null;
    }








    //private void HandleNetworkReadied()
    //{
    //    if (NetworkManager.Singleton.IsServer == false) return;

    //    ////gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
    //    //NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    //    ////gameNetPortal.OnClientSceneChanged += HandleClientSceneChanged;

    //    //NetworkManager.Singleton.SceneManager.LoadScene("Scene_Lobby", LoadSceneMode.Single);

    //    //if (NetworkManager.Singleton.IsHost)
    //    //{
    //    //    clientSceneMap[NetworkManager.Singleton.LocalClientId] = SceneManager.GetActiveScene().buildIndex;
    //    //}
    //}






    //[Header("Settings")]
    //[SerializeField] private int maxPlayers = 4;

    //public static ServerGameNetPortal Instance => instance;
    //private static ServerGameNetPortal instance;

    //private Dictionary<string, PlayerData> clientData;
    //private Dictionary<ulong, string> clientIdToGuid;
    //private Dictionary<ulong, int> clientSceneMap;
    //private bool gameInProgress;

    //private const int MaxConnectionPayload = 1024;

    //private GameNetPortal gameNetPortal;

    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    //private void Start()
    //{
    //    //gameNetPortal = GetComponent<GameNetPortal>();
    //    //gameNetPortal.OnNetworkReadied += HandleNetworkReadied;

    //    //NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    //    NetworkManager.Singleton.OnServerStarted += HandleServerStarted;

    //    clientData = new Dictionary<string, PlayerData>();
    //    clientIdToGuid = new Dictionary<ulong, string>();
    //    clientSceneMap = new Dictionary<ulong, int>();
    //}

    //private void OnDestroy()
    //{
    //    //if (gameNetPortal == null) { return; }

    //    //gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;

    //    if (NetworkManager.Singleton == null) { return; }

    //    //NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
    //    NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
    //}

    //public PlayerData? GetPlayerData(ulong clientId)
    //{
    //    if (clientIdToGuid.TryGetValue(clientId, out string clientGuid))
    //    {
    //        if (clientData.TryGetValue(clientGuid, out PlayerData playerData))
    //        {
    //            return playerData;
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"No player data found for client id: {clientId}");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"No client guid found for client id: {clientId}");
    //    }

    //    return null;
    //}

    //public void StartGame()
    //{
    //    gameInProgress = true;

    //    NetworkManager.Singleton.SceneManager.LoadScene("Scene_Main", LoadSceneMode.Single);
    //}

    //public void EndRound()
    //{
    //    gameInProgress = false;

    //    NetworkManager.Singleton.SceneManager.LoadScene("Scene_Lobby", LoadSceneMode.Single);
    //}

    //private void HandleNetworkReadied()
    //{
    //    if (!NetworkManager.Singleton.IsServer) { return; }

    //    //gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
    //    NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    //    //gameNetPortal.OnClientSceneChanged += HandleClientSceneChanged;

    //    NetworkManager.Singleton.SceneManager.LoadScene("Scene_Lobby", LoadSceneMode.Single);

    //    if (NetworkManager.Singleton.IsHost)
    //    {
    //        clientSceneMap[NetworkManager.Singleton.LocalClientId] = SceneManager.GetActiveScene().buildIndex;
    //    }
    //}

    //private void HandleClientDisconnect(ulong clientId)
    //{
    //    clientSceneMap.Remove(clientId);

    //    if (clientIdToGuid.TryGetValue(clientId, out string guid))
    //    {
    //        clientIdToGuid.Remove(clientId);

    //        //if (clientData[guid].ClientId == clientId)
    //        {
    //            clientData.Remove(guid);
    //        }
    //    }

    //    if (clientId == NetworkManager.Singleton.LocalClientId)
    //    {
    //        //gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;
    //        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    //        //gameNetPortal.OnClientSceneChanged -= HandleClientSceneChanged;
    //    }
    //}

    //private void HandleClientSceneChanged(ulong clientId, int sceneIndex)
    //{
    //    clientSceneMap[clientId] = sceneIndex;
    //}

    //private void HandleUserDisconnectRequested()
    //{
    //    HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

    //    NetworkManager.Singleton.Shutdown();

    //    ClearData();

    //    SceneManager.LoadScene("Scene_Menu");
    //}

    //private void HandleServerStarted()
    //{
    //    if (!NetworkManager.Singleton.IsHost) { return; }

    //    string clientGuid = Guid.NewGuid().ToString();
    //    string playerName = PlayerPrefs.GetString("PlayerName", "Missing Name");

    //    //clientData.Add(clientGuid, new PlayerData(playerName, NetworkManager.Singleton.LocalClientId));
    //    clientIdToGuid.Add(NetworkManager.Singleton.LocalClientId, clientGuid);
    //}

    //private void ClearData()
    //{
    //    clientData.Clear();
    //    clientIdToGuid.Clear();
    //    clientSceneMap.Clear();

    //    gameInProgress = false;
    //}

    //private void ApprovalCheck(byte[] connectionData, ulong clientId)//, NetworkManager.ConnectionApprovedDelegate callback)
    //{
    //    if (connectionData.Length > MaxConnectionPayload)
    //    {
    //        //callback(false, 0, false, null, null);
    //        return;
    //    }

    //    if (clientId == NetworkManager.Singleton.LocalClientId)
    //    {
    //        //callback(false, null, true, null, null);
    //        return;
    //    }

    //    string payload = Encoding.UTF8.GetString(connectionData);
    //    var connectionPayload = 0;//JsonUtility.FromJson<ConnectionPayload>(payload);

    //    ConnectionStatus gameReturnStatus = ConnectionStatus.Connected;

    //    // This stops us from running multiple standalone builds since 
    //    // they disconnect eachother when trying to join
    //    //
    //    // if (clientData.ContainsKey(connectionPayload.clientGUID))
    //    // {
    //    //     ulong oldClientId = clientData[connectionPayload.clientGUID].ClientId;
    //    //     StartCoroutine(WaitToDisconnectClient(oldClientId, ConnectStatus.LoggedInAgain));
    //    // }

    //    if (gameInProgress)
    //    {
    //        gameReturnStatus = ConnectionStatus.GameInProgress;
    //    }
    //    else if (clientData.Count >= maxPlayers)
    //    {
    //        gameReturnStatus = ConnectionStatus.ServerFull;
    //    }

    //    if (gameReturnStatus == ConnectionStatus.Connected)
    //    {
    //        //clientSceneMap[clientId] = connectionPayload.clientScene;
    //        //clientIdToGuid[clientId] = connectionPayload.clientGUID;
    //        //clientData[connectionPayload.clientGUID] = new PlayerData(connectionPayload.playerName, clientId);
    //    }

    //    //callback(false, 0, true, null, null);

    //    //gameNetPortal.ServerToClientConnectResult(clientId, gameReturnStatus);

    //    if (gameReturnStatus != ConnectionStatus.Connected)
    //    {
    //        StartCoroutine(WaitToDisconnectClient(clientId, gameReturnStatus));
    //    }
    //}

    //private IEnumerator WaitToDisconnectClient(ulong clientId, ConnectionStatus reason)
    //{
    //    //gameNetPortal.ServerToClientSetDisconnectReason(clientId, reason);

    //    yield return new WaitForSeconds(0);

    //    KickClient(clientId);
    //}

    //private void KickClient(ulong clientId)
    //{
    //    NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
    //    if (networkObject != null)
    //    {
    //        networkObject.Despawn(true);
    //    }

    //    NetworkManager.Singleton.DisconnectClient(clientId);
    //}
}

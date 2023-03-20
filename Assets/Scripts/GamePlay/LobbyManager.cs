using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using System;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    private Dictionary<string, PlayerData> players;
    private Dictionary<ulong, string> playerIdToGuid;
    private Dictionary<ulong, int> playerSceneMap;
    //private bool gameInProgress;




    public PlayerBoard prefabPlayerBoard;
    //private List<PlayerBoardData> players;
    //private List<Card> cards;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //gameNetPortal = GetComponent<GameNetPortal>();
        //gameNetPortal.OnNetworkReadied += HandleNetworkReadied;

        //NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;

        players = new Dictionary<string, PlayerData>();
        playerIdToGuid = new Dictionary<ulong, string>();
        playerSceneMap = new Dictionary<ulong, int>();
    }

    private void OnDestroy()
    {

        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
    }

    private void HandleServerStarted()
    {
        if (NetworkManager.Singleton.IsHost == false) return;

        string playerGuid = Guid.NewGuid().ToString();
        string playerName = PlayerPrefs.GetString("PlayerName", "Missing Name");

        players.Add(playerGuid, new PlayerData(NetworkManager.Singleton.LocalClientId, playerName));
        playerIdToGuid.Add(NetworkManager.Singleton.LocalClientId, playerGuid);
    }

    private void HandlePlayerDisconnect(ulong playerId)
    {
        playerSceneMap.Remove(playerId);

        if (playerIdToGuid.TryGetValue(playerId, out string playerGuid))
        {
            playerIdToGuid.Remove(playerId);

            if (players[playerGuid].PlayerId == playerId)
            {
                players.Remove(playerGuid);
            }
        }

        if (playerId == NetworkManager.Singleton.LocalClientId)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandlePlayerDisconnect;
        }
    }

    public PlayerData? GetPlayerById(ulong playerId)
    {
        if (playerIdToGuid.TryGetValue(playerId, out string playerGuid))
        {
            if (players.TryGetValue(playerGuid, out PlayerData player))
            {
                return player;
            }
        }

        return null;
    }





    public void StartLobby()
    {
        NetworkManager.Singleton.StartHost();
        RegisterClientMsgHandlers();
    }

    public void StopLobby()
    {
        //NetworkManager.Singleton.Stop
        //RegisterClientMsgHandlers();
    }

    private void RegisterClientMsgHandlers()
    {
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientConnectResult", (playerId, reader) =>
        {
            //reader.ReadValueSafe(out ConnectStatus status);
            //OnConnectionFinished?.Invoke(status);
        });

        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ServerToClientDisconnectReason", (playerId, reader) =>
        {
            //reader.ReadValueSafe(out ConnectStatus status);
            //OnDisconnectReasonReceived?.Invoke(status);
        });
    }

    private void UnRegisterClientMsgHandlers()
    {
        NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientConnectResult");
        NetworkManager.Singleton.CustomMessagingManager.UnregisterNamedMessageHandler("ServerToClientDisconnectReason");
    }

    //public void ServerToClientConnectResult(ulong playerId, ConnectionStatus status)
    //{
    //    var writer = new FastBufferWriter(sizeof(ConnectionStatus), Unity.Collections.Allocator.Temp);
    //    writer.WriteValueSafe(status);
    //    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ServerToClientConnectResult", playerId, writer);
    //}

    //public void ServerToClientDisconnectResult(ulong playerId, ConnectionStatus status)
    //{
    //    var writer = new FastBufferWriter(sizeof(ConnectionStatus), Unity.Collections.Allocator.Temp);
    //    writer.WriteValueSafe(status);
    //    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ServerToClientDisconnectResult", playerId, writer);
    //}






    public void StartGame()
    {
        //gameInProgress = true;

        NetworkManager.Singleton.SceneManager.LoadScene("MainScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void StopGame()
    {
        //gameInProgress = false;

        NetworkManager.Singleton.SceneManager.LoadScene("MainLobby", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }





    //public void CreatePlayer(string playerName)
    //{
    //    PlayerBoardData newPlayer = ScriptableObject.CreateInstance<PlayerBoardData>();

    //    newPlayer.playerName = playerName;
    //    newPlayer.background = null;

    //    players.Add(newPlayer);

    //    PlayerBoard board = Instantiate<PlayerBoard>(prefabPlayerBoard);
    //    board.playerBoardData = newPlayer;


    //    // link in/out stack newPlayer=>player[0]/players[Lenth-1]
    //}

    //public void QuitGame()
    //{
    //    Application.Quit();
    //}    //public void CreatePlayer(string playerName)
    //{
    //    PlayerBoardData newPlayer = ScriptableObject.CreateInstance<PlayerBoardData>();

    //    newPlayer.playerName = playerName;
    //    newPlayer.background = null;

    //    players.Add(newPlayer);

    //    PlayerBoard board = Instantiate<PlayerBoard>(prefabPlayerBoard);
    //    board.playerBoardData = newPlayer;


    //    // link in/out stack newPlayer=>player[0]/players[Lenth-1]
    //}

    //public void QuitGame()
    //{
    //    Application.Quit();
    //}
}

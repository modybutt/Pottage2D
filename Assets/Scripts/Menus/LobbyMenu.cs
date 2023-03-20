using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(NetworkObject))]
public class LobbyMenu : NetworkBehaviour
{
    //[Header("Foobar")]
    //[SerializeField] private PlayerData[] players;  // FXIME LobbyPlayerUI
    [SerializeField] private int minPlayerCount = 2;
    //[Header("yoink")]
    [SerializeField] private GameObject playerList;
    [SerializeField] private LobbyPlayerSlot prefabPlayerSlot;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button playerReadyButton;

    private NetworkList<LobbyPlayerState> lobbyPlayerStates;
    private Dictionary<ulong, LobbyPlayerSlot> playersIdToSlot;

    #region init => destroy

    private void Awake()
    {
        lobbyPlayerStates = new NetworkList<LobbyPlayerState>();
        playersIdToSlot = new Dictionary<ulong, LobbyPlayerSlot>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            lobbyPlayerStates.OnListChanged += HandleLobbyPlayerStateChanged;

            foreach (LobbyPlayerState player in lobbyPlayerStates)
            {
                HandleLobbyPlayerStateChanged(new NetworkListEvent<LobbyPlayerState>
                {
                    Type = NetworkListEvent<LobbyPlayerState>.EventType.Add,
                    Value = player
                });
            }
        }

        if (IsServer)
        {
            //startGameButton.gameObject.SetActive(true);

            NetworkManager.Singleton.OnClientConnectedCallback += HandlePlayerConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandlePlayerDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandlePlayerConnected(client.ClientId);
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        lobbyPlayerStates.OnListChanged -= HandleLobbyPlayerStateChanged;

        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandlePlayerConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandlePlayerDisconnected;
    }

    private void HandlePlayerConnected(ulong playerId)
    {
        //Debug.Log("HandlePlayerConnected " + playerId);
        var player = NetworkHostBroker.Instance.GetNetworkPlayerById(playerId);
        if (!player.HasValue) return;

        lobbyPlayerStates.Add(new LobbyPlayerState(playerId, player.Value.playerName, false));
    }

    private void HandlePlayerDisconnected(ulong playerId)
    {   
        for (int i = 0; i < lobbyPlayerStates.Count; i++)
        {
            if (lobbyPlayerStates[i].PlayerId == playerId)
            {
                lobbyPlayerStates.Remove(lobbyPlayerStates[i]);
                break;
            }
        }
    }

    private void HandleLobbyPlayerStateChanged(NetworkListEvent<LobbyPlayerState> changeEvent)
    {
        //Debug.Log("HandleLobbyPlayerStateChanged " + changeEvent.Type + ": " + changeEvent.Value.PlayerId + "@" + changeEvent.Value.PlayerName);
        //Debug.Log("+HandleLobbyPlayerStateChanged " + NetworkHostBroker.Instance.GetNetworkPlayerById(changeEvent.Value.PlayerId));

        switch (changeEvent.Type)
        {
            //case NetworkListEvent<LobbyPlayerState>.EventType.Insert:
            case NetworkListEvent<LobbyPlayerState>.EventType.Add:
            {
                    if (playersIdToSlot.TryGetValue(changeEvent.Value.PlayerId, out LobbyPlayerSlot slot))
                    {
                        Debug.Log("existing slot");
                        break; // TODO existing slot with ID?
                    }

                    playersIdToSlot[changeEvent.Value.PlayerId] = Instantiate(prefabPlayerSlot, playerList.transform);
                    playersIdToSlot[changeEvent.Value.PlayerId].UpdateView(changeEvent.Value);
                    break;
            }
            //ase NetworkListEvent<LobbyPlayerState>.EventType.RemoveAt:
            case NetworkListEvent<LobbyPlayerState>.EventType.Remove:
            {
                    //Debug.Log(changeEvent.Value.PlayerId + " " + changeEvent.Value.PlayerName);
                    if (playersIdToSlot.TryGetValue(changeEvent.Value.PlayerId, out LobbyPlayerSlot slot))
                    {
                        playersIdToSlot.Remove(changeEvent.Value.PlayerId);
                        Destroy(slot.gameObject);
                    }

                    break;
            }
            case NetworkListEvent<LobbyPlayerState>.EventType.Value:
            {
                    if (playersIdToSlot.TryGetValue(changeEvent.Value.PlayerId, out LobbyPlayerSlot slot))
                    {
                        slot.UpdateView(changeEvent.Value);
                    }

                    break;
            }
        }

        if (IsHost)
        {
            //startGameButton.interactable = PreparedAndReady();
            bool readyToStart = PreparedAndReady();
            startGameButton.gameObject.SetActive(readyToStart);
            playerReadyButton.gameObject.SetActive(!readyToStart);
        }
    }

    private bool PreparedAndReady()
    {
        if (lobbyPlayerStates.Count < minPlayerCount)
        {
            return false;
        }

        foreach (LobbyPlayerState playerState in lobbyPlayerStates)
        {
            if (!playerState.IsReady) return false;
        }

        return true;
    }

    #endregion



    public void OnLeaveClicked()
    {
        NetworkClientBroker.Instance.StopClient();
        NetworkHostBroker.Instance.StopHost();
    }

    public void OnReadyClicked()
    {
        ToggleReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < lobbyPlayerStates.Count; i++)
        {
            if (lobbyPlayerStates[i].PlayerId == serverRpcParams.Receive.SenderClientId)
            {
                lobbyPlayerStates[i] = new LobbyPlayerState(
                    lobbyPlayerStates[i].PlayerId,
                    lobbyPlayerStates[i].PlayerName,
                    !lobbyPlayerStates[i].IsReady
                );

                break;
            }
        }
    }

    public void OnStartClicked()
    {
        GameManager.Instance.StartGame();
    }


    //[ServerRpc(RequireOwnership = false)]
    //private void TogglePlayerReadyRpc(ServerRpcReceiveParams serverRpcParams = default)
    //{
    //    for (int i = 0; i < lobbyPlayerStates.Count; i++)
    //    {
    //        //if (lobbyPlayerStates[i].PlayerId == serverRpcParams.Receive.SenderClientId)
    //        {
    //            lobbyPlayerStates[i] = new LobbyPlayerState(
    //                lobbyPlayerStates[i].PlayerId,
    //                lobbyPlayerStates[i].PlayernName,
    //                !lobbyPlayerStates[i].IsReady);
    //        }
    //    }
    //}

    //[ServerRpc(RequireOwnership = false)]
    //private void StartGameServerRpc(ServerRpcSendParams serverRpcParams = default)
    //{
    //    //if (serverRpcParams.Receive.SenderClientId != NetworkManager.Singleton.LocalClientId) return;
    //    if (AreAllReady() == false) return;

    //    GameManager.Instance.StartGame();
    //}


}

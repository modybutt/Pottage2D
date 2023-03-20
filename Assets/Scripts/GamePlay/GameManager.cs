using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(NetworkHostBroker))]
//[RequireComponent(typeof(NetworkClientBroker))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;  }


    private int currRound;   // list of rounds

    private static int counter;
    private int count;

    private void Awake()
    {
        //count = counter++;
        //Debug.Log("GameManagerAwake: " + count);

        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            //return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        //Debug.Log("GameManagerDestroy: " + count);
        if (Instance == this) Instance = null;
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    public void StopGame()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameResult", LoadSceneMode.Single);
            //NetworkManager.Singleton.SceneManager.LoadScene("GameLobby", LoadSceneMode.Single);
        }
    }








    private void SpawnPlayerCards()
    {
        // from all cards
        // create 5 cards per 1 player
        // complete vegs, actions, 6 events
    }

    private void GiveCards()
    {
        // foreach card in rootstack
        // if (player.cards < 5) give card player++
    }

    private void NextEvent()
    {
        // events++
    }


    private void OnRoundStart()
    {
        GiveCards();
    }

    private void OnRoundFinish()
    {
        // 5 cycles of hand cards has been passed

        if (IsGameFinished()) StopGame();
    }

    private bool IsGameFinished()
    {
        // 3 rounds have been played

        return false;
    }

    private void CalculatePlayerPoints()
    {

    }
}

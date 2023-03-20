using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.Netcode;

//using Unity.Netcode;

public class ConnectionManager : MonoBehaviour
{

    //public void OnPlayerConnected(NetworkPlayer player)
    //{

    //}

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        //NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}

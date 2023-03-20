using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using Unity.NetCode;
using UnityEngine.SceneManagement;

public class MainMenuDisplay : MonoBehaviour
{
    //[SerializeField] private string gameplaySceneName = "Gameplay";

    public void StartHost()
    {
        //NetworkManager.Singleton.StartHost();
    }
    public void StartServer()
    {
        //NetworkManager.Singleton.StartServer();
        //NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

    public void StartClient()
    {
        //NetworkManager.Singleton.StartClient();
    }
}

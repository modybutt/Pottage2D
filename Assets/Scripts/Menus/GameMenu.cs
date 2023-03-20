using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void OnExitLobbyClicked()
    {
        GameManager.Instance.StopGame();
    }
}

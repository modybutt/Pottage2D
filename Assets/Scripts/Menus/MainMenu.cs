using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] private TMP_InputField nameInputField;

    private void Start()
    {
        //PlayerPrefs.GetString("PlayerName");
    }

    public void OnCreateClicked()
    {
        //Debug.Log("OnCreateClicked");
        //PlayerPrefs.SetString("PlayerName", nameInputField.text);

        NetworkHostBroker.Instance.StartHost();
    }

    public void OnJoinClicked()
    {
        //Debug.Log("OnJoinClicked");
        //PlayerPrefs.SetString("PlayerName", nameInputField.text);

        NetworkClientBroker.Instance.StartClient();
    }

    public void OnLocalClicked()
    {
        //Debug.Log("OnLocalClicked");
        //PlayerPrefs.SetString("PlayerName", nameInputField.text);
        //GameManager.Instance.StartLobby(bool local);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}

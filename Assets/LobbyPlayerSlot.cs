using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Image playerProfileIcon;
    [SerializeField] private Toggle playerReadyToggle;

    public void UpdateView(LobbyPlayerState playerState)
    {
        playerNameText.text = playerState.PlayerName.ToString();
        //playerProfileIcon = playerState.PlayerId;
        playerReadyToggle.gameObject.SetActive(playerState.IsReady);
    }
}

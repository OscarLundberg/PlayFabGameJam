using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMethods : MonoBehaviour
{
    public LobbyHandler lh;
    public void UpdateLobby(string param)
    {
        lh.SetEvents(JsonUtility.FromJson<Lobby>(param));
    }

    public void StartGame()
    {
        Debug.Log("the game has started");
    }

    public void ProgressGame()
    {

    }

    public void Robbed()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMethods : MonoBehaviour
{

    public LobbyHandler lh;
    public void lobby_poll(string param)
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
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "You have been robbed... ", "", "Private"));
    }

    public void Convicted()
    {
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "You have been convicted... ", "", "Private"));
    }

    public void Night()
    {
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "Night falls... ", "", "Private"));
    }

    public void Day()
    {
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "A new day breaks... ", "", "Private"));
    }
}

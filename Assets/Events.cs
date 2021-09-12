using UnityEngine;

[System.Serializable]
public class MessagePayload
{
    public MessagePayload(string lobby, string sender, string message)
    {
        this.LobbyID = lobby;
        this.sender = sender;
        this.message = message;
    }

    [SerializeField]
    public string LobbyID;


    [SerializeField]
    public string sender;

    [SerializeField]
    public string message;

}
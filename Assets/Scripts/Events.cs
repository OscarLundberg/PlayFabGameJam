using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessagePayload
{
    public MessagePayload()
    {

    }
    public MessagePayload(string lobby, string sender, string message)
    {
        this.LobbyID = lobby;
        this.sender = sender;
        this.message = message;
        this.timestring = System.DateTime.UtcNow.ToString();
    }

    public MessagePayload(string lobby, string sender, string message, string param, string type = "System")
    {
        this.LobbyID = lobby;
        this.sender = sender;
        this.message = message;
        this.timestring = System.DateTime.UtcNow.ToString();
        this.type = type;
        this.param = param;
    }

    [SerializeField]
    public string LobbyID;


    [SerializeField]
    public string sender;

    [SerializeField]
    public string message;

    [SerializeField]
    public string param;

    [SerializeField]
    public string timestring;

    [SerializeField]
    public string type;

    public System.DateTime timestamp
    {
        get
        {
            return System.DateTime.Parse(timestring);
        }
    }


}

public class CompareMessages : IComparer<MessagePayload>
{
    int IComparer<MessagePayload>.Compare(MessagePayload x, MessagePayload y)
    {
        return x.timestamp.CompareTo(y.timestamp);
    }
}

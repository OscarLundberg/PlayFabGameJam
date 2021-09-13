using UnityEngine;
using System.Collections;
using System.Collections.Generic;




[System.Serializable]
public class Poll
{
    [SerializeField]
    public string Lobby;
    [SerializeField]
    public string user;
    public Poll(string Lobby, string user)
    {
        this.Lobby = Lobby;
        this.user = user;
    }

}


[System.Serializable]
public class GenericBooleanResponse
{
    [SerializeField]
    public bool success;
}


[System.Serializable]
public class ListLobbyEventsResponse
{
    public ListLobbyEventsResponse()
    {

    }

    [SerializeField]
    public List<MessagePayload> events;
}
// [System.Serializable]
// public class Event
// {
//     public Event(string Lobby, MessagePayload Payload)
//     {
//         this.Payload = Payload;
//     }
//     [SerializeField]
//     public string Lobby;
//     [SerializeField]
//     public MessagePayload Payload;
// }
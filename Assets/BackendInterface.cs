using UnityEngine;
using System.Collections;
using System.Collections.Generic;




[System.Serializable]
public class Poll
{
    [SerializeField]
    public string Lobby;
    public Poll(string Lobby)
    {
        this.Lobby = Lobby;
    }

}



[System.Serializable]
public class ListLobbyEventsResponse
{
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
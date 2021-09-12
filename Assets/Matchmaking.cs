using UnityEngine;

public class Matchmaking
{

}



[System.Serializable]
public class Lobby
{
    [SerializeField]
    public string id;

    [SerializeField]
    public string name;

    [SerializeField]
    public List<string> users;

    [SerializeField]
    public bool isJoinable;

}

[System.Serializable]
public class JoinLobbyRequest
{
    [SerializeField]
    public string Lobby;

    [SerializeField]
    public string user;

}


[System.Serializable]
public class CreateLobbyRequest
{
    [SerializeField]
    public string Lobby;

    [SerializeField]
    public string user;

}
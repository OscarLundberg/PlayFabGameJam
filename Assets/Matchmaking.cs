using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EventsModels;
using PlayFab.Events;
using PlayFab.MultiplayerModels;
public class Matchmaking : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public MainMenu mm;
    public void Search(MainMenu menu)
    {
        this.mm = menu;
        // find games
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "get_lobbies";

        PlayFabClientAPI.ExecuteCloudScript<GetLobbiesResponse>(req, (ExecuteCloudScriptResult res) =>
        {
            UpdateList(res.FunctionResult as GetLobbiesResponse);
        }, mm.DefaultError);
    }

    public void UpdateList(GetLobbiesResponse response)
    {
        if (response.lobbies.Count <= 0)
        {
            response.lobbies.Add(new Lobby("No active lobbies - Create one below", false));
        }
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        foreach (Lobby lobby in response.lobbies)
        {
            var go = (Instantiate(prefab, parent) as GameObject).GetComponent<LobbyData>();
            go.SetData(lobby, this);
        }
    }

    public void CreateLobby(TMPro.TMP_InputField inp)
    {
        if (inp.text.Length <= 4)
        {
            mm.Error("Please enter a longer name");
        }
        else
        {
            var req = new ExecuteCloudScriptRequest();
            req.FunctionName = "create_lobby";
            req.FunctionParameter = new CreateLobbyRequest();
            PlayFabClientAPI.ExecuteCloudScript<GetLobbiesResponse>(req, (ExecuteCloudScriptResult res) =>
            {
                UpdateList(res.FunctionResult as GetLobbiesResponse);
            }, mm.DefaultError);
        }
    }

    public void TryJoin(string id)
    {

    }

}



[System.Serializable]
public class Lobby
{
    public Lobby(string name, bool joinable = true)
    {
        this.name = name;
        this.id = System.Guid.NewGuid().ToString();
        this.isJoinable = joinable;
    }
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
    public CreateLobbyRequest(string name)
    {
        this.Payload = new Lobby(name);
    }
    [SerializeField]
    public Lobby Payload;
}

[System.Serializable]
public class GetLobbiesResponse
{
    [SerializeField]
    public List<Lobby> lobbies;
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EventsModels;
using PlayFab.Events;
using PlayFab.MultiplayerModels;
using UnityEngine.Events;

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
        var tempRes = new GetLobbiesResponse();
        tempRes.lobbies = new List<Lobby>() { new Lobby("Searching...", false) };
        UpdateList(tempRes);
        PlayFabClientAPI.ExecuteCloudScript<GetLobbiesResponse>(req, (ExecuteCloudScriptResult res) =>
        {
            StartCoroutine(AfterDelay(2, () =>
            {
                UpdateList(res.FunctionResult as GetLobbiesResponse);
            }));
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
            go.UpdateText();
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
            var lobbyReq = new CreateLobbyRequest(inp.text);
            var req = new ExecuteCloudScriptRequest();
            req.FunctionName = "create_lobby";
            req.FunctionParameter = lobbyReq;
            PlayFabClientAPI.ExecuteCloudScript<GenericBooleanResponse>(req, (ExecuteCloudScriptResult res) =>
            {
                Debug.Log(res.ToJson());
                if ((res.FunctionResult as GenericBooleanResponse).success)
                {
                    TryJoin(lobbyReq.Payload.id);
                }
                else
                {
                    mm.Error("Could not create lobby");
                }
            }, mm.DefaultError);
        }
    }

    public void TryJoin(string id)
    {
        var lobbyReq = new JoinLobbyRequest(id, PlayerPrefs.GetString("username"));
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "join_lobby";
        req.FunctionParameter = lobbyReq;
        PlayFabClientAPI.ExecuteCloudScript<Lobby>(req, (ExecuteCloudScriptResult res) =>
        {
            JoinLobby((res.FunctionResult as Lobby).id);
        }, mm.DefaultError);

    }

    public void JoinLobby(string id)
    {
        PlayerPrefs.SetString("Lobby", id);
        mm.Gameplay();
    }

    IEnumerator AfterDelay(float sec, UnityAction ua)
    {
        yield return new WaitForSeconds(sec);
        ua();
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
        this.users = new List<string>();
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
    public JoinLobbyRequest(string lob, string usr)
    {
        this.Lobby = lob;
        this.user = usr;
    }

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
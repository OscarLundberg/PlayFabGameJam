using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class LobbyData : MonoBehaviour
{
    public Lobby Lobby;
    public TMP_Text textObject;
    public Button btn;
    public Matchmaking matchmaking;
    public string id;

    public void SetData(Lobby lobby, Matchmaking src)
    {
        Lobby = lobby;
        matchmaking = src;
        id = lobby.id;
    }



    public void UpdateText()
    {

        textObject.text = LobbyData.Format(Lobby);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            matchmaking.TryJoin(Lobby.id);
        });
        if (!Lobby.isJoinable)
        {
            btn.interactable = false;
        }
    }

    public static string Format(Lobby lobby)
    {
        return $"{lobby.name} [{lobby.users.Count} / 10]";
    }
}
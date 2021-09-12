using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EventsModels;
using PlayFab.Events;
using PlayFab.MultiplayerModels;

using UnityEngine.Events;

public class GameplayHandler : MonoBehaviour
{
    public MainMenu mm;

    public TMP_InputField chatmsg;
    public Button sendmsg;


    public string username;
    public string SessionTicket;
    public string MatchTicket;
    public string Lobby;


    // Start is called before the first frame update
    public void StartGame(MainMenu src)
    {
        mm = src;
        LoadPrefs();
    }


    public void LoadPrefs()
    {
        username = PlayerPrefs.GetString("username");
        SessionTicket = PlayerPrefs.GetString("SessionTicket");
        MatchTicket = PlayerPrefs.GetString("matchTicket");
        Lobby = PlayerPrefs.GetString("Lobby");
    }

    public void ChatCooldown()
    {
        chatmsg.interactable = false;
        sendmsg.interactable = false;
    }

    public void ChatReset()
    {
        chatmsg.interactable = true;
        sendmsg.interactable = true;
        chatmsg.Select();
    }

    public void Chat(TMP_InputField inp)
    {

        var message = new EventContents();
        message.Name = "Chat";
        message.Payload = new MessagePayload(Lobby, username, inp.text);

        // message.Payload =
        var req = new WriteEventsRequest();
        req.Events = new List<EventContents>() { message };
        ChatCooldown();
        PlayFabEventsAPI.WriteEvents(req, (WriteEventsResponse res) =>
        {
            StartCoroutine(AfterDelay(1, ChatReset));
            // res.AssignedEventIds()
        }, mm.DefaultError);

        // inp.text
    }

    void Poll()
    {
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "Receive";
        req.FunctionParameter = Lobby;

        PlayFabClientAPI.ExecuteCloudScript(req, (ExecuteCloudScriptResult res) =>
        {
            // res.FunctionResult();

        }, mm.DefaultError);
        // GetMatchRequest
        // PlayFabMultiplayerAPI.GetMatch()
    }


    IEnumerator AfterDelay(float sec, UnityAction ua)
    {
        yield return new WaitForSeconds(sec);
        ua();
    }

}

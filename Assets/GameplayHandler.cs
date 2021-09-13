using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EventsModels;
using PlayFab.Events;
using PlayFab.MultiplayerModels;

using UnityEngine.Events;

public class GameplayHandler : MonoBehaviour
{
    public static bool stopPolling = false;
    public MainMenu mm;

    public TMP_InputField chatmsg;
    public Button sendmsg;

    public ChatManager chatManager;

    public string username;
    public string SessionTicket;
    public string MatchTicket;
    public string Lobby;

    public GameObject gameplay;

    public static GameplayHandler instance;
    public List<MessagePayload> privateMessages;

    // Start is called before the first frame update
    public void StartGame(MainMenu src)
    {
        if (instance != null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        maxPolls = 0;
        mm = src;
        privateMessages = new List<MessagePayload>();
        LoadPrefs();
        gameplay.SetActive(true);
        SendPoll();
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
        EventSystem.current.SetSelectedGameObject(null);
        chatmsg.interactable = false;
        sendmsg.interactable = false;
    }

    public void ChatReset()
    {
        chatmsg.interactable = true;
        sendmsg.interactable = true;
        chatmsg.text = "";
        chatmsg.Select();
    }

    public void Chat(TMP_InputField inp)
    {

        var payload = new MessagePayload(Lobby, username, inp.text);
        ChatCooldown();
        SendEvent(payload, UpdateChat);
        StartCoroutine(AfterDelay(1, ChatReset));

    }


    public void SendEvent(MessagePayload payload, System.Action<ExecuteCloudScriptResult> callback)
    {
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "send_lobby_event";
        req.FunctionParameter = payload;

        PlayFabClientAPI.ExecuteCloudScript<ListLobbyEventsResponse>(req, callback, mm.DefaultError);
    }

    public static int maxPolls = 0;
    void SendPoll()
    {
        maxPolls++;
        if (maxPolls >= 2400)
            return;

        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "list_lobby_events";
        req.FunctionParameter = new Poll(Lobby);

        PlayFabClientAPI.ExecuteCloudScript<ListLobbyEventsResponse>(req, UpdateChat, mm.DefaultError);
        StartCoroutine(AfterDelay(1, SendPoll));
    }



    public void UpdateChat(ExecuteCloudScriptResult res)
    {
        chatManager.SetEvents(res.FunctionResult as ListLobbyEventsResponse);
    }

    IEnumerator AfterDelay(float sec, UnityAction ua)
    {
        yield return new WaitForSeconds(sec);
        ua();
    }

}

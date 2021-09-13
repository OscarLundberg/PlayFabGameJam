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
    public string role;
    public static bool isDay;
    public int gameStage;

    public GameObject gameplay;

    public static GameplayHandler instance;
    public List<MessagePayload> privateMessages;

    // Start is called before the first frame update
    public void StartGame(MainMenu src)
    {
        anim.SetTrigger("reset");
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        maxPolls = 0;
        mm = src;
        privateMessages = new List<MessagePayload>() { new MessagePayload("", "HOST", "You have joined the room", "", "Private") };
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
        req.FunctionParameter = new Poll(Lobby, username);

        PlayFabClientAPI.ExecuteCloudScript<ListLobbyEventsResponse>(req, UpdateChat, mm.DefaultError);
        StartCoroutine(AfterDelay(1, SendPoll));
    }
    public SkyAnimator sa;
    public void Day()
    {
        isDay = true;
        Gradient g = new Gradient();
        g.colorKeys = new GradientColorKey[] { new GradientColorKey(new Color(160, 255, 228), 0), new GradientColorKey(new Color(160, 255, 228), 0) };
        g.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0f, 0), new GradientAlphaKey(0.3f, 1) };
        sa.range = g;
    }

    public void Night()
    {
        isDay = false;
        Gradient g = new Gradient();
        g.colorKeys = new GradientColorKey[] { new GradientColorKey(new Color(118, 114, 161), 0), new GradientColorKey(new Color(118, 114, 161), 1) };
        g.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0f, 0), new GradientAlphaKey(0.3f, 1) };
        sa.range = g;
    }
    public Animator anim;
    public void Convicted()
    {
        anim.SetTrigger("jaildoor");
        AudioPlayer.instance.Convicted();
    }

    public void LeaveGame()
    {
        anim.SetTrigger("reset");
        var lobbyReq = new JoinLobbyRequest(Lobby, PlayerPrefs.GetString("username"));
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "leave_lobby";
        req.FunctionParameter = lobbyReq;
        PlayFabClientAPI.ExecuteCloudScript<Lobby>(req, (ExecuteCloudScriptResult res) =>
        {
            MainMenu.instance.LoggedIn();
        }, mm.DefaultError);

    }

    public void UpdateChat(ExecuteCloudScriptResult res)
    {
        List<MessagePayload> list = (res.FunctionResult as ListLobbyEventsResponse).events;
        list.AddRange(privateMessages);
        chatManager.SetEvents(list);
    }

    public void Target(string s)
    {
        TargetRequest target;
        if (isDay)
        {
            target = new TargetRequest(Lobby, username, s, "vote", gameStage);
        }
        else
        {
            target = new TargetRequest(Lobby, username, s, "rob", gameStage);
        }
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = "target";
        req.FunctionParameter = target;
        PlayFabClientAPI.ExecuteCloudScript<Lobby>(req, (ExecuteCloudScriptResult res) =>
        {
            MainMenu.instance.LoggedIn();
        }, mm.DefaultError);
    }

    IEnumerator AfterDelay(float sec, UnityAction ua)
    {
        yield return new WaitForSeconds(sec);
        ua();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
public class HostMethods : MonoBehaviour
{
    static bool pollLobby;
    static bool isHost = false;
    static string Lobby;
    static Lobby lobbyState;
    static string user;

    public GameObject startGameButton;

    public void CreatedRoom(string id)
    {
        startGameButton.SetActive(true);
        HostMethods.isHost = true;
        HostMethods.Lobby = id;
        HostMethods.user = PlayerPrefs.GetString("username");
        PollLobby();
    }

    public void PollLobby()
    {
        StartCoroutine(Poll(2, () =>
        {
            var param = new Poll(HostMethods.Lobby, HostMethods.user);
            RequestAndBroadcast<Lobby>("lobby_poll", param);
            return HostMethods.pollLobby;
        }));
    }

    public void StartGame()
    {
        HostMethods.pollLobby = false;
        var param = new Poll(HostMethods.Lobby, HostMethods.user);
        SimpleRequest("start_game", param);
        BroadcastToClients("StartGame", true);
    }

    public static void BroadcastToClients(string message, object param)
    {
        GameplayHandler.instance.SendEvent(new MessagePayload(HostMethods.Lobby, "HOST", message, JsonUtility.ToJson(param)), GameplayHandler.instance.UpdateChat);
    }


    IEnumerator AfterSeconds(int freq, Action callback)
    {
        yield return new WaitForSeconds(freq);
        callback();
    }


    IEnumerator Poll(int freq, Func<bool> callback)
    {
        yield return new WaitForSeconds(freq);
        if (callback())
        {
            StartCoroutine(Poll(freq, callback));
        }
    }

    public static void RequestAndBroadcast<T>(string method, object param)
    {
        SimpleRequest<T>(method, param, (ExecuteCloudScriptResult res) =>
           {
               T output = (T)res.FunctionResult;
               BroadcastToClients(method, output);
           });
    }

    static void EmptyCallback(ExecuteCloudScriptResult res) { }


    public static void SimpleRequest(string method, object param)
    {
        SimpleRequest<object>(method, param, EmptyCallback);
    }

    public static void SimpleRequest<T>(string method, object param, Action<ExecuteCloudScriptResult> callback)
    {
        var req = new ExecuteCloudScriptRequest();
        req.FunctionName = method;
        req.FunctionParameter = param;
        PlayFabClientAPI.ExecuteCloudScript<T>(req, callback, DefaultError);
    }

    public static void DefaultError(PlayFabError err)
    {
        Debug.Log(err);
    }
}

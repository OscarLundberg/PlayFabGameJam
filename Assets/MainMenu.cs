using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;

public class MainMenu : MonoBehaviour
{
    public TMP_Text errorText;
    public GameObject login;
    public GameObject mainMenu;
    public TMP_Text username;
    public TMP_InputField login_username;
    public TMP_InputField login_password;
    public GameplayHandler gh;

    void Start()
    {
        Logout();
    }

    // public void Login()
    // {

    //     if (login_username.text.Length > 4)
    //     {
    //         var req = new LoginWithEmailAddressRequest();
    //         // req.CustomId = login_username.text;
    //         // req.CreateAccount = true;
    //         // req.TitleId = Endpoints.TitleID;
    //         req.
    //         PlayFabClientAPI.LoginWithEmailAddress(req, (LoginResult res) =>
    //         {
    //             PlayerPrefs.SetString("username", login_username.text);
    //             PlayerPrefs.SetString("SessionTicket", res.SessionTicket);
    //             PlayerPrefs.Save();
    //             LoggedIn();
    //         }, (PlayFabError err) =>
    //         {
    //             Error(err.ErrorMessage);
    //         });
    //     }
    //     else
    //     {
    //         Error("Please enter a username");
    //     }
    // }

    public void Gameplay()
    {
        mainMenu.SetActive(false);
        login.SetActive(false);
        gh.StartGame(this);
    }

    public void FindMatch()
    {
        var ticket = PlayerPrefs.GetString("SessionTicket"); ;
        var stats = new GetQueueStatisticsRequest();
        stats.QueueName = "default-queue";
        stats.AuthenticationContext.ClientSessionTicket = ticket;
        PlayFab.PlayFabMultiplayerAPI.GetQueueStatistics(stats, (GetQueueStatisticsResult res) =>
        {
            var status = $"{res.NumberOfPlayersMatching} players in queue. Estimated time until match: {res.TimeToMatchStatisticsInSeconds}";
            Error(status);
        }, (PlayFabError err) =>
        {
            Error(err.ErrorMessage);
        });


        Error("Finding Match...");
        var req = new MatchmakeRequest();
        req.AuthenticationContext.ClientSessionTicket = ticket;
        req.BuildVersion = "default";
        req.Region = Region.USEast;
        req.GameMode = "default";
        PlayFabClientAPI.Matchmake(req, (MatchmakeResult mr) =>
        {
            PlayerPrefs.SetString("Lobby", mr.LobbyID);
            PlayerPrefs.SetString("MatchTicket", mr.Ticket);
            PlayerPrefs.Save();
            Gameplay();

        }, DefaultError);
    }

    public void GuestLogin()
    {

        if (login_username.text.Length > 4)
        {
            var req = new LoginWithCustomIDRequest();
            req.CustomId = login_username.text;
            req.CreateAccount = true;
            req.TitleId = Endpoints.TitleID;
            PlayFabClientAPI.LoginWithCustomID(req, (LoginResult res) =>
            {
                PlayerPrefs.SetString("username", login_username.text);
                PlayerPrefs.SetString("SessionTicket", res.SessionTicket);
                PlayerPrefs.Save();
                LoggedIn();
            }, DefaultError);
        }
        else
        {
            Error("Please enter a username");
        }
    }

    public void LoggedIn()
    {
        Error("");
        username.text = PlayerPrefs.GetString("username");
        login.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Logout()
    {
        Error("");
        mainMenu.SetActive(false);
        login.SetActive(true);
    }

    public void Error(string msg)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = msg;
    }

    public void DefaultError(PlayFabError err)
    {
        Error(err.ErrorMessage);
    }

}
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
    public GameObject matchmaking;

    void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            login_username.text = PlayerPrefs.GetString("username");
            GuestLogin();
        }
        else
        {
            Logout();
        }
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
        DisableAll();
        gh.StartGame(this);
    }

    public void FindMatch()
    {
        DisableAll();
        matchmaking.SetActive(true);
        matchmaking.GetComponent<Matchmaking>().Search(this);
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
        DisableAll();
        mainMenu.SetActive(true);
    }

    public void Logout()
    {
        Error("");
        DisableAll();
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

    public void DisableAll()
    {
        login.SetActive(false);
        mainMenu.SetActive(false);
        matchmaking.SetActive(false);


    }

}

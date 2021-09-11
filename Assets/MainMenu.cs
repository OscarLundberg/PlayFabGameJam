using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class MainMenu : MonoBehaviour
{
    public TMP_Text errorText;
    public GameObject login;
    public GameObject mainMenu;
    public void Login(TMP_InputField inputField)
    {
        if (inputField.text.Length > 4)
        {
            Gateway.SendRequest(Endpoints.LOGIN, new LoginRequest(inputField.text), (string response) =>
            {
                var res = JsonUtility.FromJson<Response<LoginResponse>>(response);
                if (res.data.SessionTicket != null)
                {
                    PlayerPrefs.SetString("username", inputField.text);
                    PlayerPrefs.SetString("SessionTicket", res.data.SessionTicket);
                    PlayerPrefs.Save();
                    login.SetActive(false);
                    mainMenu.SetActive(true);
                    Error("Success");
                }
                else
                {
                    Error(response);
                }
            });
        }
        else
        {
            Error("Username invalid. Please try another username");
        }
    }

    public void Error(string msg)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = msg;
    }

}

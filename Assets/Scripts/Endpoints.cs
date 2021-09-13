using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoints
{
    static string baseEndpoint = "https://4885D.playfabapi.com/";
    public static string TitleID = "4885D";

    public static string LOGIN
    {
        get
        {
            return baseEndpoint + "Client/LoginWithCustomID";
        }
    }
}

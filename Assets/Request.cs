using UnityEngine;

[System.Serializable]
public class Request
{
    [SerializeField]
    public string TitleId = Endpoints.TitleID;
    public Request()
    {

    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

[System.Serializable]
public class LoginRequest : Request
{
    [SerializeField]
    public bool CreateAccount = true;
    [SerializeField]
    public string CustomId;

    public LoginRequest(string id)
    {
        CustomId = id;
    }
}






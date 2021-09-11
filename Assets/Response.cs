using UnityEngine;

[System.Serializable]
public class Response<T>
{
    [SerializeField]
    public string statusCode;
    public T data;
}

[System.Serializable]
public class DefaultResponseData
{
    [SerializeField]
    string key;
}

[System.Serializable]
public class LoginResponse
{
    [SerializeField]
    public string SessionTicket;
}



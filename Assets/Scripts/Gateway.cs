using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Gateway : MonoBehaviour
{
    public static Gateway instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public static void SendRequest(string endpoint, string body, UnityAction<string> ua = null)
    {
        instance.DoSendRequest(endpoint, body, ua);
    }

    public static void SendRequest(string endpoint, Request body, UnityAction<string> ua = null)
    {
        Gateway.SendRequest(endpoint, body.ToJson(), ua);
    }

    public void DoSendRequest(string endpoint, string body, UnityAction<string> ua = null)
    {
        StartCoroutine(PostRequest(endpoint, body, ua));
    }

    IEnumerator PostRequest(string endpoint, string body, UnityAction<string> ua = null)
    {
        Debug.Log(endpoint);
        Debug.Log(body);
        UploadHandler uh = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
        uh.contentType = "application/json";
        UnityWebRequest www = new UnityWebRequest(endpoint, "POST", new DownloadHandlerBuffer(), uh);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            ua(www.downloadHandler.text);
        }
    }
}

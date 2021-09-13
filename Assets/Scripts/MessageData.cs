using UnityEngine;
using TMPro;

public class MessageData : MonoBehaviour
{
    public MessagePayload messagePayload;
    public TMP_Text textObject;
    public void SetData(MessagePayload mp)
    {
        if (mp.type == "System")
        {
            GameplayHandler.instance.SendMessage(mp.message, mp.param);
            gameObject.SetActive(false);
        }
        else if (mp.type == "Private")
        {
            UpdateText("<color=yellow>" + mp.message);
        }
        else
        {
            messagePayload = mp;
            UpdateText(MessageData.Format(messagePayload));
        }
    }

    public void UpdateText(string text)
    {
        textObject.text = text;
    }

    public static string Format(MessagePayload mp)
    {

        return $"[{mp.timestamp.ToShortTimeString()}] {mp.sender}: {mp.message}";
    }
}
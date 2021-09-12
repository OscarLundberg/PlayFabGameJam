using UnityEngine;
using TMPro;

public class MessageData : MonoBehaviour
{
    public MessagePayload messagePayload;
    public TMP_Text textObject;
    public void SetData(MessagePayload mp)
    {
        messagePayload = mp;
        UpdateText();
    }

    public void UpdateText()
    {
        textObject.text = MessageData.Format(messagePayload);
    }

    public static string Format(MessagePayload mp)
    {
        return $"[{mp.timestamp.ToShortTimeString()}] {mp.sender}: {mp.message}";
    }
}
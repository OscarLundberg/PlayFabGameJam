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
            //
        }
        else
        {
            messagePayload = mp;
            UpdateText();
        }
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
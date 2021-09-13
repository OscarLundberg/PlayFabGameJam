using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform parent;

    public void SetEvents(List<MessagePayload> events)
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        events.Sort(new CompareMessages());
        foreach (MessagePayload mp in events)
        {
            var go = (Instantiate(messagePrefab, parent) as GameObject).GetComponent<MessageData>();
            go.SetData(mp);
        }
    }
}

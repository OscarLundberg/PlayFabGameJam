using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform parent;

    public void SetEvents(ListLobbyEventsResponse response)
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        response.events.Sort(new CompareMessages());
        foreach (MessagePayload mp in response.events)
        {
            var go = (Instantiate(messagePrefab, parent) as GameObject).GetComponent<MessageData>();
            go.SetData(mp);
        }
    }
}

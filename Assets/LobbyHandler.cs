using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform parent;

    public void SetEvents(Lobby lobby)
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        foreach (string payload in lobby.users)
        {
            var go = (Instantiate(playerPrefab, parent) as GameObject).GetComponent<PlayerData>();
            go.SetData(payload);
        }
    }
}

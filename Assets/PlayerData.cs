using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public string player;
    public TMP_Text textObject;
    public void SetData(string player)
    {
        this.player = player;
    }

    public void UpdateText()
    {
        textObject.text = player;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public string player;
    public TMP_Text textObject;
    public Button btn;
    public void SetData(string player)
    {
        this.player = player;
        btn.onClick.AddListener(() =>
        {
            GameplayHandler.instance.Target(player);
        });
    }

    public void UpdateText()
    {
        textObject.text = player;
    }

}

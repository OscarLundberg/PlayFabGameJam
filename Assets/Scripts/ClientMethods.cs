using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMethods : MonoBehaviour
{

    public LobbyHandler lh;
    public void lobby_poll(string param)
    {
        lh.SetEvents(JsonUtility.FromJson<Lobby>(param));
    }

    public void StartGame()
    {
        Debug.Log("the game has started");
    }

    public void ProgressGame()
    {
        AudioPlayer.instance.RndClip();
    }

    public void assign_role(string json)
    {
        RoleAssignment ra = JsonUtility.FromJson<RoleAssignment>(json);
        if (ra.user == GameplayHandler.instance.username)
        {
            GameplayHandler.instance.role = ra.role;
        }
    }

    public void set_game_stage(string stage)
    {
        GameplayHandler.instance.gameStage = int.Parse(stage);
    }

    public void was_robbed()
    {
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "You have been robbed... Lost 500 gold", "", "Private"));
    }

    public void was_convicted(string player)
    {
        if (player == GameplayHandler.instance.username)
            GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "You have been convicted... ", "", "Private"));
    }

    public void WhoWasConvicted(string param)
    {
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", param, "", "Private"));
    }

    public void Night()
    {
        AudioPlayer.instance.Night();
        GameplayHandler.instance.Night();
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "Night falls... ", "", "Private"));
    }

    public void Day()
    {
        AudioPlayer.instance.Morning();
        GameplayHandler.instance.Day();
        GameplayHandler.instance.privateMessages.Add(new MessagePayload("", "HOST", "A new day breaks... ", "", "Private"));
    }
}


public class RoleAssignment
{
    public string user;
    public string role;

    public RoleAssignment(string user, string role)
    {
        this.user = user;
        this.role = role;
    }

}
public class SimpleWrapper
{
    string str;
    int num;
    public SimpleWrapper(string str)
    {
        this.str = str;
    }
    public SimpleWrapper(int num)
    {
        this.num = num;
    }

}
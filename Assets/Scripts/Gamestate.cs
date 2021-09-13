using System.Collections;
using System.Collections.Generic;

// state {
//     players: [{name:nm, role:role}],
//     stages: [{
//          convicted:[""],
//          robbed: [],
//     }],
//     stage: -1
// }

public class GameState
{
    public List<Player> players;
    public List<Stage> stages;
    public int stage;
}

public class Stage
{
    public List<string> convicted;
    public List<string> robbed;
}

public class Player
{
    public string name;
    public string role;

}
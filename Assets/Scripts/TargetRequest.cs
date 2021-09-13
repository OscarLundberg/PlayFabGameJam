public class TargetRequest
{

    public string Lobby;
    public string user;
    public string target;
    public string reason;
    public int stage;

    public TargetRequest(string lobby, string user, string target, string reason, int stage)
    {
        Lobby = lobby;
        this.user = user;
        this.target = target;
        this.reason = reason;
        this.stage = stage;
    }
}
